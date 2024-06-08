
using System.Linq;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Entities.Players
{
    [RequireComponent(typeof(Ciart.Pagomoa.Entities.Players.PlayerController))]
    public class PlayerDigger : MonoBehaviour
    {
        public DrillController drill;

        public Direction direction;
        public bool isDig;
        public float tiredDigSpeedScoop = 5f;

        public int drillLevel = 0;
        public int drillTier = 10;

        public int width = 2;
        public int length = 1;

        public TargetBrickChecker targetPrefabs;

        private int[] drillSpeed = { 10, 15, 20, 30, 40, 50, 66 };
        private int[] drillTierSetting = { 1, 2, 3, 4, 5};

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
                GetComponent<Ciart.Pagomoa.Entities.Players.PlayerController>()._initialStatus.digSpeed =
                    drillSpeed[drillLevel];
            }

            if (drillLevel < drillTierSetting.Length)
                drillTier = drillTierSetting[drillLevel];
        }

        private void Update()
        {
            if (!isDig)
            {
                drill.gameObject.SetActive(false);
                return;
            }

            drill.gameObject.SetActive(true);

            _target.ChangeTarget(DirectionCheck(width % 2 == 0), width, length,
                direction is Direction.Left or Direction.Right);

            // TODO: 최적화 필요합니다.
            drill.isGroundHit = _target.targetCoordsList.Any((coord) =>
            {
                var (x, y) = coord;
                var brick = WorldManager.world.currentLevel.GetBrick(x, y, out _);

                return brick.ground is not null;
            });

            foreach (var (x, y) in _target.targetCoordsList)
            {
                var worldManager = WorldManager.instance;
                worldManager.DigGround(new BrickCoords(x, y), _playerStatus.digSpeed);
            }
        }

        private WorldCoords DirectionCheck(bool a = false)
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