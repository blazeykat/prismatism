using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class IncubusStartingPassive : PassiveItem
	{
		public static void Init()
		{
			string name = "W.I.P Item which i made good";
			string resourcePath = "katmod/Resources/V2MiscItems/rusticer2.png";
			GameObject gameObject = new GameObject(name);
			IncubusStartingPassive item = gameObject.AddComponent<IncubusStartingPassive>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Tick Tock";
			string longDesc = "Increases damage for each piece of armor.\n\nOriginally used to crush down ores into dusts.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = ItemQuality.SPECIAL;
            item.CanBeDropped = false;
            item.RemovePickupFromLootTables();
        }

        public DamageTypeModifier fireResistance = new DamageTypeModifier()
        {
            damageType = CoreDamageTypes.Fire,
            damageMultiplier = 0

        };

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.healthHaver.damageTypeModifiers.Add(fireResistance);
            EraseArmour();
            player.healthHaver.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float resultValue, float maxValue)
        {
            EraseArmour();
        }

        private void EraseArmour()
        {
            if (Owner && Owner.healthHaver)
            {
                if (Owner.healthHaver.Armor != 0)
                {
                    float armor = Owner.healthHaver.Armor;
                    savedArmor += armor * 2;
                    Owner.healthHaver.Armor = 0;

                    foreach (PlayerItem item in Owner.activeItems)
                    {
                        if (item is IncubusStartingActive active)
                        {
                            active.armour = savedArmor;
                            active.SetLabel($"{active.armour} ");
                        }
                    }
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (Owner)
            {
                foreach (PlayerItem item in Owner.activeItems)
                {
                    if (item is IncubusStartingActive active)
                    {
                        if (this.savedArmor != active.armour)
                        {
                            active.armour = savedArmor;
                            active.SetLabel($"{active.armour} ");
                        }
                    }
                }
            }
        }

        public float savedArmor;
    }
}
