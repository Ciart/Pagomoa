using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindowItemEntity : MonoBehaviour
{
    public ShopItemData data;

    private SpriteRenderer _spriteRenderer;

    public ShopItemData Data
    {
        get => data;
        set
        {
            data = value;
            _spriteRenderer.sprite = data.sprite;
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = data.sprite;
    }
}
