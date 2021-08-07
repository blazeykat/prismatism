using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class SawbladeItem : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Sawblade";
            string resourceName = "katmod/Resources/V3MiscItems/jarfullofsouls";
            GameObject obj = new GameObject(itemName);
            SawbladeItem item = obj.gameObject.AddComponent<SawbladeItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Sharp and Spinny";
            string longDesc = "Throws a deadly sawblade, which does massive damage but can damage you.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.D;
            item.PlaceItemInAmmonomiconAfterItemById(155);
            item.SetCooldownType(ItemBuilder.CooldownType.Damage, 800);
            item.consumable = false;

        }
    }
}
