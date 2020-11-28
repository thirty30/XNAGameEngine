using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class MultiLightMaterial : IMaterial
    {
        public Texture2D Tex1;
        private int[] mLightType = new int[8];
        private Vector3[] mLightPosition = new Vector3[8];
        private Vector3[] mLightDir = new Vector3[8];
        private Vector3[] mLightColor = new Vector3[8];
        private float[] mLightAttenuation = new float[8];
        private float[] mLightCutOffDistance = new float[8];
        private float[] mConeAngle = new float[8];

        public override void OnDraw(Matrix aM, Matrix aV, Matrix aP)
        {
            Matrix MVP = aM * aV * aP;
            Matrix MTI = Matrix.Transpose(Matrix.Invert(aM));
            this.Shader.Parameters["matM"].SetValue(aM);
            this.Shader.Parameters["matMVP"].SetValue(MVP);
            this.Shader.Parameters["matMTI"].SetValue(MTI);
            this.Shader.Parameters["DiffuseColor"].SetValue(new Vector3(1, 1, 1));
            this.Shader.Parameters["Texture"].SetValue(this.Tex1);
            this.Shader.Parameters["Tiling"].SetValue(new Vector2(1, 1));
            this.Shader.Parameters["AmbientColor"].SetValue(LightSystem.GetSingleton().AmbientColor);
            this.Shader.Parameters["CameraPosition"].SetValue(Engine.GetSingleton().MainCamera.Position);
            this.Shader.Parameters["SpecularColor"].SetValue(new Vector3(1, 1, 1));
            this.Shader.Parameters["SpecularPower"].SetValue(4.0f);

            int lightNum = LightSystem.GetSingleton().GetLightNum();
            this.Shader.Parameters["LightNum"].SetValue(lightNum);
            for (int i = 0; i < lightNum; i++)
            {
                Light light = LightSystem.GetSingleton().GetLight(i);
                this.mLightType[i] = (int)light.Type;
                this.mLightPosition[i] = light.gameObject.Position;
                this.mLightDir[i] = light.GetLightDirection();
                this.mLightColor[i] = light.LightColor;
                this.mLightAttenuation[i] = light.Attenuation;
                this.mLightCutOffDistance[i] = light.CutOffDistance;
                this.mConeAngle[i] = light.ConeAngle;
            }


            this.Shader.Parameters["LightType"].SetValue(this.mLightType);
            this.Shader.Parameters["LightPosition"].SetValue(this.mLightPosition);
            this.Shader.Parameters["LightDir"].SetValue(this.mLightDir);
            this.Shader.Parameters["LightColor"].SetValue(this.mLightColor);
            this.Shader.Parameters["LightAttenuation"].SetValue(this.mLightAttenuation);
            this.Shader.Parameters["LightCutOffDistance"].SetValue(this.mLightCutOffDistance);
            this.Shader.Parameters["ConeAngle"].SetValue(this.mConeAngle);
        }
    }
}
