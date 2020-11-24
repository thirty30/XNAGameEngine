using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public abstract class IMaterial
    {
        public Effect Shader = null;
        public abstract void OnDraw(Matrix aM, Matrix aV, Matrix aP);
    }
}
