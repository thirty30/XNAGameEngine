using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public abstract class RenderObject : Component
    {
        private long mRGUID;
        public long RGUID { get { return this.mRGUID; } }

        public RenderObject() 
        {
            this.mRGUID = RenderSystem.GetSingleton().GenRGUID();
            RenderSystem.GetSingleton().AddRenderable(this); 
        }

        ~RenderObject() 
        { 
            RenderSystem.GetSingleton().RemoveRenderable(this); 
        }

        public abstract void Draw();
    }
}
