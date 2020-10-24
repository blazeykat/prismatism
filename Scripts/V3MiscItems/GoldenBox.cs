using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class GoldenBox : PassiveItem
	{
		public static void Init()
		{
			string name = "Golden Box";
			string resourcePath = "katmod/Resources/V3MiscItems/goldchest.png";
			GameObject gameObject = new GameObject(name);
			GoldenBox item = gameObject.AddComponent<GoldenBox>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Think Outside";
			string longDesc = "Doubles all chest items, increases shop prices.";
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.GlobalPriceMultiplier, 0.5f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.S;
			item.PlaceItemInAmmonomiconAfterItemById(525);
		}

        public override void Pickup(PlayerController player)
        {
			ETGMod.Chest.OnPostOpen += MoreItems;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
			ETGMod.Chest.OnPostOpen -= MoreItems;
            return base.Drop(player);
        }

		public void MoreItems(Chest chest, PlayerController player)
        {
			if (!GameStatsManager.Instance.IsRainbowRun && chest && chest.contents != null)
            {
				foreach (PickupObject pickup in chest.contents)
				{
					if (BoxOTools.BasicRandom(0.4f))
					{
						GameManager.Instance.RewardManager.SpawnTotallyRandomItem(player.specRigidbody.UnitCenter, pickup.quality, pickup.quality);
					}
				}
			}
        }
    }
}
