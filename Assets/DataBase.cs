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
        public WorldRenderer GetWorldRenderer();
        public ItemEntity GetItemEntity();
        public List<GameObject> GetParticles();
        public UIContainer GetUIData();
        public CutSceneController GetCutSceneController();
    }
    public class DataBase : MonoBehaviour, IDataBase
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

            cutSceneController = Instantiate(cutSceneController);
        }
        
        public WorldDatabase GetWorldData() { return worldDatabase; }
        public WorldRenderer GetWorldRenderer() { return worldRenderer; }
        public ItemEntity GetItemEntity() { return itemEntity; }
        public List<GameObject> GetParticles() { return particles; }
        public UIContainer GetUIData() { return uiContainer; }
        public CutSceneController GetCutSceneController() { return cutSceneController; }
    }
}
