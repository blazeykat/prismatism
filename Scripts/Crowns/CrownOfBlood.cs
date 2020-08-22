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
			string resourcePath = "katmod/Resources/Crowns/CrownOfBlood/crownofblood";
			GameObject gameObject = new GameObject(name);
			CrownOfBlood item = gameObject.AddComponent<CrownOfBlood>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Bloody Murder";
			string longDesc = "Increases enemy count, in exchange for stat boosts.\n\nThis fella's a long way from home.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			CrownOfBlood.ID = item.PickupObjectId;
			item.quality = PickupObject.ItemQuality.D;
			item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			item.PlaceItemInAmmonomiconAfterItemById(214);
			item.AddPassiveStatModifier(PlayerStats.StatType.Damage, 0.175f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddPassiveStatModifier(PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
			item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1.5f, StatModifier.ModifyMethod.ADDITIVE);
			List<string> ids = new List<string>()
			{
				"psm:crown_of_blood",
				"psm:monarchs_shotgun"
			};
			//CustomSynergies.Add("Kings and Queens of Wasteland", ids);
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
						AIActor actor2 = Utilities.SummonAtRandomPosition(activeEnemies[i].EnemyGuid, base.Owner);
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
