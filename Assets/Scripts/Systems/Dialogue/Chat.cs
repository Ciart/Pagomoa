using System;
using System.Diagnostics.CodeAnalysis;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Utilities;
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
        
        [SuppressMessage("ReSharper", "IteratorMethodResultIsIgnored")]
        public void Chatting(string content, float duration)
        {
            chatBalloon.chatContent.text = content;
            chatBalloon.ReSizeBalloon();
        
            chatBalloon.balloon.SetActive(true);

            Action easeChat = EaseChatting;

            TimerUtility.SetTimer(duration, easeChat);
        }

        public void EaseChatting()
        {
            chatBalloon.balloon.SetActive(false);
        }
    }
}
