using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class OnyxBracelet : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Onyx Bracelet";
            string resourceName = "katmod/Resources/V3MiscItems/onyxbracelet";
            GameObject obj = new GameObject();
            OnyxBracelet item = obj.AddComponent<OnyxBracelet>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Dark Spirits";
            string longDesc = "Gives a flat damage up by two.\n\nCatches wandering souls, and imbues your bullets with them.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 1);
            item.quality = ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            item.PlaceItemInAmmonomiconAfterItemById(440);
        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += OnyxIfy;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= OnyxIfy;
            return base.Drop(player);
        }

        private void OnyxIfy(Projectile arg1, float arg2)
        {
            if (arg1)
            {
                arg1.baseData.damage += 2;
                arg1.AdjustPlayerProjectileTint(Color.gray, 4);
            }
        }
    }
}
