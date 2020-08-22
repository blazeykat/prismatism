using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class BloodBullets : PassiveItem
    {
        public float damageToDo;
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
            item.quality = ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(111);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                Projectile projectile = sourceBeam.projectile;
                projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
            this.damageToDo = sourceProjectile.baseData.damage;
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                float hpercent = arg2.aiActor.healthHaver.GetCurrentHealthPercentage();
                damageToDo -= ((hpercent - 0.1f) * damageToDo);
                arg2.aiActor.healthHaver.ApplyDamage(damageToDo, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return base.Drop(player);
        }
    }

}
