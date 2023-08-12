using System;
using UnityEngine;
using Worlds;

namespace Player
{
    public class TargetBrickRenderer : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        private WorldManager _worldManager;

        public void ChangeTarget(Vector2Int position, int width, int length, bool isHorizontal = false)
        {
            Vector3 offset;
            
            if (width % 2 == 0)
            {
                offset = !isHorizontal ? new Vector3(0.5f, 0f) : new Vector3(0f, 0.5f);
            }
            else
            {
                offset = Vector3.zero; 
            }
            
            _spriteRenderer.size = isHorizontal ? new Vector2(length, width) : new Vector2(width, length);

            transform.position = WorldManager.ComputePosition(position) + offset;
        }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _worldManager = WorldManager.instance;
        }
    }
}