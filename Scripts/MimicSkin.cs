using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace katmod
{

	internal class MimicSkin : PassiveItem
	{
		public static int ID;
		public static void Init()
		{
			string name = "Mimic Skin Cape";
			string resourcePath = "katmod/Resources/mimicskincape.png";
			GameObject gameObject = new GameObject(name);
			MimicSkin item = gameObject.AddComponent<MimicSkin>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Leathery wood";
			string longDesc = "Mimics drop bonus items.\n\nPreviously in the gungeon, many a skilled gungeoneer would feel the chest to see if it was real wood or not. You could still do this now, but you probably would want to keep your fingers.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.A;
			item.SetupUnlockOnFlag(GungeonFlags.FRIFLE_REWARD_MIMIC_TOOTH_NECKLACE, true);
			item.PlaceItemInAmmonomiconAfterItemById(293);
			item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
			MimicSkin.ID = item.PickupObjectId;
		}


		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{
			if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null)
			{
				PlayerController player = base.Owner;
				PickupObject ItemofqualityandtypeD = LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.D, GameManager.Instance.RewardManager.ItemsLootTable, false);
				PickupObject ItemofqualityandtypeC = LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.C, GameManager.Instance.RewardManager.ItemsLootTable, false);
				PickupObject ItemofqualityandtypeB = LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.B, GameManager.Instance.RewardManager.ItemsLootTable, false);
				PickupObject ItemofqualityandtypeA = LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, false);
				PickupObject ItemofqualityandtypeS = LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.S, GameManager.Instance.RewardManager.ItemsLootTable, false);
				PickupObject ItemofqualityandtypeAny = LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.B, GameManager.Instance.RewardManager.ItemsLootTable, true);
				bool flag = (enemy.aiActor.IsMimicEnemy);
				if (flag)
				{
					if (fatal)
					{
						if (!(GameStatsManager.Instance.IsRainbowRun))
						{
							switch (enemy.aiActor.EnemyGuid)
							{
								case "2ebf8ef6728648089babb507dec4edb7":
									LootEngine.SpawnItem(ItemofqualityandtypeD.gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
									break;
								case "d8d651e3484f471ba8a2daa4bf535ce6":
									LootEngine.SpawnItem(ItemofqualityandtypeC.gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
									break;
								case "abfb454340294a0992f4173d6e5898a8":
									LootEngine.SpawnItem(ItemofqualityandtypeB.gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
									break;
								case "d8fd592b184b4ac9a3be217bc70912a2":
									LootEngine.SpawnItem(ItemofqualityandtypeA.gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
									break;
								case "6450d20137994881aff0ddd13e3d40c8":
									LootEngine.SpawnItem(ItemofqualityandtypeS.gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
									break;
								default:
									LootEngine.SpawnItem(ItemofqualityandtypeAny.gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
									break;
							}
							if (player.HasPickupID(293))
							{
								if (flag)
								{
									LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
								}
							}
							if (this.doesNNInstalled != true)
							{
								try
								{
									if (player.HasMTGConsoleID("nn:book_of_mimic_anatomy"))
									{
										if (flag)
										{
											LootEngine.SpawnItem(PickupObjectDatabase.GetById(120).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
										}
									}
								}
								catch
								{
									this.doesNNInstalled = true;
								}
							}
						}
					}
				}
			}
		}


		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));

			return base.Drop(player);
		}
        protected override void OnDestroy()
        {
			base.Owner.OnAnyEnemyReceivedDamage -= OnEnemyDamaged;
            base.OnDestroy();
        }

        public bool doesNNInstalled = false;
	}
}
