using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
	class HeartCandy : PlayerItem
	{
		public static int HeartCandyID;
		public static void Init()
		{
			string name = "Pink Candy";
			string resourcePath = "katmod/Resources/Candies/heartpop.png";
			GameObject gameObject = new GameObject(name);
			HeartCandy item = gameObject.AddComponent<HeartCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Heartichoke";
			string longDesc = "Spawns a full-heart health item.\n\nHere you go Reto.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			HeartCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
			user.healthHaver.ApplyHealing(1);
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		public override bool CanBeUsed(PlayerController user)
		{
			bool result = user.healthHaver.GetCurrentHealth() != user.healthHaver.GetMaxHealth();
			return result;
		}
	}
}


