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
        // Start is called before the first frame update
        public void Chatting(string content)
        {
            chatBalloon.chatContent.text = content;
            chatBalloon.ReSizeBalloon();
        
            chatBalloon.balloon.SetActive(true);
        }
    }
}
