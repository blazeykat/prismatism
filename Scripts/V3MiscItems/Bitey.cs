using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class Bitey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Bitey";
            string resourceName = "katmod/Resources/V3MiscItems/spider";
            GameObject obj = new GameObject();
            Bitey item = obj.AddComponent<Bitey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Toxic";
            string longDesc = "Has a chance to slow down enemies on hit. Increases damage against slowed enemies.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
        }


        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += ProcessPostly;
        }

        private void ProcessPostly(Projectile projectile, float arg2)
        {
            if (projectile)
            {
                /*if (BoxOTools.BasicRandom(0.9f / arg2))*/
                {
                    projectile.AdjustPlayerProjectileTint(Color.gray, 5);
                    projectile.OnHitEnemy += SlowDown;
                }
                projectile.specRigidbody.OnPreRigidbodyCollision += PreCollide;
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= ProcessPostly;
            return base.Drop(player);
        }

        public void SlowDown(Projectile projectile, SpeculativeRigidbody specrigbody, bool yes)
        {
            if (specrigbody && specrigbody.healthHaver && specrigbody.aiActor && specrigbody.healthHaver.IsAlive)
            {
                Gun gun = ETGMod.Databases.Items["triple_crossbow"] as Gun;
                GameActorSpeedEffect speedEffect = gun.DefaultModule.projectiles[0].speedEffect;
                speedEffect.duration = 5f;
                specrigbody.aiActor.ApplyEffect(speedEffect);
            }
        }

        public void PreCollide(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.aiActor != null && myRigidbody != null && myRigidbody.projectile && otherRigidbody.aiActor.healthHaver)
            {
                if (otherRigidbody.aiActor.GetEffect("effect") != null)
                {
                    myRigidbody.projectile.baseData.damage *= 2;
                }
            }
        }
    }
}
