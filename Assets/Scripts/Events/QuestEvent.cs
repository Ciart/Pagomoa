using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Logger.ProcessScripts;
using UnityEngine;

namespace Ciart.Pagomoa.Events
{
    /*public record QuestAccomplishEvent() : IEvent;*/
    //public record SignalToNpc(string questID, bool accomplishment) : IEvent;
    
    // TODO: QuestCompleted로 대체해야 합니다.
    /*[Obsolete("QuestCompleted로 대체 예정입니다.")]*/
    
    /// <summary>
    /// 퀘스트 리스트가 업데이트 되었을 때 발생하는 이벤트입니다.
    /// 런타임 퀘스트 객체 내부 값이 변경되었을 때는 발생하지 않습니다. 대신 QuestUpdated 이벤트를 사용하세요.
    /// </summary>
    /// <param name="questList">런타임 퀘스트 객체 리스트</param>
    public record QuestListUpdated(List<Quest> questList) : IEvent;
    
    /// <summary>
    /// 퀘스트가 시작되었을 때 발생하는 이벤트입니다.
    /// 퀘스트 진행전 처리해야할 사항이 있으면 이 이벤트를 사용합니다.
    /// </summary>
    /// <param name="quest">런타임 퀘스트 객체</param>
    public record QuestStarted(Quest quest) : IEvent;
    
    /// <summary>
    /// 퀘스트가 업데이트 되었을 때 발생하는 이벤입니다.
    /// 진행도 등이 변경되었을 때 UI를 업데이트하기 위해 사용합니다.
    /// </summary>
    /// <param name="quest">런타임 퀘스트 객체</param>
    public record QuestUpdated(Quest quest) : IEvent;

    /// <summary>
    /// 퀘스트가 완료되었을 때 발생하는 이벤트입니다.
    /// TODO: 미구현입니다.
    /// </summary>
    /// <param name="quest">런타임 퀘스트 객체</param>
    public record QuestCompleted(Quest quest) : IEvent;

    // TODO: 다른 곳으로 옮겨야 합니다.
    public record PlayerMove(Vector3 playerPos) : IEvent;
}