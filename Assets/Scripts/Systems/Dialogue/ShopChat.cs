using TMPro;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class ShopChat : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI chatting;
        [SerializeField] private string[] _awakeChats;
        [SerializeField] private string[] _endChats;
        [SerializeField] private string[] _cancleChats;

        public static ShopChat Instance = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void AwakeChat()
        {
            int random = Random.Range(0, _awakeChats.Length);
            chatting.text = _awakeChats[random];
        }
        public void ThakChat()
        {
            int random = Random.Range(0, _endChats.Length);
            chatting.text = _endChats[random];
        }
        public void CancleChat()
        {
            int random = Random.Range(0, _cancleChats.Length);
            chatting.text = _cancleChats[random];
        }
        public void BuyPriceToChat(int price)
        {
            string itemprice = string.Format("�̰� �ʿ���? 1���� {0}����.", price);
            chatting.text = itemprice;
        }
        public void SellPriceToChat(int price)
        {
            string itemprice = string.Format("�̰Ÿ� 1���� {0}��忡 ���� �� �־�.", price);
            chatting.text = itemprice;
        }
        public void TotalPriceToChat(int price)
        {
            string itemprice = string.Format("�� {0}����.~", price);
            chatting.text = itemprice;
        }
    }
}
