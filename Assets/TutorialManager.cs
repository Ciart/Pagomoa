using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Canvas _tutorialChatCanvas;

    void Start()
    {
        try
        {
            _tutorialChatCanvas = transform.GetChild(0).GetComponent<Canvas>();
        } catch(Exception e) {
            Debug.Log("튜토리얼 캔버스가 없습니다.");
        }
    }
    
    void Update()
    {
    }
}
