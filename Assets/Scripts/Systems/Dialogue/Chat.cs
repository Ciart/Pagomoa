using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChatBalloon))]
public class Chat : MonoBehaviour
{
    protected private ChatBalloon chatBalloon;

    private void Awake()
    {
        chatBalloon = GetComponent<ChatBalloon>();
    }

    public void Chatting(string content)
    {
        chatBalloon.chatContent.text = content;
        chatBalloon.ReSizeBalloon();
        
        chatBalloon.balloon.SetActive(true);
    }
}
