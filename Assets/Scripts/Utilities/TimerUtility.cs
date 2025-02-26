using System;
using System.Threading.Tasks;

namespace Ciart.Pagomoa.Utilities
{
    public static class TimerUtility
    {
        /// <summary>
        /// 짧은 시간동안 기다렸다 실행되는 예약 타이머 입니다. 유니티에서가 아닌 메모리 자체에 등록됩니다.
        /// </summary>
        ///<param name="seconds">1000 milliseconds 단위 초 단위로 대기 시간을 잽니다.</param>
        ///<param name="callback">seconds의 대기 시간이 지난 후 실행할 함수 Action을 Invoke()합니다.</param>
        /// <remarks>분 단위 이상의 긴 시간 동안 타이머를 적용하거나 Update(), Awake()등의 유니티 업데이트의 등록은 메모리 성능 저하를 야기합니다.</remarks>
        public static async void SetTimer(float seconds, Action callback)
        {
            var milliSeconds = (int)(seconds * 1000);
            await Task.Delay(milliSeconds);

            callback.Invoke();
        }
    }
}