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
            _countText.text = $"{GameManager.player.inventoryDB.stoneCount} / {GameManager.player.inventoryDB.maxCount}";
        }
        public void UpCount(int count)
        {
            GameManager.player.inventoryDB.stoneCount += count;
            _countText.text = $"{GameManager.player.inventoryDB.stoneCount} / {GameManager.player.inventoryDB.maxCount}";
            LoadDrillImage();
        }
        private void LoadDrillImage()
        {
            if (GameManager.player.inventoryDB.maxCount / 2 <= GameManager.player.inventoryDB.stoneCount)
                _drillImage.sprite = _drillImages[0];
        }
    }
}
