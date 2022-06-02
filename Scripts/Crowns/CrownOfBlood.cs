using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
    class CrownOfBlood : PassiveItem
    {
		public static int ID;
		public static void Init()
		{
			string name = "Crown of blood";
			string resourcePath = "katmod/Resources/V2MiscItems/crownofblood";
			GameObject gameObject = new GameObject(name);
			CrownOfBlood item = gameObject.AddComponent<CrownOfBlood>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Bloody Murder";
			string longDesc = "Increases enemy count, in exchange for stat boosts.\n\nThis fella's a long way from home.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			CrownOfBlood.ID = item.PickupObjectId;
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			item.PlaceItemInAmmonomiconAfterItemById(214);
			item.AddPassiveStatModifier(PlayerStats.StatType.Damage, 0.75f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddPassiveStatModifier(PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1.5f, StatModifier.ModifyMethod.ADDITIVE);
			item.RemovePickupFromLootTables();
		}

		public void OnNewRoom()
        {
			List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null && (base.Owner.CurrentRoom.area.PrototypeRoomCategory != PrototypeDungeonRoom.RoomCategory.SPECIAL))
			{
				int count = activeEnemies.Count;
				for (int i = 0; i < count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && !activeEnemies[i].IsTransmogrified && activeEnemies[i].EnemyGuid != "22fc2c2c45fb47cf9fb5f7b043a70122")
                    {
						AIActor actor2 = BoxOTools.SummonAtRandomPosition(activeEnemies[i].EnemyGuid, base.Owner);
						actor2.CanDropCurrency = true;
						actor2.AssignedCurrencyToDrop = activeEnemies[i].AssignedCurrencyToDrop;
						actor2.HandleReinforcementFallIntoRoom();
                    }
                }
            }
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnEnteredCombat += this.OnNewRoom;
		}

		public override DebrisObject Drop(PlayerController player)
		{
			player.OnEnteredCombat -= this.OnNewRoom;
			return base.Drop(player);
		}
	}
}
