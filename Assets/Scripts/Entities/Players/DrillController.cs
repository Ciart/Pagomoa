using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Time;
using Ciart.Pagomoa.Worlds;
using UnityEngine;
using Direction = Ciart.Pagomoa.Constants.Direction;

namespace Ciart.Pagomoa.Entities.Players
{
    public class DrillController : MonoBehaviour
    {
        public SpriteResolver head;

        public SpriteResolver body;

        public Vector2 upOffset;

        public Vector2 downOffset;

        public Vector2 leftOffset;

        public Vector2 rightOffset;

        public AudioClip spinSound;

        public AudioClip groundHitSound;
        
        public AudioClip groundHitLoopSound;

        public bool isGroundHit;

        private AudioSource _spinAudioSource;
        
        private AudioSource _groundHitAudioSource;

        private Direction _prevDirection;

        private List<EntityController> _enemies = new List<EntityController>();

        [SerializeField]
        private List<Drill> _drills;

        private int _drillLevel = 0;

        private int width = 2;

        private int length = 1;

        private PlayerStatus _playerStatus;

        private TargetBrickChecker _target;

        private Transform[] _drillParts;

        public TargetBrickChecker targetPrefabs;

        public Direction direction;

        public bool isDig = false;

        private void Awake()
        {
            // TODO: 자식 오브젝트의 컴포넌트로 변경해야 합니다.
            // _spinAudioSource = gameObject.AddComponent<AudioSource>();
            // _spinAudioSource.clip = spinSound;
            // _spinAudioSource.volume = 0.25f;
            
            // TODO: 자식 오브젝트의 컴포넌트로 변경해야 합니다.
            _groundHitAudioSource = gameObject.AddComponent<AudioSource>();
            _groundHitAudioSource.volume = 0.25f;

            _target = Instantiate(targetPrefabs);

            _playerStatus = transform.parent.GetComponent<PlayerStatus>();

            _drillParts = gameObject.GetComponentsInChildren<Transform>();
        }

        private void UpdateDirection()
        {
            if (direction == _prevDirection)
            {
                return;
            }

            _prevDirection = direction;

            transform.localPosition = direction switch
            {
                Direction.Up => upOffset,
                Direction.Down => downOffset,
                Direction.Left => leftOffset,
                Direction.Right => rightOffset,
                _ => downOffset
            };

            head.row = (int)direction;
            body.row = (int)direction;
        }

        private void Update()
        {
            UpdateDirection();
            //if (!isDig)
            //{
            //    foreach (var drillParts in gameObject.GetComponentsInChildren<Transform>())
            //    {
            //        if (drillParts.gameObject != gameObject)
            //            drillParts.gameObject.SetActive(false);
            //    }
            //    return;
            //}
            foreach (var drillPart in _drillParts)
            {
                if(drillPart.gameObject != gameObject)
                    drillPart.gameObject.SetActive(isDig);
            }

            if (!isDig) return;


            _target.ChangeTarget(DirectionCheck(width % 2 == 0), width, length, direction is Direction.Left or Direction.Right);

            // TODO: 최적화 필요합니다.
            isGroundHit = _target.targetCoordsList.Any((coord) =>
            {
                var (x, y) = coord;
                var brick = WorldManager.instance.world.GetBrick(x, y, out _);

                return brick.ground is not null;
            });

            foreach (var (x, y) in _target.targetCoordsList)
            {
                var worldManager = WorldManager.instance;
                worldManager.DigGround(new BrickCoords(x, y), _playerStatus.digSpeed);
            }
        }

        private void OnEnable()
        {
            // _spinAudioSource.Play();
            SoundManager.instance.PlaySfx("DrillSpin", true);
            TimeManager.instance.tickUpdated += OnTickUpdated;
        }

        private void OnDisable()
        {
            TimeManager.instance.tickUpdated -= OnTickUpdated;
        }
        
        private void OnTickUpdated(int tick)
        {
            if (isGroundHit)
            {
                if (!SoundManager.instance.FindAudioSource("DrillHitEffect").isPlaying)
                {
                    if (SoundManager.instance.FindAudioSource("DrillHitEffect").clip ==
                        SoundManager.instance.FindSfxBundle("DrillHitGround").audioClip[0])
                    {
                        SoundManager.instance.FindAudioSource("DrillHitEffect").clip =
                            SoundManager.instance.FindSfxBundle("DrillHitGroundLoop").audioClip[0];
                        SoundManager.instance.FindAudioSource("DrillHitEffect").loop = true;
                    }
                    else
                    {
                        SoundManager.instance.FindAudioSource("DrillHitEffect").clip =
                            SoundManager.instance.FindSfxBundle("DrillHitGround").audioClip[0];
                        SoundManager.instance.FindAudioSource("DrillHitEffect").loop = false;
                    }
                    SoundManager.instance.FindAudioSource("DrillHitEffect").Play();
                }
            }
            else
            {
                if (SoundManager.instance.FindAudioSource("DrillHitEffect").isPlaying)
                {
                    SoundManager.instance.FindAudioSource("DrillHitEffect").Stop();
                    SoundManager.instance.FindAudioSource("DrillHitEffect").clip =
                        SoundManager.instance.FindSfxBundle("DrillHitGroundLoop").audioClip[0];
                }
            }
            
                
                //         if (!_groundHitAudioSource.isPlaying)
                //         {
                //             if (_groundHitAudioSource.clip == groundHitSound)
                //             {
                //                 _groundHitAudioSource.clip = groundHitLoopSound;
                //                 _groundHitAudioSource.loop = true;
                //             }
                //             else
                //             {
                //                 _groundHitAudioSource.clip = groundHitSound;
                //                 _groundHitAudioSource.loop = false;
                //             }
                //             
                //             _groundHitAudioSource.Play();
                //         }
                //     }
                //     else
                //     {
                //         if (_groundHitAudioSource.isPlaying)
                //         {
                //             _groundHitAudioSource.Stop();
                //             _groundHitAudioSource.clip = groundHitLoopSound;
                //         }
                // }
                foreach (var enemy in _enemies)
                {
                    // TODO: attacker 변경해야 함.
                    // TODO: 무적 시간을 빼고 다른 시각적 효과를 줘야 함.
                    enemy.TakeDamage(5, invincibleTime: 0.3f, attacker: null, flag: DamageFlag.Melee);
                }
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var entity = collision.GetComponent<EntityController>();

            if (entity is null || _enemies.Contains(entity) || !entity.isEnemy)
            {
                return;
            }

            _enemies.Add(entity);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var entity = collision.GetComponent<EntityController>();

            if (entity is null || !_enemies.Contains(entity))
            {
                return;
            }

            _enemies.Remove(entity);
        }

        private Vector2Int DirectionCheck(bool a = false)
        {
            Vector3 digVec;
            switch (direction)
            {
                case Direction.Up:
                    //digVec = new Vector3(a ? -0.5f : 0f, 1.2f);
                    break;
                case Direction.Left:
                    //digVec = new Vector3(-0.6f, a ? -0.5f : 0f);
                    break;
                case Direction.Right:
                    //digVec = new Vector3(0.6f, a ? -0.5f : 0f);
                    break;
                case Direction.Down:
                default:
                    //digVec = new Vector3(a ? -0.5f : 0f, -1.2f);
                    break;
            }
            digVec = transform.localPosition;
            return WorldManager.ComputeCoords(transform.position + digVec);
        }

        public void DrillUpgrade()
        {
            // _drills[_drillLevel + 1].upgradeNeeds 충족확인 후
            _drillLevel += 1;
        }
    }
}