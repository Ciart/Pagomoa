using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialChat : Chat
{
    public List<string> startingChat = new List<string>();
    [Space]
    public List<string> annoyingChat = new List<string>();
    [Space]
    public List<string> lastRepeatChat = new List<string>();
    [Space]

    public float initialStartTime = 4.62f;
    public float floatingTime = 3f;
    public float updateTime = 4f;
    
    private int _chatIndex = 0;
    private int _loopCount = 0;

    private void Start()
    {
        StopChat();
        StartChatReservation(initialStartTime);
    }
    
    IEnumerator Chat()
    {
        if (startingChat.Count == 0 || annoyingChat.Count == 0)
            yield break;
        
        if (_loopCount < 3)
            _loopCount = StartingChat();
        else if (_loopCount < 5)
            _loopCount = AnnoyingChat();
        else
            LastRepeatingChat();

        yield return new WaitForSeconds(floatingTime);
        
        chatBalloon.balloon.SetActive(false);
        StartChatReservation();
    }
    
    private void StartChat()
    {
        StartCoroutine(nameof(Chat));
    }

    private void StopChat()
    {
        CancelInvoke(nameof(StartChat));
        StopCoroutine(nameof(Chat));
        chatBalloon.balloon.SetActive(false);
    }
    
    public void StartChatReservation()
    {
        Invoke(nameof(StartChat), updateTime);
    }
    public void StartChatReservation(float time)
    {
        Invoke(nameof(StartChat), time);
    }

    private int StartingChat()
    {
        Chatting(startingChat[_chatIndex]);
        
        _chatIndex++;
        if (_chatIndex == 3)
            _loopCount += 1;
        
        if (_chatIndex >= startingChat.Count)
            _chatIndex = 0;
        
        return _loopCount;
    }
    
    private int AnnoyingChat()
    {
        Chatting(annoyingChat[_chatIndex]);
        
        _chatIndex++;
        if (_chatIndex == 3)
            _loopCount += 1;
        
        if (_chatIndex >= annoyingChat.Count)
            _chatIndex = 0;
        
        return _loopCount;  
    }

    private void LastRepeatingChat()
    {
        Chatting(lastRepeatChat[_chatIndex]);
        
        _chatIndex++;
        if (_chatIndex >= lastRepeatChat.Count)
            _chatIndex = 0;
    }
}
