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
            _countText.text = $"{InventoryDB.Instance.stoneCount} / {InventoryDB.Instance.maxCount}";
        }
        public void UpCount(int count)
        {
            InventoryDB.Instance.stoneCount += count;
            _countText.text = $"{InventoryDB.Instance.stoneCount} / {InventoryDB.Instance.maxCount}";
            LoadDrillImage();
        }
        private void LoadDrillImage()
        {
            if (InventoryDB.Instance.maxCount / 2 <= InventoryDB.Instance.stoneCount)
                _drillImage.sprite = _drillImages[0];
        }
    }
}
