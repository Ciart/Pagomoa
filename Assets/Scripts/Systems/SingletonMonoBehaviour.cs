using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T instance { get; private set; }

        protected virtual void Awake()
        {
            if (!instance)
            {
                instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}