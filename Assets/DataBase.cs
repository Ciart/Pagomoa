using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Worlds;
using Cinemachine;
using Logger;
using UnityEngine;

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
        public CutSceneController GetCutSceneController();
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
        
        [Header("CutSceneController")]
        [SerializeField] private CutSceneController cutSceneController;
        
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
        
        public WorldDatabase GetWorldData() { return worldDatabase; }
        public ItemEntity GetItemEntity() { return itemEntity; }
        public QuestDatabase GetQuestData() { return questDatabase; }
        public List<ParticleSystem> GetParticles() { return particles; }
        public AudioSource GetAudioSource() { return musicSource; }
        public AudioSource[] GetSfxSources() { return sfxSources; }
        public UIContainer GetUIData() { return uiContainer; }
        public CutSceneController GetCutSceneController() { return cutSceneController; }
    }
}
