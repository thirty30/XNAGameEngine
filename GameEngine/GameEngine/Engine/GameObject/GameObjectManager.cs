using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class GameObjectManager
    {
        private GameObjectManager() { }
        private static GameObjectManager m_instance = new GameObjectManager();
        public static GameObjectManager GetSingleton() { return m_instance; }

        private Dictionary<long, GameObject> mDicObjs = new Dictionary<long, GameObject>();
        private GUIDGeneratorWithNumber GUIDGen = new GUIDGeneratorWithNumber();

        public long GenGGUID()
        {
            return this.GUIDGen.GetGUID();
        }

        public void AddGameObject(GameObject aObj)
        {
            this.mDicObjs.Add(aObj.GGUID, aObj);
        }

        public void RemoveGameObject(GameObject aObj)
        {
            this.mDicObjs.Remove(aObj.GGUID);
        }

        public void Initialize()
        {

        }

        public void Update()
        {
            foreach (var obj in this.mDicObjs.Values)
            {
                obj.InitComponents();
            }

            foreach (var obj in this.mDicObjs.Values)
            {
                obj.UpdateComponents();
            }
        }
    }
}
