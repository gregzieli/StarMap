using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using StarMap.Cll.Abstractions.Services;
using StarMap.Core.Models;
using StarMap.Droid.Sensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using Axis = Android.Hardware.Axis;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceRotationImplementation))]
namespace StarMap.Droid.Sensors
{
  // Thanks to Java.Lang.Object, I don't have to (neither should I) implement the IntPtr Handle from the IJavaObject interface
  // (inherited by ISensorEventListener).
  [Preserve(AllMembers = true)]
  public class DeviceRotationImplementation : Java.Lang.Object, ISensorEventListener, IDeviceRotation, IDisposable
  {
    IWindowManager _windowManager;
    SensorManager _sensorManager;

    Sensor _accelerometer;
    Sensor _magnetometer;
    
    bool _on, _accReady, _magReady;
    float[] _gravity = new float[3],
      _magnet = new float[3],
      _orientation = new float[3],
      R = new float[9],
      rotatedR = new float[9];

    int lastUpdate = 0;

    static readonly object _lock = new object();

    public DeviceRotationImplementation() : base()
    {
      Init();
    }

    public void Init()
    {
      var context = Application.Context;
      _windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>(); // TODO: check if this funny casting is a must
      _sensorManager = (SensorManager)context.GetSystemService(Context.SensorService);

      _accelerometer = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);
      _magnetometer = _sensorManager.GetDefaultSensor(SensorType.MagneticField);
    }

    #region ISensorEventListener implementation

    public void OnSensorChanged(SensorEvent e)
    {
      // Neither the emulator, nor my LG Spirit, give higher readings than this.
      //if (e.Accuracy == SensorStatus.Unreliable) return;

      // I don't think the lock is needed. Threads don't get confused on which sensor fired the event.
      // Maybe it's to prevent from raising RotationVhanged event multiple times, by many threads, then OK.
      // But e.g. in standard Windows application, Filewatcher catches events synchronously, one thread at a time, even if 
      // multuiple events are raised simultanously - each one waits for it's turn. From what I see here it's the same.
      lock (_lock)
      {
        switch (e.Sensor.Type)
        {
          case SensorType.Accelerometer:
            if (!_accReady)
            {
              //e.Values.CopyTo(_gravity, 0);
              // check if this works better
              for (int i = 0; i < e.Values.Count; i++)
                _gravity[i] = e.Values[i];

              _accReady = true;
            }              
            break;
          case SensorType.MagneticField:
            if (!_magReady)
            {
              //e.Values.CopyTo(_magnet, 0);
              for (int i = 0; i < e.Values.Count; i++)
                _magnet[i] = e.Values[i];

              _magReady = true;
            }              
            break;
        }

        if (!(_accReady && _magReady))
          return;

        _accReady = _magReady = false;

        //var time = Environment.TickCount;
        //if ((time - lastUpdate) < 3000)
        //  return;
        //lastUpdate = time;

        SensorManager.GetRotationMatrix(R, null, _gravity, _magnet);


        // Cellphone's natural orientation is portrait, tilted to the left it returns display (rotation) = 1,
        // to the right = 3.
        // However, a tablet is already in landscape, so the natural application displays are 0 or 2.
        Axis x = Axis.X, y = Axis.Y;
        switch (_windowManager.DefaultDisplay.Rotation)
        {
          case SurfaceOrientation.Rotation180:
            x = Axis.MinusX;
            y = Axis.MinusY;
            break;
          case SurfaceOrientation.Rotation270:
            x = Axis.MinusY; // TODO: check if they are not in fact inverted.
            y = Axis.X;
            break;
          case SurfaceOrientation.Rotation90: // phone tilted to the left
            x = Axis.Y;
            y = Axis.MinusX;
            break;
          default:
            break;
        }

        SensorManager.RemapCoordinateSystem(R, x, y, rotatedR);
        SensorManager.GetOrientation(rotatedR, _orientation);
        
        _rotationChanged?.Invoke(this, new RotationChangedEventArgs(_orientation));
      }
    }    

    public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }

    #endregion

    #region IDeviceRotation implementation

    static readonly object _eventLocker = new object();
    EventHandler<RotationChangedEventArgs> _rotationChanged;
    // This nice piece of code ensures that only one Handler is attached to the event.
    // No need to worry about any checks. Alternatively, I could expose some 'Subscribe' and 'Unsubscribe'
    // methods, that would take an Action and hide the event altogether.
    public event EventHandler<RotationChangedEventArgs> RotationChanged
    {
      [SecurityCritical] // suggested by Code Analysis
      add
      {
        lock (_eventLocker)
        {
          _rotationChanged -= value;
          _rotationChanged += value;
        }
      }
      [SecurityCritical]
      remove
      {
        lock (_eventLocker)
        {
          _rotationChanged -= value;
        }
      }
    }

    [SecurityCritical]
    public void Start()
    {
      if (_on) return;

      _sensorManager.RegisterListener(this, _accelerometer, SensorDelay.Normal);
      _sensorManager.RegisterListener(this, _magnetometer, SensorDelay.Normal);

      _on = true;
    }

    [SecurityCritical]
    public void Stop()
    {
      if (!_on) return;

      if (_accelerometer != null)
        _sensorManager?.UnregisterListener(this, _accelerometer);
      if (_magnetometer != null)
        _sensorManager?.UnregisterListener(this, _magnetometer);

      _on = false;
    }


    #endregion



    // Auto-generated by auto implementing the interface.
    #region IDisposable Support

    private bool disposed = false; // To detect redundant calls
    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual new void Dispose(bool disposing)
    {
      // free unmanaged resources (unmanaged objects) and override a finalizer below.
      // set large fields to null.
      if (disposed)
        return;

      if (disposing)
      {
        Stop();
        _sensorManager?.Dispose();
        _sensorManager = null;
        _accelerometer.Dispose();
        _accelerometer = null;
        _magnetometer.Dispose();
        _magnetometer = null;
      }

      disposed = true;
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    ~DeviceRotationImplementation()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(false);
    }
        
    #endregion
    
  }
}