using Ciart.Pagomoa.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ciart.Pagomoa.UI.Book
{
    public class InfoUI : UIBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] needStoneText;
        [SerializeField] private Image[] needStoneImage;

        [SerializeField] private TextMeshProUGUI drillName;
        [SerializeField] private TextMeshProUGUI drillDescription;

        [SerializeField] private Button upgradeBtn;

        private void SetMineralCount(string id, int count)
        {
            UIUpdate();
        }

        protected override void Start()
        {
            base.Start();
            
            upgradeBtn?.onClick.AddListener(Game.Instance.player.drill.DrillUpgrade);
            upgradeBtn?.onClick.AddListener(UIUpdate);
        }

        private void UIUpdate()
        {
            if (Game.Instance.player == null) return;

            var nowDrill = Game.Instance.player.drill.nowDrill;
            drillName.text = nowDrill.drillName;
            drillDescription.text = nowDrill.drillDescription;


            var nextDrill = Game.Instance.player.drill.nextDrill;

            for (int i = 0; i < nextDrill.upgradeNeeds.Length; i++)
            {
                var mineralID = nextDrill.upgradeNeeds[i].mineral.id;
                var mineralName = nextDrill.upgradeNeeds[i].mineral.name;
                var currentCount = MineralCollector.GetMineralCount(mineralID);
                var maxCount = nextDrill.upgradeNeeds[i].count;

                needStoneImage[i].sprite = Resources.Load<Sprite>("Items/" + mineralID);
                needStoneImage[i].enabled = true;
                needStoneText[i].text = $"{mineralName} {currentCount}/{maxCount}";
                needStoneText[i].enabled = true;
            }
            for (int i = nextDrill.upgradeNeeds.Length; i < needStoneText.Length; i++)
            {
                needStoneText[i].enabled = false;
                needStoneImage[i].enabled = false;
            }
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            UIUpdate();
            MineralCollector.OnMineralsChange += SetMineralCount;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            MineralCollector.OnMineralsChange -= SetMineralCount;
        }
    }
}
