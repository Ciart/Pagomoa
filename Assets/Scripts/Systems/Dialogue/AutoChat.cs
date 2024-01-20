using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoChat : Chat
{
    public List<string> chat = new List<string>();

    public float initialStartTime = 4.62f;
    public float floatingTime = 3f;
    public float updateTime = 4f;
    
    private int _chatIndex = 0;

    private void Start()
    {
        StopChat();
        StartChatReservation(initialStartTime);
    }

    private IEnumerator Chat()
    {
        if (chat.Count == 0)
            yield break;

        Chatting(chat[_chatIndex]);
        
        _chatIndex++;
        if (_chatIndex >= chat.Count)
            _chatIndex = 0;

        yield return new WaitForSeconds(floatingTime);
        
        chatBalloon.balloon.SetActive(false);
        StartChatReservation();
    }

    public void StartChatReservation()
    {
        Invoke(nameof(StartChat), updateTime);
    }

    public void StartChatReservation(float time)
    {
        Invoke(nameof(StartChat), time);
    }
    private void StartChat()
    {
        StartCoroutine("Chat");
    }

    public void StopChat()
    {
        CancelInvoke(nameof(StartChat));
        StopCoroutine(nameof(Chat));
        chatBalloon.balloon.SetActive(false);
    }
}
