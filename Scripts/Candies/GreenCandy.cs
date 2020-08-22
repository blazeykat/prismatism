using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
    class GreenCandy : PlayerItem
    {
		public static int GreenCandyID;
        public static void Init()
        {
			string name = "Green Candy";
			string resourcePath = "katmod/Resources/Candies/greenpop.png";
			GameObject gameObject = new GameObject(name);
			GreenCandy item = gameObject.AddComponent<GreenCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Carammol";
			string longDesc = "Spawns an ammo box.\n\nI have no clue how the ammo box fits in the wrapper.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			GreenCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return true;
		}
	}
}
    

