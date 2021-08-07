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

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.healthHaver.OnHealthChanged += HealthHaver_OnHealthChanged;
        }

        private void HealthHaver_OnHealthChanged(float resultValue, float maxValue)
        {
			if (Owner && Owner.healthHaver)
            {
				if (Owner.healthHaver.Armor != 0)
                {
					float armor = Owner.healthHaver.Armor;
					Owner.healthHaver.Armor = 0;
					foreach (PlayerItem item in Owner.activeItems)
                    {
                        if (item is IncubusStartingActive)
                        {
                            item.CurrentDamageCooldown = Mathf.Max(0, item.CurrentDamageCooldown - 500 * armor);
                        }
                    }
                }
            }
        }
    }
}
