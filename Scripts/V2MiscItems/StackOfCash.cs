using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class StackOfCash : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Stack Of Cash";
            string resourceName = "katmod/Resources/V2MiscItems/stackofcash";
            GameObject obj = new GameObject();
            StackOfCash item = obj.AddComponent<StackOfCash>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Pop Pop";
            string longDesc = "The raw money-power gives you a chance to shoot multiple projectiles at once.\n\nWorth 100 Hegemony credits, or 39947.93 dogcoin.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.A;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.ExtremeShadowBulletChance, 20f, StatModifier.ModifyMethod.ADDITIVE);
            item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET, true);
            item.PlaceItemInAmmonomiconAfterItemById(490);
        }
    }
}
