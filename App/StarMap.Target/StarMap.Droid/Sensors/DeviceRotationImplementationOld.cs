﻿//using Android.App;
//using Android.Content;
//using Android.Hardware;
//using Android.Runtime;
//using Android.Views;
//using StarMap.Cll.Abstractions.Services;
//using StarMap.Core.Models;
//using StarMap.Droid.Sensors;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Security;
//using Axis = Android.Hardware.Axis;

//[assembly: Xamarin.Forms.Dependency(typeof(DeviceRotationImplementation))]
//namespace StarMap.Droid.Sensors
//{
//  // Thanks to Java.Lang.Object, I don't have to (neither should I) implement the IntPtr Handle from the IJavaObject interface
//  // (inherited by ISensorEventListener).
//  [Preserve(AllMembers = true)]
//  [Obsolete("Turns out, there is no need to calculate GetOrientation, rotationMatrix is all Urho needs.")]
//  public class DeviceRotationImplementationOld : Java.Lang.Object, ISensorEventListener, IDeviceRotation, IDisposable
//  {
//    IWindowManager _windowManager;
//    SensorManager _sensorManager;

//    Sensor _gravitySensor;
//    Sensor _magnetometer;
    
//    bool _on, _gReady, _magReady;
//    float[] _gravity = new float[3],
//      _magnet = new float[3],
//      _orientation = new float[3],
//      R = new float[9],
//      rotatedR = new float[9];

//    static object _lock = new object();

//    public DeviceRotationImplementationOld() : base()
//    {
//      Init();
//    }

//    public void Init()
//    {
//      var context = Application.Context;
//      _windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>(); 
//      _sensorManager = (SensorManager)context.GetSystemService(Context.SensorService);

//      _gravitySensor = _sensorManager.GetDefaultSensor(SensorType.Gravity);
//      _magnetometer = _sensorManager.GetDefaultSensor(SensorType.MagneticField);
//    }

//    #region ISensorEventListener implementation
    
//    public void OnSensorChanged(SensorEvent e)
//    {
//      // Neither the emulator, nor some real devices, give higher readings than this.
//      if (e.Accuracy == SensorStatus.Unreliable) return;

//      // I don't think the lock is needed. Threads don't get confused on which sensor fired the event.
//      // Maybe it's to prevent from raising RotationVhanged event multiple times, by many threads, then OK.
//      // But e.g. in standard Windows application, Filewatcher catches events synchronously, one thread at a time, even if 
//      // multuiple events are raised simultanously - each one waits for it's turn. From what I see here it's the same.
//      lock (_lock)
//      {
//        switch (e.Sensor.Type)
//        {
//          case SensorType.Gravity:
//            if (!_gReady)
//            {
//              e.Values.CopyTo(_gravity, 0);

//              _gReady = true;
//            }              
//            break;
//          case SensorType.MagneticField:
//            if (!_magReady)
//            {
//              e.Values.CopyTo(_magnet, 0);

//              _magReady = true;
//            }              
//            break;
//        }

//        if (!(_gReady && _magReady))
//          return;

//        _gReady = _magReady = false;

//        SensorManager.GetRotationMatrix(R, null, _gravity, _magnet);


//        // Cellphone's natural orientation is portrait, tilted to the left it returns display (rotation) = 1,
//        // to the right = 3.
//        // However, a tablet is already in landscape, so the natural application displays are 0 or 2.
//        Axis x = Axis.X, y = Axis.Y;
//        switch (_windowManager.DefaultDisplay.Rotation)
//        {
//          case SurfaceOrientation.Rotation180:
//            x = Axis.MinusX;
//            y = Axis.MinusY;
//            break;
//          case SurfaceOrientation.Rotation270:
//            x = Axis.MinusY; 
//            y = Axis.X;
//            break;
//          case SurfaceOrientation.Rotation90: // phone tilted to the left
//            x = Axis.Y;
//            y = Axis.MinusX;
//            break;
//          default:
//            break;
//        }

//        SensorManager.RemapCoordinateSystem(R, x, y, rotatedR);
//        SensorManager.GetOrientation(rotatedR, _orientation);
        
//        _rotationChanged?.Invoke(this, new RotationChangedEventArgs(_orientation));
//      }
//    }    

//    public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }

//    #endregion

//    #region IDeviceRotation implementation

//    static readonly object _eventLocker = new object();
//    EventHandler<RotationChangedEventArgs> _rotationChanged;
//    // This nice piece of code ensures that only one Handler is attached to the event.
//    // No need to worry about any checks. Alternatively, I could expose some 'Subscribe' and 'Unsubscribe'
//    // methods, that would take an Action and hide the event altogether.
//    public event EventHandler<RotationChangedEventArgs> RotationChanged
//    {
//      [SecurityCritical] // suggested by Code Analysis
//      add
//      {
//        lock (_eventLocker)
//        {
//          _rotationChanged -= value;
//          _rotationChanged += value;
//        }
//      }
//      [SecurityCritical]
//      remove
//      {
//        lock (_eventLocker)
//        {
//          _rotationChanged -= value;
//        }
//      }
//    }

//    [SecurityCritical]
//    public void Start()
//    {
//      if (_on) return;

//      _sensorManager.RegisterListener(this, _gravitySensor, SensorDelay.Ui);
//      _sensorManager.RegisterListener(this, _magnetometer, SensorDelay.Ui);

//      _on = true;
//    }

//    [SecurityCritical]
//    public void Stop()
//    {
//      if (!_on) return;

//      if (_gravitySensor != null)
//        _sensorManager?.UnregisterListener(this, _gravitySensor);
//      if (_magnetometer != null)
//        _sensorManager?.UnregisterListener(this, _magnetometer);

//      _on = false;
//    }


//    #endregion



//    // Auto-generated by auto implementing the interface.
//    #region IDisposable Support

//    private bool disposed = false; // To detect redundant calls
//    // This code added to correctly implement the disposable pattern.
//    public void Dispose()
//    {
//      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//      Dispose(true);
//      GC.SuppressFinalize(this);
//    }

//    protected virtual new void Dispose(bool disposing)
//    {
//      // free unmanaged resources (unmanaged objects) and override a finalizer below.
//      // set large fields to null.
//      if (disposed)
//        return;

//      if (disposing)
//      {
//        Stop();
//        _sensorManager?.Dispose();
//        _sensorManager = null;
//        _gravitySensor.Dispose();
//        _gravitySensor = null;
//        _magnetometer.Dispose();
//        _magnetometer = null;
//      }

//      disposed = true;
//    }

//    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
//    ~DeviceRotationImplementationOld()
//    {
//      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//      Dispose(false);
//    }
        
//    #endregion
    
//  }
//}