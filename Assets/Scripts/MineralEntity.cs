using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MineralEntity: MonoBehaviour
{
    public MineralData data;
    
    private SpriteRenderer _spriteRenderer;

    public MineralData Data
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