using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;

namespace katmod
{
	class EnchantedTome : PlayerItem
	{
		public static int pick3;
		public static void Init()
		{
			string name = "Enchanted Book";
			string resourcePath = "katmod/Resources/V2MiscItems/enchantedbook.png";
			GameObject gameObject = new GameObject(name);
			EnchantedTome item = gameObject.AddComponent<EnchantedTome>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "RNG Prayer";
			string longDesc = "Gives you three random permanent buffs on use.\n\nA copy of Gunjuration 101 which, surpisingly, doesn't try to kill you.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.A;
			ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.None, 3f);
			item.consumable = true;
			item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_HIGH_PRIEST, true);
			item.PlaceItemInAmmonomiconAfterItemById(487);
		}

		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{
			PlayerController user = base.LastOwner;
			float coolness = user.stats.GetStatValue(PlayerStats.StatType.Coolness);
			float chance = 0.98f - (coolness / 50);
			bool flag = UnityEngine.Random.value > chance;
			if (fatal)
			{
				if (flag)
				{
					if (enemy.specRigidbody != null && enemy.aiActor != null && base.LastOwner != null && enemy != null)
					{
						switch (pick3)
						{
							case 1:
								LootEngine.SpawnItem(PickupObjectDatabase.GetById(67).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
								break;
							case 2:
								LootEngine.SpawnItem(PickupObjectDatabase.GetById(565).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
								break;
							case 3:
								StatModifier damageboost = new StatModifier();
								damageboost.statToBoost = PlayerStats.StatType.Damage;
								damageboost.amount = 0.1f;
								damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
								user.ownerlessStatModifiers.Add(damageboost);
								user.stats.RecalculateStats(user, true, false);
								AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
								break;
							case 4:
								IntVector2? intVector = new IntVector2?(this.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
								var BulletKin = EnemyDatabase.GetOrLoadByGuid("a446c626b56d4166915a4e29869737fd");
								if (enemy.aiActor.EnemyGuid != "a446c626b56d4166915a4e29869737fd")
								{
									AIActor aiactor = AIActor.Spawn(BulletKin.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
									aiactor.HandleReinforcementFallIntoRoom(0f);
								}
								break;
						}
					}
				}
			}
		}
		protected override void DoEffect(PlayerController user)
		{
			bool error = false;
			System.Random rand = new System.Random();
			StatModifier damageboost = new StatModifier();
			switch (rand.Next(1, 5))
			{
				default:
					ETGModConsole.Log("well crap.1");
					damageboost.statToBoost = PlayerStats.StatType.Damage;
					damageboost.amount = 0.2f;
					damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					error = true;
					break;
				case 1:
					damageboost.statToBoost = PlayerStats.StatType.Damage;
					damageboost.amount = 0.2f;
					damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					break;
				case 2:
					damageboost.statToBoost = PlayerStats.StatType.RateOfFire;
					damageboost.amount = 0.3f;
					damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					break;
				case 3:
					damageboost.statToBoost = PlayerStats.StatType.ProjectileSpeed;
					damageboost.amount = 0.4f;
					damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					break;
				case 4:
					damageboost.statToBoost = PlayerStats.StatType.ReloadSpeed;
					damageboost.amount = -0.3f;
					damageboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					break;
			}
			user.ownerlessStatModifiers.Add(damageboost);
			user.stats.RecalculateStats(user, true, false);
			switch (rand.Next(1, 8))
			{
				default:
					ETGModConsole.Log("oh crap.2");
					StatModifier damageboost4 = new StatModifier();
					damageboost4.statToBoost = PlayerStats.StatType.Damage;
					damageboost4.amount = 0.3f;
					damageboost4.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(damageboost4);
					user.stats.RecalculateStats(user, true, false);
					error = true;
					break;
				case 1:
					StatModifier damagebossboost = new StatModifier();
					damagebossboost.statToBoost = PlayerStats.StatType.DamageToBosses;
					damagebossboost.amount = 0.5f;
					damagebossboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(damagebossboost);
					user.stats.RecalculateStats(user, true, false);
					break;
				case 2:
					StatModifier damageboost2 = new StatModifier();
					damageboost2.statToBoost = PlayerStats.StatType.Damage;
					damageboost2.amount = 1.25f;
					damageboost2.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
					user.ownerlessStatModifiers.Add(damageboost2);
					user.stats.RecalculateStats(user, true, false);
					break;
				case 3:
					StatModifier moneyboost = new StatModifier();
					moneyboost.statToBoost = PlayerStats.StatType.MoneyMultiplierFromEnemies;
					moneyboost.amount = .5f;
					moneyboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(moneyboost);
					user.stats.RecalculateStats(user, true, false);
					break;
				case 4:
					StatModifier priceboost = new StatModifier();
					priceboost.statToBoost = PlayerStats.StatType.GlobalPriceMultiplier;
					priceboost.amount = .5f;
					priceboost.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(priceboost);
					user.stats.RecalculateStats(user, true, false);
					break;
				case 5:
					StatModifier damageboost3 = new StatModifier();
					damageboost3.statToBoost = PlayerStats.StatType.Damage;
					damageboost3.amount = 0.5f;
					damageboost3.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(damageboost3);
					StatModifier cursed = new StatModifier();
					cursed.statToBoost = PlayerStats.StatType.Damage;
					cursed.amount = 1.5f;
					cursed.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(cursed);
					user.stats.RecalculateStats(user, true, false);
					break;
				case 6:
					StatModifier bonk = new StatModifier();
					bonk.statToBoost = PlayerStats.StatType.KnockbackMultiplier;
					bonk.amount = 5f;
					bonk.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(bonk);
					user.stats.RecalculateStats(user, true, false);
					break;
				case 7:
					StatModifier swag = new StatModifier();
					swag.statToBoost = PlayerStats.StatType.Coolness;
					swag.amount = 7f;
					swag.modifyType = StatModifier.ModifyMethod.ADDITIVE;
					user.ownerlessStatModifiers.Add(swag);
					user.stats.RecalculateStats(user, true, false);
					break;
			}
			if (error)
			{
				ETGModConsole.Log("you found an error, report this to blazeykat");
			}
			pick3 = rand.Next(1, 5);
			if (pick3 == 2)
			{
				ETGModConsole.Log("you did it1");
			}
			user.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(user.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}
		public override bool CanBeUsed(PlayerController user)
		{
			bool result = true;
			return result;
		}
	}
}


