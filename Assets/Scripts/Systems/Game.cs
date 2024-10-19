using System;

namespace Ciart.Pagomoa.Systems
{
    public class Game : SingletonMonoBehaviour<Game>
    {
        public static ResourceManager resourceManager { get; private set; }
        
        public event Action awake;

        protected override void Awake()
        {
            base.Awake();

            resourceManager = new ResourceManager(this);
            
            awake?.Invoke();
        }
    }
}