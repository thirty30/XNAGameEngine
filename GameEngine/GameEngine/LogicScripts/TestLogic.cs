using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class TestLogic : Component
    {
        public override void Initialize()
        {
        }

        public override void Update()
        {
            float velocity = 0.5f;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.gameObject.Position += Vector3.Left * velocity;
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.gameObject.Position += Vector3.Right * velocity;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.gameObject.Position += Vector3.Forward * velocity;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.gameObject.Position += Vector3.Backward * velocity;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                this.gameObject.LocalRotation += new Vector3(0.5f, 0, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                this.gameObject.LocalRotation += new Vector3(-0.5f, 0, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.J))
            {
                this.gameObject.LocalRotation += new Vector3(0, 0.5f, 0);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                this.gameObject.LocalRotation += new Vector3(0, -0.5f, 0);
            }
        }
    }
}
