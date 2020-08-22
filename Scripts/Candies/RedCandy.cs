using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;
using katmod.ExpandAudio;

namespace katmod
{
	class RedCandy : PlayerItem
	{
		public static int MonsterCandyID; //this is monster candy now
		public static void Init()
		{
			string name = "Monster Candy";
			string resourcePath = "katmod/Resources/Candies/purplecandy.png";
			GameObject gameObject = new GameObject(name);
			RedCandy item = gameObject.AddComponent<RedCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Ruined";
			string longDesc = "Increases your damage and spawns a half-heart health item.\n\nHas a distinct, un-licorice-like flavor.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			MonsterCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
				StatModifier damageboost = new StatModifier();
				damageboost.statToBoost = PlayerStats.StatType.Damage;
				damageboost.amount = 0.05f;
				damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
				user.ownerlessStatModifiers.Add(damageboost);
				user.stats.RecalculateStats(user, true, false);
			try
			{
				AkSoundEngine.PostEvent("Play_Laugh", base.gameObject);
			} catch (Exception E)
            {
				ETGModConsole.Log($"{E}");
			}
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		public override bool CanBeUsed(PlayerController user)
		{
			bool result = true;
			return result;
		}
	}
}


