using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Events
{
    public class EventSystem : SingletonMonoBehaviour<EventSystem>
    {
        private Dictionary<Type, HashSet<Delegate>> _listeners = new();

        /// <summary>
        /// 이벤트를 발생시킵니다.ZF
        /// </summary>
        /// <param name="args">전달할 데이터</param>
        /// <typeparam name="T">이벤트 타입</typeparam>
        public static void Notify<T>(T args) where T : IEvent
        {
            var eventManager = instance;
            var type = typeof(T);

            if (!eventManager._listeners.ContainsKey(type))
            {
                return;
            }

            foreach (var listener in eventManager._listeners[type])
            {
                ((Action<T>)listener)(args);
            }
        }

        /// <summary>
        /// 이벤트를 받을 리스너를 등록합니다.
        /// </summary>
        /// <param name="listener">이벤트 발생 시 호출될 델리게이트</param>
        /// <typeparam name="T">이벤트 타입</typeparam>
        public static void AddListener<T>(Action<T> listener) where T : IEvent
        {
            var eventManager = instance;
            var type = typeof(T);

            if (!eventManager._listeners.ContainsKey(type))
            {
                eventManager._listeners[type] = new HashSet<Delegate>();
            }

            eventManager._listeners[type].Add(listener);
        }

        /// <summary>
        /// 등록된 리스너를 제거합니다.
        /// </summary>
        /// <param name="listener">제거할 델리게이트</param>
        /// <typeparam name="T">이벤트 타입</typeparam>
        public static void RemoveListener<T>(Action<T> listener) where T : IEvent
        {
            var eventManager = instance;
            var type = typeof(T);

            if (!eventManager._listeners.ContainsKey(type))
            {
                return;
            }

            eventManager._listeners[type].Remove(listener);
        }
    }
}