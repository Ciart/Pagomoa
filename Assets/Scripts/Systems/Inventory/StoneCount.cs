using Ciart.Pagomoa.Entities.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class StoneCount : MonoBehaviour
    {
        [SerializeField] private Sprite[] _drillImages;
        [SerializeField] private Image _drillImage;
        [SerializeField] private TextMeshProUGUI _countText;

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
            Inventory playerInventory = Game.Get<GameManager>().player.inventory;
            _countText.text = $"{playerInventory.stoneCount} / {playerInventory.maxCount}";
        }
        public void UpCount(int count)
        {
            Inventory playerInventory = Game.Get<GameManager>().player.inventory;
            
            playerInventory.stoneCount += count;
            _countText.text = $"{playerInventory.stoneCount} / {playerInventory.maxCount}";
            LoadDrillImage();
        }
        private void LoadDrillImage()
        {
            Inventory playerInventory = Game.Get<GameManager>().player.inventory;
            
            if (playerInventory.maxCount / 2 <= playerInventory.stoneCount)
                _drillImage.sprite = _drillImages[0];
        }
    }
}
