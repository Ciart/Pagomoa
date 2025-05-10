using Ciart.Pagomoa.Entities;
using Ciart.Pagomoa.Entities.Players;

namespace Ciart.Pagomoa.Events
{
    /// <summary>
    /// Player가 생성될 때 호출됩니다.
    /// </summary>
    /// <param name="player"></param>
    public record PlayerSpawnedEvent(PlayerController player) : IEvent;
    
    /// <summary>
    /// Entity가 파괴되기 이전에 호출됩니다.
    /// </summary>
    /// <param name="entity"></param>
    public record EntityDied(EntityController entity) : IEvent;
}
