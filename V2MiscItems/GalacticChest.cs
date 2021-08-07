using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;
using System.IO;

namespace katmod
{
	class GalacticChest : PlayerItem
	{
		public static int BlueCandyID;
		public static void Init()
		{
			string name = "Galactic Cardboard Box";
			string resourcePath = "katmod/Resources/Candies/bluepop.png";
			GameObject gameObject = new GameObject(name);
			GalacticChest item = gameObject.AddComponent<GalacticChest>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Spacial Storage";
			string longDesc = "Swaps your current gun for the gun inside the chest.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			BlueCandyID = item.PickupObjectId;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = false;
			item.SetCooldownType(ItemBuilder.CooldownType.None, 0);
		}
		protected override void DoEffect(PlayerController user)
		{
			try
			{
				//string text = System.IO.File.ReadAllText(@Path.Combine(Module.ConfigDirectory, "simplestats.json"));
				string text = System.IO.File.ReadAllText(@"katmod/Resources/MagicChest/ChestItem.txt");
				ETGModConsole.Log(text);
				if (text != null)
				{
					PickupObject pickup = Gungeon.Game.Items[text];
					LootEngine.GivePrefabToPlayer(pickup.gameObject, user);
				}
				else
				{

					LootEngine.GivePrefabToPlayer(Gungeon.Game.Items["magnum"].gameObject, user);
				}
				System.IO.StreamWriter file = new System.IO.StreamWriter(@"katmod/Resources/MagicChest/ChestItem.txt");
				file.WriteLine(user.CurrentGun.name);
			} catch (Exception ah)
            {
				ETGModConsole.Log($"{ah}");
            }
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return !user.CurrentGun.InfiniteAmmo;
		}
	}
}


