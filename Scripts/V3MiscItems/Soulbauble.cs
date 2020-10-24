using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Soulbauble : PassiveItem
	{
		public static void Init()
		{
			string name = "Soulbauble";
			string resourcePath = "katmod/Resources/V3MiscItems/soulstone";
			GameObject gameObject = new GameObject(name);
			Soulbauble item = gameObject.AddComponent<Soulbauble>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Blood for Blood";
			string longDesc = "Has a chance to decrease enemy health drastically. Chance increase with less HP.";
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 3f, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.S;
			item.PlaceItemInAmmonomiconAfterItemById(525);
		}

		public override void Pickup(PlayerController player)
		{
			ETGMod.AIActor.OnPostStart += MoreItems;
			base.Pickup(player);
		}

		public override DebrisObject Drop(PlayerController player)
		{
			ETGMod.AIActor.OnPostStart -= MoreItems;
			return base.Drop(player);
		}

		public void MoreItems(AIActor actor)
		{
			if (actor && actor.healthHaver)
            {
				if (BoxOTools.BasicRandom(Owner.healthHaver.GetCurrentHealthPercentage()))
				{
					float BaseHealth = actor.healthHaver.GetMaxHealth();
					actor.healthHaver.SetHealthMaximum(BaseHealth * 0.5f, null, true);
                }
            }
		}
	}
}
