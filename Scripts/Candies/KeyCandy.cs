using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
	class KeyCandy : PlayerItem
	{
		public static int GreyCandyID;
		public static void Init()
		{
			string name = "Gray Candy";
			string resourcePath = "katmod/Resources/Candies/redpop.png";
			GameObject gameObject = new GameObject(name);
			KeyCandy item = gameObject.AddComponent<KeyCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Lockolate.";
			string longDesc = "Spawns a key.\n\nMost keys don't taste good, but these ones do.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			GreyCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(67).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
			
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return true;
		}
	}
}


