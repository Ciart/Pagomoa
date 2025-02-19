using System;
using System.Collections;
using Ink.Parsed;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ciart.Pagomoa.Entities.CactusBoss.RenewCactusBoss
{
    public class RCactusBossHammer : MonoBehaviour
    {
        [SerializeField] private bool isRight;
        [SerializeField] private GameObject shockWave;
        public Transform[] shockWaveSpawner;

        [SerializeField] private GameObject particle;
        public Transform[] particleSpawnPoints;
        
        [NonSerialized] public bool doneInstantiate = true;
        [NonSerialized] public bool isGather;
        
        private RCactusBoss _boss;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        void Start()
        {
            _boss = GetComponentInParent<RCactusBoss>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponentInParent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var info = _animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log(info.IsName("HammerResetRotation"));
            
            if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                if (info.IsName("HammerSmashDown"))
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    InstantiateParticles();
                }
                else if(info.IsName("HammerDown"))
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    StartCoroutine(InstantiateShockWave());
                }
                else if (info.IsName("HammerCrash"))
                {
                    return;
                }
            }

            if (collision.gameObject.CompareTag("Boss"))
            {
                if (info.IsName("HammerDown"))
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    if (isRight)
                        StartCoroutine(InstantiateShockWave());
                }
                else if (info.IsName("HammerCrash"))
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    Debug.Log(gameObject.name + ": " + _rigidbody.linearVelocity);
                    StartCoroutine(InstantiateShockWaveAtCenter());
                }
                else if (info.IsName("HammerGather"))
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    isGather = true;
                }
            }
        }

        private void InstantiateParticles()
        {
            for (int i = 0; i < particleSpawnPoints.Length; i++)
            {
                GameObject dust = Instantiate(particle, particleSpawnPoints[i].position, Quaternion.identity);
                
                Rigidbody2D dustRigidbody = dust.GetComponent<Rigidbody2D>();

                float randomX = Random.Range(-1.0f, 1.0f);
                
                Vector2 random = new Vector2(randomX, 1).normalized;
                dustRigidbody.AddForce(random * 35f, ForceMode2D.Impulse);
            }
            doneInstantiate = true;
        }
        private IEnumerator InstantiateShockWaveAtCenter()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject center = Instantiate(shockWave, shockWaveSpawner[2].position, quaternion.identity);

                if (isRight)
                    center.GetComponent<RenewCactusBossShockWave>().AddForce(150, 1);
                else
                    center.GetComponent<RenewCactusBossShockWave>().AddForce(150, -1);
                
                yield return new WaitForSeconds(1.5f);
            }
            doneInstantiate = true;
        }
        private IEnumerator InstantiateShockWave()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject left = Instantiate(shockWave, shockWaveSpawner[0].position, Quaternion.identity);
                GameObject right = Instantiate(shockWave, shockWaveSpawner[1].position, Quaternion.identity);

                if (isRight)
                {
                    left.GetComponent<RenewCactusBossShockWave>().AddForce(150, -1);
                    right.GetComponent<RenewCactusBossShockWave>().AddForce(150, 1);
                }
                else
                {
                    left.GetComponent<RenewCactusBossShockWave>().AddForce(100, -1);
                    right.GetComponent<RenewCactusBossShockWave>().AddForce(100, 1);
                }
                yield return new WaitForSeconds(1.5f);
            }
            doneInstantiate = true;
        }
        public void HammerCoroutine(float downSpeed)
        {
            _boss.hammers[0].GetComponent<Rigidbody2D>().AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);

            StartCoroutine(AfterHammerDown(downSpeed));
        }
        
        private IEnumerator AfterHammerDown(float downSpeed)
        {
            yield return new WaitForSeconds(2);
            _boss.hammers[1].GetComponent<Rigidbody2D>().AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
        }
    }
}
