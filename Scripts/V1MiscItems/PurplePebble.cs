using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class PurplePebble : PassiveItem
    {
		public static void Init()
		{
			string name = "Purple Pebble";
			string resourcePath = "katmod/Resources/pureblobonium.png";
			GameObject gameObject = new GameObject(name);
			PurplePebble item = gameObject.AddComponent<PurplePebble>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "What's a \"Knurl\"?";
			string longDesc = "Increases damage, health and accuracy.\n\nCan be used as a shield in dire emergencies.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 2f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);

		}
	}
}
