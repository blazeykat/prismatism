using ItemAPI;
using System;
using UnityEngine;


namespace katmod
{
    class BloodBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Iron Bullets";
            string resourceName = "katmod/Resources/V2MiscItems/bleedingbullets";
            GameObject obj = new GameObject();
            BloodBullets item = obj.AddComponent<BloodBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Execute IV";
            string longDesc = "Does more damage to damaged enemies.\n\nA bullet made of iron, which forcibly removes the iron from your enemies. A living contradiction.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(111);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.specRigidbody.OnPreRigidbodyCollision += OnHitEnemy;
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.aiActor != null && myRigidbody != null && myRigidbody.projectile && otherRigidbody.aiActor.healthHaver)
            {
                float hpercent = otherRigidbody.aiActor.healthHaver.GetCurrentHealthPercentage();
                myRigidbody.projectile.baseData.damage *= 1 + ((0.35f - (hpercent * 0.35f)));
            }
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
