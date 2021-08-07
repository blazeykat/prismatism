using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	class ClockworkCog : PassiveItem
	{
		public static void Init()
		{
			string name = "Rustic Cog";
			string resourcePath = "katmod/Resources/V2MiscItems/rusticer2.png";
			GameObject gameObject = new GameObject(name);
			ClockworkCog item = gameObject.AddComponent<ClockworkCog>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Tick Tock";
			string longDesc = "Increases damage for each piece of armor.\n\nOriginally used to crush down ores into dusts.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = ItemQuality.B;
			item.PlaceItemInAmmonomiconAfterItemById(114);
		}


		protected override void Update()
		{
			if (Owner)
			{
				this.ShieldAmount();
			}
		}

		private void ShieldAmount()
		{
			if (Owner)
			{
				this.armor = this.m_owner.healthHaver.Armor;
				bool flag = this.armor == this.lastArmor;
				if (!flag)
				{
					if (base.Owner.characterIdentity == PlayableCharacters.Robot)
					{
						this.armor = Mathf.Max(armor - 6, 0);
					}
					this.RemoveStat(PlayerStats.StatType.Damage);
					this.AddStat(PlayerStats.StatType.Damage, this.armor * 0.1f, StatModifier.ModifyMethod.ADDITIVE);
					base.Owner.stats.RecalculateStats(base.Owner, true, false);
					this.lastArmor = this.armor;
				}
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

		/*public void CorruptSynergy()
		{
			if (Owner.passiveItems != lastInventory)
			{
				lastInventory = Owner.passiveItems;
				RemoveStat(PlayerStats.StatType.Health);
				if (Owner.PlayerHasActiveSynergy("Corrupted Soul"))
                {
					AddStat(PlayerStats.StatType.Health, 1);
                }
				base.Owner.stats.RecalculateStats(base.Owner, true, false);
			}
		}*/

		private float armor;

		private float lastArmor = 0f;

		//private List<PassiveItem> lastInventory = new List<PassiveItem>();
	}
}
