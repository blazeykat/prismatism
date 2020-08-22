using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
	class GoldenCandy : PlayerItem
	{
		public static int GoldenCandyID;
		public static void Init()
		{
			string name = "Gold Candy";
			string resourcePath = "katmod/Resources/Candies/goldenpop.png";
			GameObject gameObject = new GameObject(name);
			GoldenCandy item = gameObject.AddComponent<GoldenCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Moneycomb";
			string longDesc = "Spawns some casings.\n\nTastes metallic. Either that or your mouth is bleeding.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			GoldenCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(70).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return true;
		}
	}
}


