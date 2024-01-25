using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    public class TargetBrickChecker : MonoBehaviour
    {
        private (int, int)[] _targetCoordsList;

        public (int, int)[] targetCoordsList
        {
            get => _targetCoordsList;
        }

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
            
            // Update targetCoordsList

            _targetCoordsList = new (int, int)[width * length];

            if (isHorizontal)
            {
                var topLeft = transform.position - new Vector3((float)length / 2f - 0.5f, (float)width / 2f - 0.5f);
                
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        var coords = WorldManager.ComputeCoords(topLeft + new Vector3(i, j));
                        _targetCoordsList[i * width + j] = (coords.x, coords.y);
                    }
                }
            }
            else
            {
                var topLeft = transform.position - new Vector3((float)width / 2f - 0.5f, (float)length / 2f - 0.5f);
                
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        var coords = WorldManager.ComputeCoords(topLeft + new Vector3(i, j));
                        _targetCoordsList[i * length + j] = (coords.x, coords.y);
                    }
                }
            }
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _worldManager = WorldManager.instance;
        }
    }
}