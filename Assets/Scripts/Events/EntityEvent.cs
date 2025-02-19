using Ciart.Pagomoa.Entities.Players;

namespace Ciart.Pagomoa.Events
{
    /// <summary>
    /// Player가 생성될 때 호출됩니다.
    /// </summary>
    /// <param name="player"></param>
    public record PlayerSpawnedEvent(PlayerController player) : IEvent;
}
