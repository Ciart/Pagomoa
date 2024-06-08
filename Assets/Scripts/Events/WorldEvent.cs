using Ciart.Pagomoa.Worlds;

namespace Ciart.Pagomoa.Events
{
    public record WorldCreatedEvent(World world) : IEvent;
    
    public record LevelChangedEvent(Level level) : IEvent;
    
    /// <summary>
    /// Chunk 내부 Brick이 변경될 경우 호출됩니다.
    /// 같은 Chunk는 LateUpdate당 한 번만 호출됩니다.
    /// </summary>
    /// <param name="level">변경된 레벨</param>
    /// <param name="chunk">변경된 청크</param>
    public record ChunkChangedEvent(Level level, Chunk chunk) : IEvent;

    public record GroundBrokenEvent(int x, int y, Brick brick) : IEvent;
}