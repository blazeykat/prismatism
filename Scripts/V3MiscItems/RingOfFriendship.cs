using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class RingOfFriendship : PassiveItem
	{
		public static void Init()
		{
			string name = "Ring of Friendship";
			string resourcePath = "katmod/Resources/fishsnack.png";
			GameObject gameObject = new GameObject(name);
			RingOfFriendship item = gameObject.AddComponent<RingOfFriendship>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Bad Company";
			string longDesc = "Drastically increases the chance to find companions.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.D;
		}

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
			this.weight = 3;
			foreach (WeightedGameObject WeightedObject in GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements)
			{
				PickupObject companionMaybe = PickupObjectDatabase.GetById(WeightedObject.pickupId);
				bool flag4 = companionMaybe is CompanionItem;
				if (flag4)
				{
					WeightedObject.weight *= this.weight;
				}
			}
		}

		public float weight;
    }
}
