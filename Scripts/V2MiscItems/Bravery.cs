using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	internal class Bravery : PassiveItem
	{
		public static void Init()
		{
			string name = "Medal of Bravery";
			string resourcePath = "katmod/Resources/V2MiscItems/medalofbravery.png";
			GameObject gameObject = new GameObject(name);
			Bravery uten = gameObject.AddComponent<Bravery>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Fearless Warrior.";
			string longDesc = "Increases damage for each enemy in the room.";
			uten.SetupItem(shortDesc, longDesc, "psm");
			uten.quality = PickupObject.ItemQuality.C;
			uten.sprite.IsPerpendicular = true;
			uten.PlaceItemInAmmonomiconAfterItemById(451);
		}

		protected override void Update()
		{
			base.Update();
			if (base.Owner != null)
			{
				this.BlankAmount();
			}
		}

		private void BlankAmount()
		{
			this.armor = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
			bool flag = this.armor == this.lastArmor;
			if (!flag)
			{
				this.RemoveStat(PlayerStats.StatType.Damage);
				this.AddStat(PlayerStats.StatType.Damage, (Owner.HasPassiveItem(494) ? this.armor * 0.1f : this.armor * 0.05f), StatModifier.ModifyMethod.ADDITIVE);
				base.Owner.stats.RecalculateStats(base.Owner, true, false);
				this.lastArmor = this.armor;
			}
		}


		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			StatModifier statModifier = new StatModifier
			{
				amount = amount,
				statToBoost = statType,
				modifyType = method
			};
			foreach (StatModifier statModifier2 in this.passiveStatModifiers)
			{
				bool flag = statModifier2.statToBoost == statType;
				if (flag)
				{
					return;
				}
			}
			bool flag2 = this.passiveStatModifiers == null;
			if (flag2)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
				return;
			}
			this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
			{
				statModifier
			}).ToArray<StatModifier>();
		}

		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				if (flag)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}

		private int armor;

		private float lastArmor = 0f;
	}
}
