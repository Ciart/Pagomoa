using System;
using System.Collections.Generic;
using Ciart.Pagomoa.Constants;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;
using UnityEngine;

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
        
        // public AudioClip spinSound;

        public AudioClip groundHitSound;

        public AudioClip groundHitLoopSound;
        
        public bool isGroundHit;
        
        // private AudioSource _spinAudioSource;
        
        private AudioSource _groundHitAudioSource;
        
        private PlayerDigger _digger;

        private Direction _prevDirection;

        private List<EntityController> _enemies = new List<EntityController>();

        private void Awake()
        {
            // TODO: 자식 오브젝트의 컴포넌트로 변경해야 합니다.
            // _spinAudioSource = gameObject.AddComponent<AudioSource>();
            // _spinAudioSource.clip = spinSound;
            // _spinAudioSource.volume = 0.25f;
            
            // TODO: 자식 오브젝트의 컴포넌트로 변경해야 합니다.
            _groundHitAudioSource = gameObject.AddComponent<AudioSource>();
            _groundHitAudioSource.volume = 0.25f;
            
            _digger = transform.parent.GetComponent<PlayerDigger>();
        }

        private void OnEnable()
        {
            // _spinAudioSource.Play();
            SoundManager.instance.PlaySfx("DrillSpin");
        }

        private void UpdateDirection()
        {
            var direction = _digger.direction;

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

            if (isGroundHit)
            {
                // if (!SoundManager.instance.FindAudioSource("TeamEffect").clip == AudioClipDB.instance.SfxBundleDB)
                //     SoundManager.instance.PlaySfx("DrillGroundHit");
                    
                // if (!_groundHitAudioSource.isPlaying)
                // {
                    // if (_groundHitAudioSource.clip == groundHitSound)
                    // {
                    //     _groundHitAudioSource.clip = groundHitLoopSound;
                    //     _groundHitAudioSource.loop = true;
                    // }
                    // else
                    // {
                    //     _groundHitAudioSource.clip = groundHitSound;
                    //     _groundHitAudioSource.loop = false;
                    // }
                    //
                    // _groundHitAudioSource.Play();
                // }
            }
            // else
            // {
            //     if (_groundHitAudioSource.isPlaying)
            //     {
            //         _groundHitAudioSource.Stop();
            //         _groundHitAudioSource.clip = groundHitLoopSound;
            //     }
            // }
            
            foreach (var enemy in _enemies)
            {
                enemy.TakeDamage(10, attacker: _digger.gameObject, flag: DamageFlag.Melee);
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
    }
}