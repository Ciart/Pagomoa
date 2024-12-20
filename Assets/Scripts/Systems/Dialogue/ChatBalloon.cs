using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class ChatBalloon : MonoBehaviour
    {
        public GameObject chatBalloonPrefab;

        [HideInInspector]
        public GameObject balloon;
        [HideInInspector]
        public TextMeshProUGUI chatContent;
        [HideInInspector]
        public Image icon;

        public Vector3 floatingPosition = new Vector3(1,0.3f, 0);

        private RectTransform _chatBalloonTransform;
        private RectTransform _chatIconTransform;

        private void Awake()
        {
            balloon = Instantiate(chatBalloonPrefab, transform.position + floatingPosition, Quaternion.identity, transform).transform.GetChild(0).gameObject;
            balloon.SetActive(true);
            chatContent = GetComponentInChildren<TextMeshProUGUI>();
            
            /*if (balloon.transform.GetChild(1).name == "Icon")
            {
                icon = balloon.transform.GetChild(1).GetComponent<Image>();
                balloon.SetActive(false);
            }
            else { balloon.SetActive(false); }*/
            
            _chatBalloonTransform = balloon.transform.GetComponent<RectTransform>();
            if(icon)
                _chatIconTransform = icon.transform.GetComponent<RectTransform>();
        }
        public void ReSizeBalloon()
        {
            _chatBalloonTransform.sizeDelta = new Vector2(100 + 45 * chatContent.text.Length, _chatBalloonTransform.sizeDelta.y);
            if(icon)
                _chatIconTransform.anchoredPosition = new Vector3((_chatBalloonTransform.sizeDelta.x * -1) / 32, _chatIconTransform.anchoredPosition.y);
        }
    }
}
