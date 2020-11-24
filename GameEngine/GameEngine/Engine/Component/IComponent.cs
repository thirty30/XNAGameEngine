using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public interface IComponent
    {
        public void Initialize();
        public void Update();
    }

    public abstract class Component : IComponent
    {
        public GameObject gameObject = null;
        public bool IsInitialized = false;

        public virtual void Initialize() { }
        public virtual void Update() { }
        public virtual void OnDestroy() { }
        public virtual void OnDraw() { }
    }
}
