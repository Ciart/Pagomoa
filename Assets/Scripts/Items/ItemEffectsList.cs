using Ciart.Pagomoa.Systems;
using Ciart.Pagomoa.Worlds;
using UnityEngine;

namespace Ciart.Pagomoa.Items
{
    [System.Serializable]
    public class CopperEffect : NewItemEffect
    {
        public override void Effect(string itemID)
        {
            var player = Game.Instance.player;
            var amount = ResourceSystem.instance.GetItem(itemID).hungeyAmount;
            player.Hungry += amount;
            
            Debug.Log("구리 섭취");
        }
    }


    [System.Serializable]
    public class BombEffect : NewItemEffect
    {
        public override void Effect(string itemID)
        {
            Debug.Log("BombEffect : NewItemEffect - 폭탄 설치");
            var playerTransform = Game.Instance.player.transform;

            if (playerTransform == null) return;

            var itemEntity = DataBase.data.GetItemEntity();

            var spawnPosition = WorldManager.ComputeCoords(playerTransform.position);
            spawnPosition.y -= 1;

            var entity = itemEntity.InstantiateItem(itemEntity, WorldManager.ComputePosition(spawnPosition));

            if (entity.transform.GetChild(0).TryGetComponent<BoxCollider2D>(out var col)) { col.enabled = false; }

            entity.Item = ResourceSystem.instance.GetItem("Bomb");

            entity.gameObject.AddComponent<Bomb>().Init(3, 3);
        }
    }


    public class BandageEffect : NewItemEffect
    {
        public override void Effect(string itemID)
        {
            var player = Game.Instance.player;

            player.Health += 100;

            Debug.Log("오래된 붕대 사용");
        }
    }


    [System.Serializable]
    public class AirBurbleEffect : NewItemEffect
    {
        public override void Effect(string itemID)
        {
            var player = Game.Instance.player;
            player.Oxygen += 100;

            Debug.Log("공기 섭취");
        }
    }


    [System.Serializable]
    public class CookieEffect : NewItemEffect
    {
        public override void Effect(string itemID)
        {
            var player = Game.Instance.player;
            player.Hungry += 100;

            Debug.Log("구리과자 섭취");
        }
    }


    [System.Serializable]
    public class AwfulSnakeBloodEffect : NewItemEffect
    {
        public override void Effect(string itemID)
        {
            var player = Game.Instance.player;

            player.Health = player.MaxHealth;

            Debug.Log("역겨운 뱀의 피 섭취");
        }
    }
}
