using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class LightSystem
    {
        private LightSystem() { }
        private static LightSystem m_instance = new LightSystem();
        public static LightSystem GetSingleton() { return m_instance; }

        public Vector3 AmbientColor = new Vector3(0.15f, 0.15f, 0.15f);

        private List<Light> mLights = new List<Light>();
        
        public void Initialize()
        {

        }

        public void AddLight(Light aObj)
        {
            this.mLights.Add(aObj);
        }

        public void RemoveLight(Light aObj)
        {
            this.mLights.Remove(aObj);
        }

        public int GetLightNum() { return this.mLights.Count; }
        public Light GetLight(int aIDX) 
        {
            if (aIDX < 0 || aIDX >= this.GetLightNum())
            {
                return null;
            }
            return this.mLights[aIDX]; 
        }

    }
}
