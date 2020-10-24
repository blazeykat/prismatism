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

        protected override void Update()
        {
            base.Update();
            if (base.m_owner != null)
            {
                if (lastGun == null || lastGun != base.m_owner.CurrentGun)
                {
                    lastGun = base.m_owner.CurrentGun;
                    HandleBullets();
                }
            }
        }

        private void HandleBullets()
        {
            foreach (Projectile projecs in base.m_owner.CurrentGun.DefaultModule.projectiles)
            {
                projecs.PenetratesInternalWalls = true;
            }
            foreach (ProjectileModule.ChargeProjectile projjers in base.m_owner.CurrentGun.DefaultModule.chargeProjectiles)
            {
                projjers.Projectile.PenetratesInternalWalls = true;
            }
        }

        public Gun lastGun;
    }
}
