using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    public record MakeQuestListEvent() : IEvent;
    
    public record AddNpcImageEvent(Sprite sprite) : IEvent;
}
