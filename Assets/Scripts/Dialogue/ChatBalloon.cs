using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBalloon : MonoBehaviour
{
    public GameObject chatBalloonPrefab;

    [HideInInspector]
    public GameObject balloon;
    [HideInInspector]
    public TextMeshProUGUI chatContent;

    public Vector3 floatingPosition = new Vector3(1,0.3f, 0);

    private RectTransform _chatBalloonTransform;

    private void Awake()
    {
        balloon = Instantiate(chatBalloonPrefab, transform.position + floatingPosition, Quaternion.identity, transform).transform.GetChild(0).gameObject;
        balloon.SetActive(true);
        chatContent = GetComponentInChildren<TextMeshProUGUI>();
        balloon.SetActive(false);
        
        _chatBalloonTransform = balloon.transform.GetComponent<RectTransform>(); 
    }
    public void ReSizeBalloon()
    {
        _chatBalloonTransform.sizeDelta = new Vector2(100 + 45 * chatContent.text.Length, _chatBalloonTransform.sizeDelta.y);
    }
}
