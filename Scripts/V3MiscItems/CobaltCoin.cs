using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class CobaltCoin : PassiveItem
	{
		public static void Init()
		{
			string name = "Cobalt Coin";
			string resourcePath = "katmod/Resources/V3MiscItems/cobaltcoin.png";
			GameObject gameObject = new GameObject(name);
			CobaltCoin item = gameObject.AddComponent<CobaltCoin>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Worth It";
			string longDesc = "Doubles all shop items, 50% chance to use double keys on chests.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(272);
		}

        public override void Pickup(PlayerController player)
        {
            player.OnItemPurchased += Doublinator;
			ETGMod.Chest.OnPostOpen += Event;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnItemPurchased -= Doublinator;
            ETGMod.Chest.OnPostOpen -= Event;
            return base.Drop(player);
        }

        void Event(Chest chest, PlayerController player) 
		{
			if (player.carriedConsumables.KeyBullets > 0 && BoxOTools.BasicRandom(0.5f))
            {
				player.carriedConsumables.KeyBullets--;

			}
        }

        private void Doublinator(PlayerController arg1, ShopItemController arg2)
        {
            if (arg1 && arg2 && arg2.item)
            {
				if (arg2.item.quality != ItemQuality.COMMON && arg2.item.quality != ItemQuality.SPECIAL && arg2.item.quality != ItemQuality.EXCLUDED && BoxOTools.BasicRandom(0.4f))
                {
					LootEngine.GivePrefabToPlayer(BoxOTools.GetTotallyRandomItem(arg2.item.quality, false).gameObject, arg1);
                }
            }
        }
    }
}
