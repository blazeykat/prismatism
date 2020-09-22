using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	internal class RaidersAxe : PassiveItem
	{
		public static void Init()
		{
			string name = "Raiders Axe";
			string resourcePath = "katmod/Resources/V2MiscItems/raidersaxe.png";
			GameObject gameObject = new GameObject(name);
			RaidersAxe item = gameObject.AddComponent<RaidersAxe>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Viking Waraxe";
			string longDesc = "Increases damage by 0.00055 for each enemy killed.\n\nNothing lasts for ever after all.\nI'll love you, till the ocean takes us all.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(165);
		}

		public override void Pickup(PlayerController player)
		{
			bool pickedUp = this.m_pickedUp;
			if (!pickedUp)
			{
				base.Pickup(player);
			}
		}

		protected override void Update()
		{
			base.Update();
			this.BlankAmount();
		}

		private void BlankAmount()
		{
			this.armor = this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All).Count;
			bool flag = this.armor == this.lastArmor;
			if (!flag)
			{
				this.RemoveStat(PlayerStats.StatType.Damage);
				this.AddStat(PlayerStats.StatType.Damage, this.m_owner.KillsThisRun * 0.00055f, StatModifier.ModifyMethod.ADDITIVE);
				base.Owner.stats.RecalculateStats(base.Owner, true, false);
				this.lastArmor = this.armor;
			}
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			debrisObject.GetComponent<RaidersAxe>().m_pickedUpThisRun = true;
			return debrisObject;
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
