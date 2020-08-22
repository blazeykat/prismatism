using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
	class BlueCandy : PlayerItem
	{
		public static int BlueCandyID;
		public static void Init()
		{
			string name = "Blue Candy";
			string resourcePath = "katmod/Resources/Candies/bluepop.png";
			GameObject gameObject = new GameObject(name);
			BlueCandy item = gameObject.AddComponent<BlueCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Blankberry";
			string longDesc = "Spawns a blank.\n\nBlankberries are a real fruit, but they have no correlation with the Gungeon or blanks.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			BlueCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(224).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return true;
		}
	}
}


