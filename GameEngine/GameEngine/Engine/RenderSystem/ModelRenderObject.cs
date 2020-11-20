using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class ModelRenderObject : RenderObject
    {
        private readonly Model mModel = null;
        private readonly Effect mShader = null;

        public ModelRenderObject(Model aModel, Effect aShader)
        {
            this.mModel = aModel;
            this.mShader = aShader;

            foreach (var mesh in this.mModel.Meshes)
            {
                foreach (var effect in mesh.MeshParts)
                {
                    effect.Effect = this.mShader;
                }
            }
        }

        public Model Model { get { return this.mModel; } }
        public Effect Shader { get { return this.mShader; } }

        public override void Draw()
        {
            Matrix matM = Matrix.Identity;
            matM *= Matrix.CreateScale(this.gameObject.LocalScale);
            matM *= Matrix.CreateFromQuaternion(this.gameObject.LocalRotation);
            matM *= Matrix.CreateTranslation(this.gameObject.Position);
            Matrix matV = Engine.GetSingleton().MainCamera.View;
            Matrix matP = Engine.GetSingleton().MainCamera.Projection;
            foreach (var mesh in this.mModel.Meshes)
            {
                this.mShader.Parameters["WorldViewProjection"].SetValue(matM * matV * matP);
                mesh.Draw();
            }
        }
    }
}
