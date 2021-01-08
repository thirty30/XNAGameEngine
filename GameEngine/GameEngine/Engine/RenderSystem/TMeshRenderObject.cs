using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class TMeshRenderObject : RenderObject
    {
        private TMesh mModel = null;
        private IMaterial mMaterial = null;

        public TMesh Model { get { return this.mModel; } }
        public IMaterial Material { get { return this.mMaterial; } }


        private DynamicVertexBuffer mVertexBuffer;
        private IndexBuffer mIndexBuffer;


        public void Init(TMesh aModel, IMaterial aMaterial)
        {
            this.mModel = aModel;
            this.mMaterial = aMaterial;
            this.BindVertex();
        }

        public void BindVertex()
        {
            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[this.mModel.VertexNum];
            for (int i = 0; i < this.mModel.VertexNum; i++)
            {
                TMeshVertex vert = this.mModel.Verts[i];
                Vector3 pos = new Vector3(vert.x, vert.y, vert.z);
                Vector3 normal = new Vector3(vert.nx, vert.ny, vert.nz);
                Vector2 uv = new Vector2(vert.u, vert.v);
                vertices[i] = new VertexPositionNormalTexture(pos, normal, uv);
            }

            this.mVertexBuffer = new DynamicVertexBuffer(Engine.GetSingleton().GraphicsDevice, 
                typeof(VertexPositionNormalTexture), 
                vertices.Length, 
                BufferUsage.WriteOnly);
            this.mVertexBuffer.SetData<VertexPositionNormalTexture>(vertices);
            this.mIndexBuffer = new IndexBuffer(Engine.GetSingleton().GraphicsDevice, 
                typeof(int), 
                this.mModel.TriangleIndices.Length,
                BufferUsage.WriteOnly);
            this.mIndexBuffer.SetData(this.mModel.TriangleIndices);
        }

        public void SetMaterial(IMaterial aMat)
        {
            this.mMaterial = aMat;
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
            

            Engine.GetSingleton().GraphicsDevice.SetVertexBuffer(this.mVertexBuffer);
            Engine.GetSingleton().GraphicsDevice.Indices = this.mIndexBuffer;
            
            foreach (EffectPass pass in this.mMaterial.Shader.CurrentTechnique.Passes)
            {
                this.mMaterial.OnDraw(matM, matV, matP);
                pass.Apply();
                Engine.GetSingleton().GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.mModel.TriangleNum);
            }
        }
    }
}
