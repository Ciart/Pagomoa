using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    [RequireComponent(typeof(Ciart.Pagomoa.Entities.Players.PlayerController))]
    public class PlayerDigger : MonoBehaviour
    {
        public Direction direction;
        public bool isDig;
        public float tiredDigSpeedScoop = 5f;

        public int drillLevel = 0;
        public int drillTier = 10;

        public int width = 2;
        public int length = 1;

        public TargetBrickChecker targetPrefabs;

        private int[] drillSpeed = { 10, 20, 40, 80, 200, 5000, 1000000 };
        private int[] drillTierSetting = { 1, 2, 3, 4, 5 };
        
        private TargetBrickChecker _target;

        private PlayerStatus _playerStatus;

        private void Awake()
        {
            _playerStatus = GetComponent<PlayerStatus>();
            _target = Instantiate(targetPrefabs);
        }

        public void DrillUprade()
        {
            drillLevel++;
            if (drillLevel < drillSpeed.Length)
            {
                GetComponent<Ciart.Pagomoa.Entities.Players.PlayerController>()._initialStatus.digSpeed = drillSpeed[drillLevel];
            }

            if (drillLevel < drillTierSetting.Length)
                drillTier = drillTierSetting[drillLevel];
        }

        private void Update()
        {
            if (!isDig)
            {
                return;
            }

            _target.ChangeTarget(DirectionCheck(width % 2 == 0), width, length,
                direction is Direction.Left or Direction.Right);

            foreach (var (x, y) in _target.targetCoordsList)
            {
                var worldManager = WorldManager.instance;
                worldManager.DigGround(new BrickCoords(x, y), _playerStatus.digSpeed);
            }
        }

        Vector2Int DirectionCheck(bool a = false)
        {
            Vector3 digVec;
            switch (direction)
            {
                case Direction.Up:
                    digVec = new Vector3(a ? -0.5f : 0f, 1.2f);
                    break;
                case Direction.Left:
                    digVec = new Vector3(-1.2f, a ? -0.5f : 0f);
                    break;
                case Direction.Right:
                    digVec = new Vector3(1.2f, a ? -0.5f : 0f);
                    break;
                case Direction.Down:
                default:
                    digVec = new Vector3(a ? -0.5f : 0f, -1.2f);
                    break;
            }

            return WorldManager.ComputeCoords(transform.position + digVec);
        }
    }
}