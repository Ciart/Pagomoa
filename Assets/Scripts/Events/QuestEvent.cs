using Ciart.Pagomoa.Logger.ForEditorBaseScripts;
using Ciart.Pagomoa.Systems;
using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    public record QuestAccomplishEvent() : IEvent;
    
    public record SignalToNpc(string questId, bool accomplishment, InteractableObject questInCharge) : IEvent;
    
    public record PlayerMove(Vector3 playerPos) : IEvent;
    
    public record QuestRegister(InteractableObject questInCharge, string id) : IEvent;

    public record QuestValidation(Quest quest) : IEvent;

    public record ValidationResult(bool result) : IEvent;

    public record SetCompleteChat(string id) : IEvent;

    public record CompleteQuest(InteractableObject questInCharge, string id) : IEvent;
}