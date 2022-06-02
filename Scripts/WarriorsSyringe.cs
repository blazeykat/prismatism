using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class WarriorsSyringe : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Warrior's Syringe";
            string resourceName = "katmod/Resources/warriorsyringe";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<PassiveItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Chance of rainfall";
            string longDesc = "Increases rate of fire, charge speed and reload speed.\n\nDoesn't stack.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RateOfFire, .20f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ReloadSpeed, -0.45f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ChargeAmountMultiplier, .15f, StatModifier.ModifyMethod.ADDITIVE);
            item.PlaceItemInAmmonomiconAfterItemById(259);
        }
    }
}
