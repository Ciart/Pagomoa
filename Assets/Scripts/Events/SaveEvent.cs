using Ciart.Pagomoa.Systems.Save;
using Ciart.Pagomoa.Worlds;

namespace Ciart.Pagomoa.Events
{
    public record DataSaveEvent(SaveData saveData) : IEvent;
    
    public record DataLoadedEvent(SaveData saveData) : IEvent;
}