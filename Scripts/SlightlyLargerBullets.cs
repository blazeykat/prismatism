using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class SlightlyLargerBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Slightly Larger Bullets";
            string resourceName = "katmod/Resources/slightlylargerthanusualbullets";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<PassiveItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Slightly larger than usual";
            string longDesc = "These bullets are at a size which is slightly larger than the original size that you may be accustommed to.\n\n\"I say it would be funnier if they were called that, but were massive. Like Titan bullets massive.\"";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.D;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            item.PlaceItemInAmmonomiconAfterItemById(286);
        }
    }
}
