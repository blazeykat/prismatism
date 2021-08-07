using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class ScarecrowsHead : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Scarecrow's Head";
            string resourceName = "katmod/Resources/V3MiscItems/scarecrow";
            GameObject obj = new GameObject();
            ScarecrowsHead item = obj.AddComponent<ScarecrowsHead>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Yellow like a scarecrow";
            string longDesc = "Has a 10% chance to deal double damage.\n\nContains the spirit of a cowboy, whose eyes were rather strange.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(168);
        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += Player_PostProcessProjectile;
            base.Pickup(player);
        }

        private void Player_PostProcessProjectile(Projectile projectile, float arg2)
        {
            if (projectile && BoxOTools.BasicRandom(0.9f))
            {
                projectile.baseData.damage *= 2;
                projectile.AdjustPlayerProjectileTint(Color.yellow, 5);
            }
        }
    }
}
