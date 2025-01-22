using System;
using UnityEngine;

namespace Ciart.Pagomoa.Systems
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        [Obsolete("Instance를 사용하세요. (대문자로 시작하는 프로퍼티)")]
        public static T instance { get; private set; }
        
        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (!instance)
            {
                instance = (T)this;

                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}