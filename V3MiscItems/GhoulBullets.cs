using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class GhoulBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ghoullets";
            string resourceName = "katmod/Resources/V3MiscItems/ghoullet";
            GameObject obj = new GameObject();
            GhoulBullets item = obj.AddComponent<GhoulBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Haunting";
            string longDesc = "Bullets can pierce some walls.\n\nMade from the \"Flesh\" of the hollowpoints of the gungeon.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.D;
            item.PlaceItemInAmmonomiconAfterItemById(111);
        }

        public override void Pickup(PlayerController player)
        {
            player.PostProcessProjectile += Player_PostProcessProjectile;
            base.Pickup(player);
        }

        private void Player_PostProcessProjectile(Projectile arg1, float arg2)
        {
            if (arg1)
            {
                arg1.PenetratesInternalWalls = true;
                arg1.UpdateCollisionMask();
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= Player_PostProcessProjectile;
            return base.Drop(player);
        }
    }
}
