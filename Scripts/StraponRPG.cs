using ItemAPI;
using System;
using System.Collections;
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
            item.SetupUnlockOnCustomFlag(CustomDungeonFlags.STRAPONRPG_TRORC_FLAG, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Buy it");
            item.AddItemToTrorcMetaShop(16);
            item.PlaceItemInAmmonomiconAfterItemById(438);
        }
        private void WhenReload(PlayerController player, Gun playerGun)
        {
            if (base.Owner != null)
            {
                bool empty = playerGun.ClipShotsRemaining == 0f;
                if ((empty || player.PlayerHasActiveSynergy("Rocket King")) && !CoolAsIce)
                {
                    CoolAsIce = true;
                    StartCoroutine(StartCooldown());
                    player.HandleProjectile(10f, 7f, 129, false, Vector2.zero, true);
                }
            }
        }

        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
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

        protected override void OnDestroy()
        {
            if (Owner != null)
            {
                base.Owner.OnReloadedGun -= WhenReload;
            }
            base.OnDestroy();
        }

        private static bool CoolAsIce = false;
    }
}
