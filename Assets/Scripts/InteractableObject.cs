using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class InteractableObject : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public Light2D Highlight;

    // 유니티 이벤트 호출
    public UnityEvent InteractionEvent; 
    
    void Start()
    {
        SpriteRenderer = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        Highlight = transform.GetChild(1).GetComponentInChildren<Light2D>();
        SpriteRenderer.enabled = false;
        Highlight.enabled = false;
    }
    public void ActiveObject()
    {
        SpriteRenderer.enabled = true;
        Highlight.enabled = true;
    }
    public void DisableObject()
    {
        SpriteRenderer.enabled = false;
        Highlight.enabled = false;
    }
}
