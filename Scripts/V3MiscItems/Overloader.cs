using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class Overloader : PlayerItem
    {
        public static void Init()
        {
			string name = "Overloader";
			string resourcePath = "katmod/Resources/V3MiscItems/batterybruv";
			GameObject gameObject = new GameObject(name);
			Overloader item = gameObject.AddComponent<Overloader>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "bzzzzzzzzzzzzzzz";
			string longDesc = "Halves your heart containers, converts them to shields. Gives 6 shields if you're already fully charged.\n\nUsed to charge large spaceships.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(485);
			item.consumable = true;
		}

		protected override void DoEffect(PlayerController user)
		{
			if (user.characterIdentity == PlayableCharacters.Robot || user.healthHaver.GetMaxHealth() == 0)
			{
				user.healthHaver.Armor += 6;
			}
			else
			{
				float health = (user.stats.GetBaseStatValue(PlayerStats.StatType.Health) / 2).RoundToNearest(1);
				user.healthHaver.Armor += health * 4;
				user.stats.SetBaseStatValue(PlayerStats.StatType.Health, health, user);
			}
		}
	}
}
