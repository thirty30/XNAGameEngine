using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class ModelRenderObject : RenderObject
    {
        private Model mModel = null;
        private IMaterial mMaterial = null;

        public Model Model { get { return this.mModel; } }
        public IMaterial Material { get { return this.mMaterial; } }
        
        public void Init(Model aModel, IMaterial aMaterial)
        {
            this.mModel = aModel;
            this.mMaterial = aMaterial;
            foreach (var mesh in this.mModel.Meshes)
            {
                foreach (var effect in mesh.MeshParts)
                {
                    effect.Effect = this.mMaterial.Shader;
                }
            }
        }

        public override void Draw()
        {
            if (this.gameObject.IsActive == false)
            {
                return;
            }

            Matrix matM = Matrix.Identity;
            matM *= Matrix.CreateScale(this.gameObject.LocalScale);
            float x = MathHelper.ToRadians(this.gameObject.LocalRotation.X);
            float y = MathHelper.ToRadians(this.gameObject.LocalRotation.Y);
            float z = MathHelper.ToRadians(this.gameObject.LocalRotation.Z);
            Quaternion quatRotation = Quaternion.CreateFromYawPitchRoll(y, x, z);
            matM *= Matrix.CreateFromQuaternion(quatRotation);
            matM *= Matrix.CreateTranslation(this.gameObject.Position);
            Matrix matV = Engine.GetSingleton().MainCamera.View;
            Matrix matP = Engine.GetSingleton().MainCamera.Projection;

            foreach (var mesh in this.mModel.Meshes)
            {
                this.mMaterial.OnDraw(matM, matV, matP);
                mesh.Draw();
            }
        }
    }
}
