using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoChat : Chat
{
    public List<string> chat = new List<string>();

    int chatIndex = 0;

    public float initialStartTime = 4.62f;
    public float floatingTime = 3f;
    public float updateTime = 4f;

    private void Start()
    {
        StopChat();
        StartChatReservation(initialStartTime);
    }

    IEnumerator Chat()
    {
        if (chat.Count == 0)
            yield break;

        Chatting(chat[chatIndex]);
        
        chatIndex++;
        if (chatIndex >= chat.Count)
            chatIndex = 0;

        yield return new WaitForSeconds(floatingTime);
        chatBalloon.balloon.SetActive(false);
        StartChatReservation();
    }
    public void StartChatReservation()
    {
        Invoke("StartChat", updateTime);
    }
    public void StartChatReservation(float time)
    {
        Invoke("StartChat", time);
    }
    void StartChat()
    {
        StartCoroutine("Chat");
    }

    public void StopChat()
    {
        CancelInvoke("StartChat");
        StopCoroutine("Chat");
        chatBalloon.balloon.SetActive(false);
    }
}
