using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class RingOfWealth : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ring Of Wealth";
            string resourceName = "katmod/Resources/V3MiscItems/ringofhate";
            GameObject obj = new GameObject();
            RingOfWealth item = obj.AddComponent<RingOfWealth>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Keys => Money";
            string longDesc = "Opening chests gives you casings.\n\n";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.D;
            item.PlaceItemInAmmonomiconAfterItemById(440);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
        }

        public override void Pickup(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen += Chnest;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen -= Chnest;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            ETGMod.Chest.OnPostOpen -= Chnest;
            base.OnDestroy();
        }

        public void Chnest(Chest chest, PlayerController player)
        {
            if (player)
            {
                AkSoundEngine.PostEvent("Play_OBJ_coin_medium_01", base.gameObject);
                player.carriedConsumables.Currency += 10;
                player.BloopItemAboveHead(this.sprite);
            }
        }
    }
}