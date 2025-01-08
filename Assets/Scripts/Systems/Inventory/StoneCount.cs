using Ciart.Pagomoa.Entities.Players;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class StoneCount : MonoBehaviour
    {
        [SerializeField] private Sprite[] drillImages;
        [SerializeField] private Image drillImage;
        [SerializeField] private TextMeshProUGUI countText;

        public static StoneCount Instance = null;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
        }
        private void Start()
        {
            WriteStoneCount();
            LoadDrillImage();
        }
        private void WriteStoneCount()
        {
            var playerInventory = Game.instance.player.inventory;
            countText.text = $"{playerInventory.stoneCount} / {playerInventory.maxCount}";
        }
        public void UpCount(int count)
        {
            var playerInventory = Game.instance.player.inventory;
            
            playerInventory.stoneCount += count;
            countText.text = $"{playerInventory.stoneCount} / {playerInventory.maxCount}";
            LoadDrillImage();
        }
        private void LoadDrillImage()
        {
            var playerInventory = Game.instance.player.inventory;
            
            if (playerInventory.maxCount / 2 <= playerInventory.stoneCount)
                drillImage.sprite = drillImages[0];
        }
    }
}
