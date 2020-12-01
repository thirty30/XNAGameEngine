using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class Skybox
    {
        private Model mModel;
        private TextureCube mTex;
        private Effect mShader;

        public float Size = 1;

        public void Init(Model aModel, TextureCube aTex, Effect aShader)
        {
            this.mModel = aModel;
            this.mTex = aTex;
            this.mShader = aShader;
            foreach (var mesh in this.mModel.Meshes)
            {
                foreach (var effect in mesh.MeshParts)
                {
                    effect.Effect = this.mShader;
                }
            }
        }

        public void Draw()
        {
            Matrix matM = Matrix.Identity;
            matM *= Matrix.CreateScale(Vector3.One * this.Size);
            matM *= Matrix.CreateTranslation(Engine.GetSingleton().MainCamera.Position);
            Matrix matV = Engine.GetSingleton().MainCamera.View;
            Matrix matP = Engine.GetSingleton().MainCamera.Projection;
            Matrix matMVP = matM * matV * matP;

            foreach (var mesh in this.mModel.Meshes)
            {
                this.mShader.Parameters["matM"].SetValue(matM);
                this.mShader.Parameters["matMVP"].SetValue(matMVP);
                this.mShader.Parameters["Texture"].SetValue(this.mTex);
                this.mShader.Parameters["CameraPosition"].SetValue(Engine.GetSingleton().MainCamera.Position);
                mesh.Draw();
            }
        }
    }
}
