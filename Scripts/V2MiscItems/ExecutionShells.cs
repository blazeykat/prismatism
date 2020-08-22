using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class ExecutionShells : PassiveItem
    {
        public float damageToDo;
        public static void Init()
        {
            string itemName = "Execution Shells";
            string resourceName = "katmod/Resources/V2MiscItems/executionshells";
            GameObject obj = new GameObject();
            ExecutionShells item = obj.AddComponent<ExecutionShells>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "First Strike IV";
            string longDesc = "Doubles damage against enemies with more than 50% HP.\n\nUsed by the Executioners of the gungeon, to kill traitorous bullets.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_GUNGEON, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 75);
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
            this.damageToDo = sourceProjectile.baseData.damage;
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                float hpercent = arg2.aiActor.healthHaver.GetCurrentHealthPercentage();
                if (hpercent > 0.5f)
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
