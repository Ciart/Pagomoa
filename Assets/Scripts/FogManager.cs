using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    SpriteRenderer fogRenderer;
    Texture2D fogTexture;

    float worldWidth, worldHeight;
    int pixelWidth, pixelHeight;
    void Start()
    {
        fogRenderer = GetComponent<SpriteRenderer>();
        fogTexture = new Texture2D(3000, 3000);

        for (int i = 0; i < fogTexture.width; i++)
        {
            for (int j = 0; j < fogTexture.height; j++)
            {
                fogTexture.SetPixel(i, j, Color.white);
            }
        }
        fogTexture.Apply();
        MakeSprite();

        worldWidth = fogRenderer.bounds.size.x;
        worldHeight = fogRenderer.bounds.size.y;
        pixelWidth = fogRenderer.sprite.texture.width;
        pixelHeight = fogRenderer.sprite.texture.height;

        Debug.Log("World: " + worldWidth + ", " + worldHeight + "Pixel: " + pixelWidth + ", " + pixelHeight );
    }
    public void fadeOutFogs(CircleCollider2D sight)
    {
        Vector2Int colliderCenter = WorldToPixel(sight.bounds.center);
        int radius = Mathf.RoundToInt(sight.bounds.size.x / 2 * pixelWidth/worldWidth);
        int distance, minXPoint, minYPoint, plusXPoint, plusYPoint;

       /* for (int i = 0; i < radius; i++)
        {
            distance = Mathf.RoundToInt(Mathf.Sqrt(radius * radius - i * i));
            for(int j = 0; j < distance; j++)
            {
                minXPoint = colliderCenter.x - i;
                plusXPoint = colliderCenter.x + i;
                minYPoint = colliderCenter.y - j;
                plusYPoint = colliderCenter.y + j;

                fogTexture.SetPixel(plusXPoint, plusYPoint, Color.clear);
                fogTexture.SetPixel(plusXPoint, minYPoint, Color.clear);
                fogTexture.SetPixel(minXPoint, minYPoint, Color.clear);
                fogTexture.SetPixel(minXPoint, plusYPoint, Color.clear);
            }
        }*/
        fogTexture.Apply();
        MakeSprite();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<CircleCollider2D>()) return;
        fadeOutFogs(collision.GetComponent<CircleCollider2D>());
    }
    private void MakeSprite()
    {
        fogRenderer.sprite = Sprite.Create(fogTexture, new Rect( 0, 0, fogTexture.width, fogTexture.height), Vector2.one * 0.5f);
    }

    public void fadeOutFog( Vector3 pos )
    {
        Vector2Int pixelPosition = WorldToPixel(pos);
        fogTexture.SetPixel(pixelPosition.x, pixelPosition.y, Color.clear);
        fogTexture.Apply();
    }

    public Vector2Int WorldToPixel(Vector3 pos)
    {
        Vector2Int pixelPosition = Vector2Int.zero;
        
        float dx = pos.x - transform.position.x;
        float dy = pos.y - transform.position.y;

        pixelPosition.x = Mathf.RoundToInt(0.5f * pixelWidth + dx * (pixelWidth / worldWidth));
        pixelPosition.y = Mathf.RoundToInt(0.5f * pixelHeight + dy * (pixelHeight / worldHeight));

        return pixelPosition;
    }
}
