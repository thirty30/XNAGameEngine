using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public enum LightType
    {
        DIRECTION,
        POINT,
        SPOT
    }

    public class Light : Component
    {
        public LightType Type;
        public Vector3 LightColor = Vector3.One;
        public float Attenuation;
        public float CutOffDistance;
        public float InnerAngle;
        public float OuterAngle;

        public Light()
        {
            LightSystem.GetSingleton().AddLight(this);
        }

        ~Light()
        {
            LightSystem.GetSingleton().RemoveLight(this);
        }

        public virtual Vector3 GetLightDirection()
        {
            float x = MathHelper.ToRadians(this.gameObject.LocalRotation.X);
            float y = MathHelper.ToRadians(this.gameObject.LocalRotation.Y);
            float z = MathHelper.ToRadians(this.gameObject.LocalRotation.Z);
            Quaternion quatRotation = Quaternion.CreateFromYawPitchRoll(y, x, z);
            Matrix matRotation = Matrix.CreateFromQuaternion(quatRotation);
            return Vector3.Transform(Vector3.Forward, matRotation);
        }
    }

}
