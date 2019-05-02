using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Views;
using StarMap.Cll.Abstractions.Services;
using StarMap.Cll.Events;
using StarMap.Droid.Sensors;
using System;
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
        private IWindowManager _windowManager;
        private SensorManager _sensorManager;
        private Sensor _gravitySensor;
        private Sensor _magnetometer;
        private bool _on, _gReady, _magReady;
        private Axis _currentX, _currentY;
        private readonly float[] _gravity = new float[3];
        private readonly float[] _magnet = new float[3];
        private readonly float[] R = new float[9];
        private readonly float[] rotatedR = new float[9];

        static readonly object _lock = new object();

        public DeviceRotationImplementation() : base()
        {
            Init();
        }

        public void Init()
        {
            var context = Application.Context;
            _windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            _sensorManager = (SensorManager)context.GetSystemService(Context.SensorService);

            // Gravity is smoother than the accelerometer.
            _gravitySensor = _sensorManager.GetDefaultSensor(SensorType.Gravity);
            _magnetometer = _sensorManager.GetDefaultSensor(SensorType.MagneticField);

            ReorientDevice();
        }

        /// <summary>
        /// Sets device's axes to app's coordinate system.
        /// </summary>
        /// <remarks>
        /// StarMap has locked screen orientation: landscape. 
        /// On phone this is <see cref="SurfaceOrientation.Rotation90"/>.
        /// On tablet this is already <see cref="SurfaceOrientation.Rotation0"/>.
        /// </remarks>
        private void ReorientDevice()
        {
            // Phone's natural orientation is portrait, in landscape orientation (tilted to the left) it returns Rotation90,
            //
            // Tablet's is already in landscape, so the natural application displays are 0 or 2.
            switch (_windowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    _currentX = Axis.X;
                    _currentY = Axis.Y;
                    break;
                case SurfaceOrientation.Rotation90:
                    _currentX = Axis.MinusY;
                    _currentY = Axis.X;
                    break;

                // These will never be used. Just for reference.
                case SurfaceOrientation.Rotation180:
                    _currentX = Axis.MinusX;
                    _currentY = Axis.MinusY;
                    break;
                case SurfaceOrientation.Rotation270:
                    _currentX = Axis.Y;
                    _currentY = Axis.MinusX;
                    break;
            }
        }

        #region ISensorEventListener implementation

        public void OnSensorChanged(SensorEvent e)
        {
            // Neither the emulator, nor some real devices, give higher readings than this.
            if (e.Accuracy == SensorStatus.Unreliable)
                return;

            lock (_lock)
            {
                switch (e.Sensor.Type)
                {
                    case SensorType.Gravity:
                        if (!_gReady)
                        {
                            e.Values.CopyTo(_gravity, 0);

                            _gReady = true;
                        }
                        break;
                    case SensorType.MagneticField:
                        if (!_magReady)
                        {
                            e.Values.CopyTo(_magnet, 0);

                            _magReady = true;
                        }
                        break;
                }

                if (!(_gReady && _magReady))
                    return;

                _gReady = _magReady = false;

                SensorManager.GetRotationMatrix(R, null, _gravity, _magnet);
                if (SensorManager.RemapCoordinateSystem(R, _currentX, _currentY, rotatedR))
                    _rotationChanged?.Invoke(this, new RotationChangedEventArgs(rotatedR));
            }
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }

        #endregion

        #region IDeviceRotation implementation

        private static readonly object EventLocker = new object();
        private EventHandler<RotationChangedEventArgs> _rotationChanged;
        // This nice piece of code ensures that only one Handler is attached to the event.
        // No need to worry about any checks. Alternatively, I could expose some 'Subscribe' and 'Unsubscribe'
        // methods, that would take an Action and hide the event altogether.
        public event EventHandler<RotationChangedEventArgs> RotationChanged
        {
            [SecurityCritical] // suggested by Code Analysis
            add
            {
                lock (EventLocker)
                {
                    _rotationChanged -= value;
                    _rotationChanged += value;
                }
            }
            [SecurityCritical]
            remove
            {
                lock (EventLocker)
                {
                    _rotationChanged -= value;
                }
            }
        }

        [SecurityCritical]
        public void Start()
        {
            if (_on)
                return;

            _sensorManager.RegisterListener(this, _gravitySensor, SensorDelay.Ui);
            _sensorManager.RegisterListener(this, _magnetometer, SensorDelay.Ui);

            _on = true;
        }

        [SecurityCritical]
        public void Stop()
        {
            if (!_on)
                return;

            if (_gravitySensor != null)
                _sensorManager?.UnregisterListener(this, _gravitySensor);
            if (_magnetometer != null)
                _sensorManager?.UnregisterListener(this, _magnetometer);

            _on = false;
        }


        #endregion



        // Auto-generated by auto implementing the interface.
        #region IDisposable Support

        private bool _disposed = false; // To detect redundant calls
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
            if (_disposed)
                return;

            if (disposing)
            {
                Stop();
                _sensorManager?.Dispose();
                _sensorManager = null;
                _gravitySensor.Dispose();
                _gravitySensor = null;
                _magnetometer.Dispose();
                _magnetometer = null;
            }

            _disposed = true;
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