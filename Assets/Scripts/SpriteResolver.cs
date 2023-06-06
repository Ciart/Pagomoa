using System;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteResolver : MonoBehaviour
{
    public Texture2D texture;

    public int pixelPerUnit = 16;
    
    public Vector2 pivot = new Vector2(0.5f, 0.5f);
    
    [Min(1)]
    public int columnCount = 4;
    
    [Min(1)]
    public int rowCount = 4;

    [Min(0)]
    public int column;
    
    [Min(0)]
    public int row;

    private SpriteRenderer _spriteRenderer;
    
    private Sprite[,] _sprites;
    
    private Texture2D _prevTexture;

    private int _prevColumn;
    
    private int _prevRow;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateSpriteSet();
        UpdateSprite();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        UpdateSpriteSet();
        UpdateSprite();
    }
#endif

    private void LateUpdate()
    {
        if (_prevTexture != texture)
        {
            _prevTexture = texture;
            UpdateSpriteSet();
            UpdateSprite();

            return;
        }

        if (_prevColumn != column || _prevRow != row)
        {
            _prevColumn = column;
            _prevRow = row;

            UpdateSprite();
        }
    }

    private void UpdateSpriteSet()
    {
        _sprites = new Sprite[columnCount, rowCount];

        var spriteWidth = texture.width / columnCount;
        var spriteHeight = texture.height / rowCount;

        for (var i = 0; i < columnCount; i++)
        {
            for (var j = 0; j < rowCount; j++)
            {
                var x = i * spriteWidth;
                var y = texture.height - (j + 1) * spriteHeight;

                _sprites[i, j] = Sprite.Create(texture, new Rect(x, y, spriteWidth, spriteHeight), pivot, pixelPerUnit);
            }
        }
    }

    private void UpdateSprite()
    {
        _spriteRenderer.sprite = _sprites[Math.Clamp(column, 0, columnCount - 1), Math.Clamp(row, 0, rowCount - 1)];
    }
}
