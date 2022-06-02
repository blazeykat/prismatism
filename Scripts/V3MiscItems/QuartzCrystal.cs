using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class QuartzCrystal : DamageReductionItem
	{
		public static void Init()
		{
			string name = "Quartz Crystal";
			string resourcePath = "katmod/Resources/V3MiscItems/quartz";
			GameObject gameObject = new GameObject(name);
			QuartzCrystal item = gameObject.AddComponent<QuartzCrystal>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Pure";
			string longDesc = "Bullets of the jammed do less damage.\n\nContains minor purification power.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.AddPassiveStatModifier(PlayerStats.StatType.Damage, .15f);
			item.quality = ItemQuality.D;
			item.PlaceItemInAmmonomiconAfterItemById(314);
		}
	}
}
