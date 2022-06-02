using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class StarFruit : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Star Fruit";
            string resourceName = "katmod/Resources/V3MiscItems/starfruit";
            GameObject obj = new GameObject();
            StarFruit item = obj.AddComponent<StarFruit>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Heavenly Fruit";
            string longDesc = "Launches the power of the stars upon reloading.\n\nTastes good.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(258);
        }
        private void WhenReload(PlayerController player, Gun playerGun)
        {
            if (base.Owner != null)
            {
                bool empty = playerGun.ClipShotsRemaining == 0f;
                if (empty && !CoolAsIce)
                {
                    CoolAsIce = true;
                    StartCoroutine(StartCooldown());
                    for (int i = 0; i < 5; i++)
                    {
                        player.HandleProjectile(32, 8, 52, true, Vector2.zero, true, playerGun.CurrentAngle + (i * 72));
                    }
                }
            }
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(5f);
            CoolAsIce = false;
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun += WhenReload;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReloadedGun -= WhenReload;

            return base.Drop(player);
        }

        static bool CoolAsIce = false;

        protected override void OnDestroy()
        {
            if (Owner)
            {
                base.Owner.OnReloadedGun -= WhenReload;
            }
            base.OnDestroy();
        }

    }
}
