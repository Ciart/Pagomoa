using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Worlds;
using Cinemachine;
using Logger;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Ciart.Pagomoa
{
    public interface IDataBase
    {
        public WorldDatabase GetWorldData();
        public ItemEntity GetItemEntity();
        public QuestDatabase GetQuestData();
        public List<ParticleSystem> GetParticles();
        public AudioSource GetAudioSource();
        public AudioSource[] GetSfxSources();
        public UIContainer GetUIData();
        public T InstantiateData<T>(T target, Vector3 position) where T : Object;
        public T InstantiateData<T>(T target, Transform targetTransform) where T : Object;
        public void DestroyData<T>(T target, float duration = 0) where T : Object; 
    }
    public class DataBase : MonoBehaviour, IDataBase
    {
        [Header("World Data")]
        [SerializeField] private WorldDatabase worldDatabase;
        
        [Header("Quest Data")]
        [SerializeField] private QuestDatabase questDatabase;

        [Header("ItemEntity Data")]
        [SerializeField] private ItemEntity itemEntity;
        
        [Header("Particle Data")]
        [SerializeField] private List<ParticleSystem> particles;
        
        [Header("Sound Data")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource[] sfxSources;
        
        [Header("UI Data")]
        [SerializeField] private UIContainer uiContainer;
        
        public static IDataBase data { get; private set; }

        protected void Awake()
        {
            if (data is null)
            {
                data = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public WorldDatabase GetWorldData()
        {
            return worldDatabase;
        }

        public ItemEntity GetItemEntity()
        {
            return itemEntity;
        }

        public QuestDatabase GetQuestData()
        {
            return questDatabase;
        }

        public List<ParticleSystem> GetParticles()
        {
            return particles;
        }

        public AudioSource GetAudioSource()
        {
            return musicSource;
        }
        
        public AudioSource[] GetSfxSources()
        {
            return sfxSources;
        }

        public UIContainer GetUIData()
        {
            return uiContainer;
        }

        public T InstantiateData<T>(T target, Vector3 position = default) where T : Object
        {
            return Instantiate(target, position, Quaternion.identity);
        }
        
        public T InstantiateData<T>(T target, Transform targetTransform = null) where T : Object
        {
            return Instantiate(target, targetTransform);
        }

        public void DestroyData<T>(T target, float duration = 0) where T : Object
        {
            if (duration == 0)
            {
                Destroy(target);
            }
            else
            {
                Destroy(target, duration);    
            }
        }
    }
}
