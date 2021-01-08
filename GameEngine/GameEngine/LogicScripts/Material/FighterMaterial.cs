using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class FighterMaterial : IMaterial
    {
        public Texture2D TexDiffuse;
        public Texture2D TexNormal;

        public override void OnDraw(Matrix aM, Matrix aV, Matrix aP)
        {
            Matrix MVP = aM * aV * aP;
            Matrix MTI = Matrix.Transpose(Matrix.Invert(aM));
            this.Shader.Parameters["matM"].SetValue(aM);
            this.Shader.Parameters["matMVP"].SetValue(MVP);
            this.Shader.Parameters["matMTI"].SetValue(MTI);

            Light light = LightSystem.GetSingleton().GetLight(0);

            this.Shader.Parameters["DiffuseColor"].SetValue(new Vector3(1, 1, 1));
            this.Shader.Parameters["TexDiffuse"].SetValue(this.TexDiffuse);
            this.Shader.Parameters["TexNormal"].SetValue(this.TexNormal);
            this.Shader.Parameters["UseNormalTexture"].SetValue(false);
            this.Shader.Parameters["Tiling"].SetValue(new Vector2(1, 1));
            this.Shader.Parameters["LightDir"].SetValue(light.GetLightDirection());
            this.Shader.Parameters["LightColor"].SetValue(light.LightColor);
            this.Shader.Parameters["LightPosition"].SetValue(light.gameObject.Position);
            this.Shader.Parameters["AmbientColor"].SetValue(LightSystem.GetSingleton().AmbientColor);
            this.Shader.Parameters["CameraPosition"].SetValue(Engine.GetSingleton().MainCamera.Position);
            this.Shader.Parameters["SpecularColor"].SetValue(new Vector3(2, 2, 2));
            this.Shader.Parameters["SpecularPower"].SetValue(1.0f);
        }
    }
}
