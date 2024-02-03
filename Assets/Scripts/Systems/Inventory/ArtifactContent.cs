using System.Collections.Generic;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class ArtifactContent : MonoBehaviour
    {
        //[SerializeField] private ArtifactSlotDB artifactSlotDB;
        [SerializeField] private Sprite _emptyImage;
        [SerializeField] private List<Slot> _artifactSlotDatas = new List<Slot>();

        private static ArtifactContent instance;
        public static ArtifactContent Instance
        {
            get
            {
                if (!instance)
                {
                    instance = GameObject.FindObjectOfType(typeof(ArtifactContent)) as ArtifactContent;
                }
                return instance;
            }
        }
        private void Start()
        {
            ResetSlot();
        }
        public void ResetSlot() 
        {
            int i = 0;
            for (; i < ArtifactSlotDB.Instance.Artifact.Count && i < _artifactSlotDatas.Count; i++)
                _artifactSlotDatas[i].inventoryItem = ArtifactSlotDB.Instance.Artifact[i];
            for (; i < _artifactSlotDatas.Count; i++)
                _artifactSlotDatas[i].inventoryItem = null;
            UpdateSlot();
        }
        public void UpdateSlot()
        {
            DeleteSlot();
            for (int i = 0; i < ArtifactSlotDB.Instance.Artifact.Count; i++)
                _artifactSlotDatas[i].SetUI(ArtifactSlotDB.Instance.Artifact[i].item.itemImage);
        }
        public void DeleteSlot()
        {
            if (ArtifactSlotDB.Instance.Artifact.Count >= 0)
                for (int i = 0; i < _artifactSlotDatas.Count; i++)
                    _artifactSlotDatas[i].SetUI(_emptyImage);
        }
    }
}
