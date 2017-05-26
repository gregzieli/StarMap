using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System;

namespace StarMap.Bll.Managers
{
  [Obsolete("I went with the full on-platform implementation to get the device rotation.")]
  public class SensorManager
  {
    const MotionSensorType ACCELEROMETER = MotionSensorType.Accelerometer;
    const MotionSensorType MAGNETOMETER = MotionSensorType.Magnetometer;

    public double[] Gravity { get; private set; } = new double[3];

    public double[] Magnet { get; private set; } = new double[3];

    public void Start()
    {
      if (!CrossDeviceMotion.Current.IsActive(ACCELEROMETER))
        CrossDeviceMotion.Current.Start(ACCELEROMETER, MotionSensorDelay.Default);
      if (!CrossDeviceMotion.Current.IsActive(MAGNETOMETER))
        CrossDeviceMotion.Current.Start(MAGNETOMETER, MotionSensorDelay.Default);

      // Subscribe
      CrossDeviceMotion.Current.SensorValueChanged += ReadSensor;
    }

    public void Stop()
    {
      if (CrossDeviceMotion.Current.IsActive(ACCELEROMETER))
        CrossDeviceMotion.Current.Stop(ACCELEROMETER);
      if (CrossDeviceMotion.Current.IsActive(MAGNETOMETER))
        CrossDeviceMotion.Current.Stop(MAGNETOMETER);

      // Unsubscribe
      CrossDeviceMotion.Current.SensorValueChanged -= ReadSensor;
    }

    void ReadSensor(object sender, SensorValueChangedEventArgs e)
    {
      var vector = e.Value as MotionVector;
      switch (e.SensorType)
      {
        case ACCELEROMETER:
          SetValues(Gravity, vector);
          System.Diagnostics.Debug.WriteLine($"A: {Gravity[0]}, {Gravity[1]}, {Gravity[2]}");
          return;
        case MAGNETOMETER:
          SetValues(Magnet, vector);
          System.Diagnostics.Debug.WriteLine($"M: {Magnet[0]}, {Magnet[1]}, {Magnet[2]}");
          return;
      }
    }

    void SetValues(double[] holder, MotionVector vector)
    {
      holder[0] = vector.X;
      holder[1] = vector.Y;
      holder[2] = vector.Z;
    }
  }
}
