using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using Ciart.Pagomoa.Systems.Save;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ArtifactSlotDB : MonoBehaviour
    {
        public List<InventoryItem> Artifact = new List<InventoryItem>();

        public static ArtifactSlotDB Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }
        private void Start()
        {
            SaveManager.Instance.LoadArtifactItem();
        }
        public void Remove(Item data)
        {
            var inventoryItem = Artifact.Find(inventoryItem => inventoryItem.item == data);
            if (inventoryItem != null)
                Artifact.Remove(inventoryItem);
        }
    }
}
