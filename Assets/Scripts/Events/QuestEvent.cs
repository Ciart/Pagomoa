using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;

    public record SignalToNpc(int questId, bool accomplishment, GameObject questInCharge) : IEvent;

    public record PlayerMove(Vector3 playerPos) : IEvent;

    public record QuestRegister(GameObject questInCharge, int id) : IEvent;
    public record IsQuestComplete(int id) : IEvent;
}