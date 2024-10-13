using Ciart.Pagomoa.Entities.Players;

namespace Ciart.Pagomoa.Events
{
    /// <summary>
    /// Player가 생성될 때 호출됩니다.
    /// </summary>
    /// <param name="player"></param>
    public record PlayerSpawnedEvent(PlayerController player) : IEvent;

    /// <summary>
    /// TimeManager에서 실행됩니다.
    /// Player의 조작을 제한합니다.
    /// 유니티의 Time을 제한하고 멈춥니다.
    /// </summary>
    /// <param name="player"></param>
    public record TimeStopEvent() : IEvent;
}