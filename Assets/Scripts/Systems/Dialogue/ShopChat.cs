using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ciart.Pagomoa.Systems.Dialogue
{
    public class ShopChat : MonoBehaviour
    {
        public TextMeshProUGUI chatting;
        [SerializeField] private string[] awakeChats;
        [SerializeField] private string[] endChats;
        [SerializeField] private string[] cancelChats;

        public void AwakeChat()
        {
            var random = Random.Range(0, awakeChats.Length);
            chatting.text = awakeChats[random];
        }

        public void ThankChat()
        {
            var random = Random.Range(0, endChats.Length);
            chatting.text = endChats[random];
        }

        public void CancelChat()
        {
            var random = Random.Range(0, cancelChats.Length);
            chatting.text = cancelChats[random];
        }

        public void BuyPriceToChat(int price)
        {
            var itemPrice = $"이게 필요해? 1개당 {price}골드야.";
            chatting.text = itemPrice;
        }

        public void SellPriceToChat(int price)
        {
            var itemPrice = $"이거면 1개당 {price}골드에 사줄 수 있어.";
            chatting.text = itemPrice;
        }

        public void TotalPriceToChat(int price)
        {
            var itemPrice = $"총 {price}골드야.";
            chatting.text = itemPrice;
        }
    }
}
