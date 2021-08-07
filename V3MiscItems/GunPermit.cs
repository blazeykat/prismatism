using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class GunPermit : PassiveItem
    {
        public static void Init()
        {
            string name = "Gun Permit";
            string resourcePath = "katmod/Resources/V3MiscItems/gunpermit.png";
            GameObject gameObject = new GameObject(name);
            GunPermit item = gameObject.AddComponent<GunPermit>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Controlled";
            string longDesc = "Decreases shop prices, increase damage of purchased guns by 30%.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(115);
            item.AddPassiveStatModifier(PlayerStats.StatType.GlobalPriceMultiplier, -0.1f, StatModifier.ModifyMethod.ADDITIVE);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnItemPurchased += Doublinator;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnItemPurchased -= Doublinator;
            return base.Drop(player);
        }

        private void Doublinator(PlayerController arg1, ShopItemController arg2)
        {
            TimesBought++;
            if (arg1 && arg2 && arg2.item)
            {
                if (arg2.item is Gun)
                {
                    arg1.CurrentGun.gameObject.AddComponent<HowToExplain>();
                }
            }
        }

        public static int TimesBought;

        public class HowToExplain : GunBehaviour
        {
            public override void PostProcessProjectile(Projectile projectile)
            {
                PlayerController playerController = this.gun.CurrentOwner as PlayerController;
                float damagemod = 1.3f;
                if (playerController.PlayerHasActiveSynergy("Print Money"))
                {
                    damagemod += TimesBought * 0.05f;
                }
                projectile.baseData.damage *= damagemod;
                base.PostProcessProjectile(projectile);
            }
        }
    }
}
