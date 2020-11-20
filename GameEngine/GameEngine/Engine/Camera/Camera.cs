using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class Camera
    {
        public Matrix World = Matrix.Identity;
        public Matrix View = Matrix.Identity;
        public Matrix Projection = Matrix.Identity;

        public Vector3 Position;
        public float ViewAngle = 45.0f;
        public float Ratio;
        public float Near = 0.1f;
        public float Far = 100.0f;

        public void Update()
        {
            this.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            this.View = Matrix.CreateLookAt(this.Position, new Vector3(0, 0, 0), Vector3.Up);
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(this.ViewAngle),
                this.Ratio,
                this.Near,
                this.Far
                );
        }
    }
}
