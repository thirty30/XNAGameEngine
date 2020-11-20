using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public class GameObject
    {
        private List<IComponent> mComponents = new List<IComponent>();
        private long mGGUID;
        public long GGUID { get { return this.mGGUID; } }

        public Vector3 Position = Vector3.Zero;
        public Quaternion LocalRotation;
        public Vector3 LocalScale = Vector3.One;

        public GameObject()
        {
            this.mGGUID = GameObjectManager.GetSingleton().GenGGUID();
            GameObjectManager.GetSingleton().AddGameObject(this);
        }

        ~GameObject()
        {
            GameObjectManager.GetSingleton().RemoveGameObject(this);
            foreach (Component com in this.mComponents)
            {
                com.OnDestroy();
            }
        }

        public T AddComponent<T>() where T : new()
        {
            T obj = new T();
            Component com = obj as Component;
            com.gameObject = this;
            this.mComponents.Add(com);
            return obj;
        }

        public void InitComponents()
        {
            foreach (Component com in this.mComponents)
            {
                if (com.IsInitialized == false)
                {
                    com.Initialize();
                    com.IsInitialized = true;
                }
            }
        }

        public void UpdateComponents()
        {
            foreach (Component com in this.mComponents)
            {
                com.Update();
            }
        }
    }
}
