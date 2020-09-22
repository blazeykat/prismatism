using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
    using UnityEngine;

namespace katmod
{
    class StraponRPG : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Stick on Rocket Launcher";
            string resourceName = "katmod/Resources/stickonrpg";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<StraponRPG>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            String shortDesc = "ATG";
            String longDesc = "Launches a rocket upon reloading.\n\nA rocket launcher, made out of pieces of another rocket launcher.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(438);
        }
        private void WhenReload(PlayerController player, Gun playerGun)
        {
            if (base.Owner != null)
            {
                bool empty = playerGun.ClipShotsRemaining == 0f;
                if ((empty || (player.HasGun(129) || player.HasGun(16))) && !CoolAsIce)
                {
                    CoolAsIce = true;
                    StartCoroutine(StartCooldown());
                    player.HandleProjectile(10f, 5f, 129);
                }
            }
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
            CoolAsIce = false;
            yield break;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(player.OnReloadedGun, new Action<PlayerController, Gun>(this.WhenReload));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(player.OnReloadedGun, new Action<PlayerController, Gun>(this.WhenReload));

            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner != null)
            {
                base.Owner.OnReloadedGun -= WhenReload;
            }
            base.OnDestroy();
        }

        static bool CoolAsIce = false;
    }
}
