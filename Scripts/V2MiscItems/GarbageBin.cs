using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
	class GarbageBin : PlayerItem
	{
		public static void Init()
		{
			string name = "Garbage Bin";
			string resourcePath = "katmod/Resources/V2MiscItems/garbage.png";
			GameObject gameObject = new GameObject(name);
			GarbageBin item = gameObject.AddComponent<GarbageBin>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "funny";
			string longDesc = "Spawns 3 junk on use.\n\nAlso can summon Garbage.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.D;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
			item.PlaceItemInAmmonomiconAfterItemById(301);
			item.SetupUnlockOnStat(TrackedStats.WOODEN_CHESTS_BROKEN, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 19);
		}
		protected override void DoEffect(PlayerController user)
		{
			for (int counter = 0; counter < 3; counter++)
            {
				if (Utilities.BasicRandom(base.LastOwner, 0.975f, 100f) && !base.LastOwner.HasPickupID(580) && !base.LastOwner.HasPickupID(641))
				{
					if (Utilities.UncoolRandom(0.05f) && base.LastOwner.HasPickupID(580))
					{
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(580).gameObject, base.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
					}
					else if (!base.LastOwner.HasPickupID(641))
					{
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(641).gameObject, base.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
					}
				}
				else
				{
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(127).gameObject, base.LastOwner.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
				}
			}
			Application.OpenURL("https://modworkshop.net/mod/27616");
		}
		public override bool CanBeUsed(PlayerController user)
		{
			return true;
		}
	}
}


