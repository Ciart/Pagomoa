using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [CreateAssetMenu(fileName = "New ItemGoMoleInherentEffect", menuName = "New ItemInherentEffect/GoMoleEffect")]
    public class GoMoleEffect : InherentEffect
    {
        public int moleCount = 3;
        public override void Effect(InherentItem item, PlayerStatus stat)
        {
            int count = 0;
            while (count < moleCount)
            {
                GoMole();
                count++;
            }
        }
        void GoMole()
        {
            UnityEngine.Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            var point = playerTransform.position + new Vector3(0, -1.2f);
            var pointInt = WorldManager.ComputeCoords(point);
            bool find = false;

            while (!find)
            {
                if (WorldManager.world.currentLevel.GetBrick(pointInt.x, pointInt.y, out _).mineral != null)
                    find = true;

                WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        point += Vector3.right;
                        break;
                    case 1:
                        point += Vector3.left;
                        break;
                    case 2:
                        point += Vector3.down;
                        break;
                    case 3:
                        point += Vector3.up;
                        break;
                }
                pointInt = WorldManager.ComputeCoords(point);
            }
        }
    }
}
