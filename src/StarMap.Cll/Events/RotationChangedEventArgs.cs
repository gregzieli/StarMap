using System;
using Urho;

namespace StarMap.Cll.Events
{
    public class RotationChangedEventArgs : EventArgs
    {
        public RotationChangedEventArgs(float[] rotation)
        {
            Rotation = new Matrix3(rotation);
        }

        public Matrix3 Rotation { get; private set; }
    }
}
