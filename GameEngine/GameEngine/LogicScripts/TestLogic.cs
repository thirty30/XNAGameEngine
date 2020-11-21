using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameEngine
{
    public class TestLogic : Component
    {
        private Texture mTex;
        private ModelRenderObject mMRO;

        public override void Initialize()
        {
            this.mTex = Engine.GetSingleton().Content.Load<Texture>("Checkerboard");
            this.mMRO = this.gameObject.GetComponent<ModelRenderObject>();
        }

        public override void Update()
        {
            this.mMRO.Shader.Parameters["Texture"].SetValue(this.mTex);
            this.mMRO.Shader.Parameters["LightDir"].SetValue(new Vector3(-1, -1, -1));
            this.mMRO.Shader.Parameters["LightColor"].SetValue(new Vector3(0.8f, 0.8f, 0.8f));
            this.mMRO.Shader.Parameters["AmbientColor"].SetValue(new Vector3(0.15f, 0.15f, 0.15f));
            this.mMRO.Shader.Parameters["CameraPosition"].SetValue(new Vector3(0, 0, 1.5f));
            this.mMRO.Shader.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 1));
        }
    }
}
