using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class InteractableObject : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _clickRenderer;

    private readonly string _outline = "_OutlineColor";
    
    // 유니티 이벤트 호출
    public UnityEvent InteractionEvent; 
    
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _clickRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _clickRenderer.enabled = false;
    }
    public void ActiveObject()
    {
        Color a = new Color(0.38f,0.75f, 0.92f, 1f);
        
        _spriteRenderer.material.SetColor(_outline, a);
        _clickRenderer.enabled = true;
    }

    public void DisableObject()
    {
        _spriteRenderer.material.SetColor(_outline, Color.white);
        _clickRenderer.enabled = false;
    }
}
