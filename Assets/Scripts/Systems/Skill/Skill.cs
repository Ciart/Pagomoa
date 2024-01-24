using System.Collections;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Entities.Players;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using PlayerController = Ciart.Pagomoa.Entities.Players.PlayerController;

namespace Ciart.Pagomoa.Systems.Skill
{
    public class Skill : MonoBehaviour
    {
        PlayerController _player;
        bool Helicopter = false;
        private void Awake()
        {
            _player = GetComponent<PlayerController>();
        }
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    Helicopter = !Helicopter;
            //    if (Helicopter)
            //        StartCoroutine(HelicopterStart());
            //}
            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    MoleHill();
            //}
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    StartCoroutine(GoMole());
            //}
        }
    
        IEnumerator HelicopterStart()
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            while (WorldManager.instance.CheckClimbable(transform.position) && Helicopter)
            {
                _player.state = PlayerState.Jump;
                if(rb.velocity.y >= 0)
                    rb.AddForce(new Vector2(0, 30), ForceMode2D.Force);
                else
                    rb.AddForce(new Vector2(0, 60), ForceMode2D.Force);
                yield return new WaitForFixedUpdate();
            }
            Helicopter = false;
        }
        void MoleHill()
        {
            Vector3 digVec;
            switch (_player.GetDirection()) 
            {
                case Direction.Left:
                    digVec = Vector3.left;
                    break;
                case Direction.Right:
                    digVec = Vector3.right;
                    break;
                default:
                    digVec = Vector3.down;
                    break;
            }
        
            var point = transform.position + digVec * 1.2f;
            var pointInt = WorldManager.ComputeCoords(point);
            while (WorldManager.instance.world.GetBrick(pointInt.x, pointInt.y, out _).ground)
            {
                if (WorldManager.instance.world.GetBrick(pointInt.x, pointInt.y, out _).mineral)
                    return;
            
                WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999);
                point = point + digVec;
                pointInt = WorldManager.ComputeCoords(point);
            }
        }
        IEnumerator GoMole()
        {
            var point = transform.position + new Vector3(0, -1.2f);
            var pointInt = WorldManager.ComputeCoords(point);
            bool find = false;

            while (WorldManager.instance.world.GetBrick(pointInt.x, pointInt.y, out _).ground && !find)
            {
                if (WorldManager.instance.world.GetBrick(pointInt.x, pointInt.y, out _).mineral)
                    find = true;
            
                WorldManager.instance.BreakGround(pointInt.x, pointInt.y, 99999);
                switch(Random.Range(0, 3))
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
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}
