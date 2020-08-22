using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class HoodedShells : PassiveItem
    {
        public float damageToDo;
        public static void Init()
        {
            string itemName = "Hooded Shells";
            string resourceName = "katmod/Resources/V2MiscItems/hoodedshells";
            GameObject obj = new GameObject();
            HoodedShells item = obj.AddComponent<HoodedShells>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Giant Slayer IV";
            string longDesc = "Does more damage to enemies with higher max HP.\n\nThe bigger they are, the harder they fall.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_DEMONWALL, true);
            item.PlaceItemInAmmonomiconAfterItemById(323);
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
            damageToDo = sourceProjectile.baseData.damage;
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                float hpercent = (arg2.aiActor.healthHaver.GetMaxHealth() / 100);
                if (hpercent > 0.5f) { hpercent = 0.5f; }
                damageToDo *= hpercent;
                //if (arg2.aiActor.healthHaver.IsBoss == false)
                {
                    arg2.aiActor.healthHaver.ApplyDamage(damageToDo, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
                }
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
