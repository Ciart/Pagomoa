using System.Collections.Generic;
using Ciart.Pagomoa.Items;
using UnityEngine;

namespace Ciart.Pagomoa.Systems.Inventory
{
    public class Auction : MonoBehaviour
    {
        [SerializeField] private List<string> auctionItemsIDs = new();
        public List<string> GetAuctionItemIDs() => auctionItemsIDs;
    }
}
