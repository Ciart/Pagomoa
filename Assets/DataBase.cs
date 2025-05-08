using System;
using System.Collections;
using System.Collections.Generic;
using Ciart.Pagomoa.Sounds;
using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using Cinemachine;
using Logger;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa
{
    public interface IDataBase
    {
        public WorldDatabase GetWorldData();
        public WorldRenderer GetWorldRenderer();
        public ItemEntity GetItemEntity();
        public List<GameObject> GetParticles();
        public UIContainer GetUIData();
        public CutSceneController GetCutSceneController();
        public AudioMixerController GetAudioController();
    }
    public class DataBase : SingletonMonoBehaviour<DataBase>, IDataBase
    {
        [Header("World Data")]
        [SerializeField] private WorldDatabase worldDatabase;

        [Header("World Renderer Data")]
        [SerializeField] private WorldRenderer worldRenderer;

        [Header("ItemEntity Data")]
        [SerializeField] private ItemEntity itemEntity;
        
        [Header("Particle Data")]
        [SerializeField] private List<GameObject> particles;
        
        [Header("UI Data")]
        [SerializeField] private UIContainer uiContainer;
        
        [Header("AudioController")] 
        [SerializeField] private AudioMixerController _audioMixerController;
        
        [Header("CutSceneControllerPrefab")]
        [SerializeField] private CutSceneController _cutSceneController;
        
        public static IDataBase data { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
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

        private void Start()
        {
            _cutSceneController = Instantiate(_cutSceneController);
            DontDestroyOnLoad(_cutSceneController);
            _audioMixerController = Instantiate(_audioMixerController);
            DontDestroyOnLoad(_audioMixerController);
        }

        public WorldDatabase GetWorldData() { return worldDatabase; }
        public WorldRenderer GetWorldRenderer() { return worldRenderer; }
        public ItemEntity GetItemEntity() { return itemEntity; }
        public List<GameObject> GetParticles() { return particles; }
        public UIContainer GetUIData() { return uiContainer; }
        public AudioMixerController GetAudioController() { return _audioMixerController; }
        public CutSceneController GetCutSceneController() { return _cutSceneController; }
    }
}
