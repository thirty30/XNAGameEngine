using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameEngine
{
    public class RenderSystem
    {
        private RenderSystem() { }
        private static RenderSystem m_instance = new RenderSystem();
        public static RenderSystem GetSingleton() { return m_instance; }

        private Dictionary<long, RenderObject> mDicRenderObj = new Dictionary<long, RenderObject>();
        private GUIDGeneratorWithNumber GUIDGen = new GUIDGeneratorWithNumber();

        public void Initialize()
        {

        }

        public long GenRGUID()
        {
            return this.GUIDGen.GetGUID();
        }

        public void AddRenderable(RenderObject aObj)
        {
            this.mDicRenderObj.Add(aObj.RGUID, aObj);
        }

        public void RemoveRenderable(RenderObject aObj)
        {
            this.mDicRenderObj.Remove(aObj.RGUID);
        }

        public void Update()
        {
            this.Render();
        }

        private void Render()
        {
            foreach(var obj in this.mDicRenderObj.Values)
            {
                obj.Draw();
            }
        }
    }
}

