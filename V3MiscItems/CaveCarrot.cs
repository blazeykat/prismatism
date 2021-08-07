using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class CaveCarrot : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Salsify";
            string resourceName = "katmod/Resources/V3MiscItems/cavecarrot";
            GameObject obj = new GameObject();
            CaveCarrot item = obj.AddComponent<CaveCarrot>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Cave Carrot";
            string longDesc = "Increases max HP by 1.\n\nA delicious treat cultivated for its beautiful flower and delicious root.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(258);
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }
    }
}
