﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Events;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Systems.Inventory;
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

        public bool isGroundHit;

        private AudioSource _spinAudioSource;
        
        private AudioSource _groundHitAudioSource;

        private Direction _prevDirection;

        private List<EntityController> _enemies = new List<EntityController>();

        [SerializeField]
        private List<Drill> _drills;
        
        private int _drillLevel = 0;

        public Drill nowDrill
        {
            get
            {
                return _drills[_drillLevel];
            }
        }

        public Drill nextDrill
        {
            get {
                if (_drillLevel + 1 < _drills.Count)
                    return _drills[_drillLevel + 1];
                else
                    return null;
            }
        } 
        
        private int width = 2;

        private int length = 1;

        private TargetBrickChecker _target;

        private Transform[] _drillParts;

        public TargetBrickChecker targetPrefabs;

        public Direction direction;

        public bool isDig = false;
        
        public bool isPlayed = false;

        private MoaInteraction _fairyMoa;
        
        private PlayerController _player;
       
        public static event Action<Drill> OnDrillUpgrade;
        
        private void Awake()
        {
            _groundHitAudioSource = gameObject.AddComponent<AudioSource>();
            _groundHitAudioSource.volume = 0.25f;

            _target = Instantiate(targetPrefabs);

            _drillParts = gameObject.GetComponentsInChildren<Transform>();
            
            _fairyMoa = FindAnyObjectByType<MoaInteraction>();
            _fairyMoa.InitMoa();
            
            _player = GetComponentInParent<PlayerController>();
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
            RemoveThisIfItWorkedProperly();
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

            var soundManager = Game.Instance.Sound;
            
            if (!isDig)
            {
                if (!_fairyMoa.gameObject.activeSelf)
                {
                    _fairyMoa.gameObject.SetActive(true);
                    
                    _fairyMoa.MoaAppearEffect();
                }
                return;
            }
            if (isDig)
            {
                if (!soundManager.controller.GetDrillSpinSource().isPlaying && isPlayed == false)
                {
                    soundManager.PlaySfx("DrillSpin", false);
                    isPlayed = true;
                }

                if (_fairyMoa.gameObject.activeSelf)
                {
                    _fairyMoa.MoaAppearEffect();
                    
                    _fairyMoa.gameObject.SetActive(false);
                }
            }
            else
            {
                soundManager.controller.GetDrillSpinSource().Stop();
                isPlayed = false;
                return;
            }
            
            _target.ChangeTarget(DirectionCheck(width % 2 == 0), width, length, direction is Direction.Left or Direction.Right);

            // TODO: 최적화 필요합니다.
            isGroundHit = _target.targetCoordsList.Any((coord) =>
            {
                var (x, y) = coord;
                var brick = WorldManager.world.currentLevel.GetBrick(x, y, out _);

                return brick?.ground is not null;
            });

            foreach (var (x, y) in _target.targetCoordsList)
            {
                var worldManager = Game.Instance.World;
                worldManager.DigGround(new WorldCoords(x, y), _drills[_drillLevel].speed);
            }
        }

        private void OnEnable()
        {
            Game.Instance.Time.tickUpdated += OnTickUpdated;
            EventManager.AddListener<EntityDied>(OnEntityDied);
        }

        private void OnDisable()
        {
            Game.Instance.Time.tickUpdated -= OnTickUpdated;
            EventManager.RemoveListener<EntityDied>(OnEntityDied);
        }

        private void OnEntityDied(EntityDied e)
        {
            _enemies.Remove(e.entity);
        }
        
        private void OnTickUpdated(int tick)
        {
            var soundManager = Game.Instance.Sound;
            
            if (isDig && isGroundHit)
            {
                if (!soundManager.controller.GetDrillHitSource().isPlaying)
                {
                    if (soundManager.controller.GetDrillHitSource().clip ==
                        soundManager.FindSfxBundle("DrillHitGround").audioClip[0])
                    {
                        soundManager.controller.GetDrillHitSource().clip =
                            soundManager.FindSfxBundle("DrillHitGroundLoop").audioClip[0];
                        soundManager.controller.GetDrillHitSource().loop = true;
                    }
                    else
                    {
                        soundManager.controller.GetDrillHitSource().clip =
                            soundManager.FindSfxBundle("DrillHitGround").audioClip[0];
                        soundManager.controller.GetDrillHitSource().loop = false;
                    }
                    soundManager.controller.GetDrillHitSource().Play();
                }
            }
            else
            {
                if (soundManager.controller.GetDrillHitSource().isPlaying)
                {
                    soundManager.controller.GetDrillHitSource().Stop();
                    soundManager.controller.GetDrillHitSource().clip =
                        soundManager.FindSfxBundle("DrillHitGroundLoop").audioClip[0];
                }
            }
            
            
            if (isDig)
            {
                var preFrameEnemy = _enemies.ToArray();
                foreach (var enemy in preFrameEnemy)
                {
                    // TODO: attacker 변경해야 함.
                    // TODO: 무적 시간을 빼고 다른 시각적 효과를 줘야 함.
                    enemy.TakeDamage(_player.Attack, invincibleTime: 0.3f, attacker: GetComponentInParent<EntityController>(), flag: DamageFlag.Melee);
                }
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

        private WorldCoords DirectionCheck(bool a = false)
        {
            Vector3 digVec;
            switch (direction)
            {
                case Direction.Up:
                    digVec = new Vector3(a ? -0.5f : 0f, 1.2f);
                    break;
                case Direction.Left:
                    digVec = new Vector3(-0.6f, a ? -0.5f : 0f);
                    break;
                case Direction.Right:
                    digVec = new Vector3(0.6f, a ? -0.5f : 0f);
                    break;
                case Direction.Down:
                default:
                    digVec = new Vector3(a ? -0.5f : 0f, -1.2f);
                    break;
            }
            return WorldManager.ComputeCoords(transform.position + digVec - transform.localPosition);
        }

        public void DrillUpgrade()
        {
            var inventory = Game.Instance.player.inventory;

            if (nextDrill == null) { Debug.Log("Drill is on Max Level. Can not Upgrade!"); return; }

            foreach (var needs in nextDrill.upgradeNeeds)
            {
                if (needs.mineral.id == "")
                    return;

                if (!MineralCollector.TryUseMineral(needs.mineral.id, needs.count))
                    return;
            }

            _drillLevel += 1;
            OnDrillUpgrade?.Invoke(_drills[_drillLevel]);
            Debug.Log("업그레이드 완료" + _drills[_drillLevel]);
        }

        private void RemoveThisIfItWorkedProperly()
        {
            if (Input.GetKeyDown(KeyCode.U)) DrillUpgrade();
        }
    }
}
