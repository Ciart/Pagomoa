using System;
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
        try
        {
            if (balloon.transform.GetChild(1).name == "Icon")
            {
                icon = balloon.transform.GetChild(1).GetComponent<Image>();
                balloon.SetActive(false);
            }
            else
            {
                balloon.SetActive(false);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e + " 아이콘이 존재하지 않음");
        }

        _chatBalloonTransform = balloon.transform.GetComponent<RectTransform>();
        _chatIconTransform = icon.transform.GetComponent<RectTransform>();
    }
    public void ReSizeBalloon()
    {
        _chatBalloonTransform.sizeDelta = new Vector2(100 + 45 * chatContent.text.Length, _chatBalloonTransform.sizeDelta.y);
        _chatIconTransform.anchoredPosition = new Vector3(_chatIconTransform.anchoredPosition.x + chatContent.text.Length, _chatIconTransform.anchoredPosition.y);
    }
}
