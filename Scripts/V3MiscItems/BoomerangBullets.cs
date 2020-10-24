using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class BoomerangBullets : PassiveItem
    {
        public float damageToDo;
        public static void Init()
        {
            string itemName = "BulletRangs";
            string resourceName = "katmod/Resources/V3MiscItems/bulletrang";
            GameObject obj = new GameObject();
            BoomerangBullets item = obj.AddComponent<BoomerangBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Spin";
            string longDesc = "Bullets go backwards, like a boomerang.\n\nBullets straight from the bottom of the Gungeon, where all the bullet kins walk upside down.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.D;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, -.25f);
            item.PlaceItemInAmmonomiconAfterItemById(448);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                BoomerangEffect boom = sourceProjectile.gameObject.AddComponent<BoomerangEffect>();
                boom.m_speed = sourceProjectile.baseData.speed / 10;
                boom.startingDamage = sourceProjectile.baseData.damage;
                sourceProjectile.baseData.range *= 10;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
            }
            this.damageToDo = sourceProjectile.baseData.damage;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;

            return base.Drop(player);
        }
    }

}
