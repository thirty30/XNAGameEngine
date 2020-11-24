using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class PlaneMaterial : IMaterial
    {
        public Texture2D Tex1;

        public override void OnDraw(Matrix aM, Matrix aV, Matrix aP)
        {
            Matrix MVP = aM * aV * aP;
            Matrix MTI = Matrix.Transpose(Matrix.Invert(aM));
            this.Shader.Parameters["matM"].SetValue(aM);
            this.Shader.Parameters["matMVP"].SetValue(MVP);
            this.Shader.Parameters["matMTI"].SetValue(MTI);

            Light light = LightSystem.GetSingleton().GetLight(2);

            this.Shader.Parameters["DiffuseColor"].SetValue(new Vector3(1, 1, 1));
            this.Shader.Parameters["Texture"].SetValue(this.Tex1);
            this.Shader.Parameters["Tiling"].SetValue(new Vector2(10, 10));
            this.Shader.Parameters["LightDir"].SetValue(light.GetLightDirection());
            this.Shader.Parameters["LightColor"].SetValue(light.LightColor);
            this.Shader.Parameters["LightPosition"].SetValue(light.gameObject.Position);
            if (light.Type == LightType.POINT)
            {
                this.Shader.Parameters["LightAttenuation"].SetValue(light.Attenuation);
                this.Shader.Parameters["LightCutOffDistance"].SetValue(light.CutOffDistance);
            }
            else if (light.Type == LightType.SPOT)
            {
                this.Shader.Parameters["LightAttenuation"].SetValue(light.Attenuation);
                this.Shader.Parameters["LightCutOffDistance"].SetValue(light.CutOffDistance);
                this.Shader.Parameters["InnerAngle"].SetValue(light.InnerAngle);
                this.Shader.Parameters["OuterAngle"].SetValue(light.OuterAngle);
            }
            this.Shader.Parameters["AmbientColor"].SetValue(LightSystem.GetSingleton().AmbientColor);
            this.Shader.Parameters["CameraPosition"].SetValue(Engine.GetSingleton().MainCamera.Position);
            this.Shader.Parameters["SpecularColor"].SetValue(new Vector3(1, 1, 1));
            this.Shader.Parameters["SpecularPower"].SetValue(4.0f);
        }
    }
}
