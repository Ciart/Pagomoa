using Ciart.Pagomoa.Systems;

namespace Ciart.Pagomoa.Events
{
        public record FadeEvent(FadeState state) : IEvent;
}
