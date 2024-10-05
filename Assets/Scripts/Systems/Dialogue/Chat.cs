using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    [RequireComponent(typeof(ChatBalloon))]
    public class Chat : MonoBehaviour
    {
        protected ChatBalloon chatBalloon;

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

        public void EaseChatting()
        {
            chatBalloon.balloon.SetActive(false);
        }
    }
}
