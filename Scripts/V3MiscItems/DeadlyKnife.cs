using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class DeadlyKnife : PlayerItem
	{
		public static void Init()
		{
			string name = "Fatal Knife";
			string resourcePath = "katmod/Resources/V3MiscItems/reptileknife";
			GameObject gameObject = new GameObject(name);
			DeadlyKnife item = gameObject.AddComponent<DeadlyKnife>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Soulless";
			string longDesc = "Consumes ALL your hearts (Not heart containers), converts them to shields. Gives 6 shields if you already lack a soul.\n\nCovered in reptilian blood.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.C;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}

		protected override void DoEffect(PlayerController user)
		{
			if (user.characterIdentity == PlayableCharacters.Robot || user.healthHaver.GetCurrentHealth() == 0)
			{
				user.healthHaver.Armor += 6;
			}
			else
			{
				float health = user.healthHaver.GetCurrentHealth();
				user.healthHaver.Armor += health * (user.HasGun(341) || user.HasGun(479) ? 3 : 2);
				user.healthHaver.ForceSetCurrentHealth(0);
			}
		}
	}
}
