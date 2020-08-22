using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{

    class HyperBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Low Priest's Cloak";
            string resourceName = "katmod/Resources/V2MiscItems/lowpriestcape";
            GameObject obj = new GameObject();
            HyperBullets item = obj.AddComponent<HyperBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Omniscent";
            string longDesc = "A relic of the low priest, before he became more than human.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.2f, StatModifier.ModifyMethod.ADDITIVE);
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(MimicSkin.ID);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            sourceProjectile.ChangeColor(default, UnityEngine.Color.red);
        }


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            woop.spawnShadows = true;
            player.gameObject.AddComponent(woop);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.gameObject.GetComponent<AfterImageTrailController>().spawnShadows = false;
            return base.Drop(player);
        }
        afterimageButBetter woop = new afterimageButBetter()
        {
            dashColor = UnityEngine.Color.red,
            spawnShadows = true,
        };
    }

}
