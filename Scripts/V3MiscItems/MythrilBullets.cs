using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Runtime.CompilerServices;

namespace katmod
{
    class MythrilBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Mythril Rounds";
            string resourceName = "katmod/Resources/V3MiscItems/godrounds";
            GameObject obj = new GameObject();
            MythrilBullets item = obj.AddComponent<MythrilBullets> ();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Hardened";
            string longDesc = "Increases damage by a fuck ton.\n\nMade from the rare mineral Orichalcum.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.6f, StatModifier.ModifyMethod.ADDITIVE);
            item.PlaceItemInAmmonomiconAfterItemById(528);
        }
    }
}
