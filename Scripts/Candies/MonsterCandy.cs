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
	class MonsterCandy : PlayerItem
	{
		public static int RedCandyID; //this is redcandy now
		public static void Init()
		{
			string name = "Red Candy";
			string resourcePath = "katmod/Resources/Candies/cursepop.png";
			GameObject gameObject = new GameObject(name);
			MonsterCandy item = gameObject.AddComponent<MonsterCandy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Salted carammol";
			string longDesc = "Spawns a spread ammo box.\n\nHas \"monster candy\" on the wrapper, with poor attempts of removing it being present.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.SPECIAL;
			RedCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
		}
		protected override void DoEffect(PlayerController user)
		{
			LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, this.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
			AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return true;
		}
	}
}


