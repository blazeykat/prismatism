using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace katmod
{
    class EnemyPrinter : PassiveItem
    {
        public static void Init()
        {
            string name = "Enemy Printer";
            string resourcePath = "katmod/Resources/WyrmSet/spogrechamp.png";
            GameObject gameObject = new GameObject(name);
            EnemyPrinter item = gameObject.AddComponent<EnemyPrinter>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "testinator";
            string longDesc = "prints guids of enemies in the console\n\ngo die.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.EXCLUDED;
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
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                ETGModConsole.Log(arg2.aiActor.EnemyGuid);
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
            DebrisObject debrisObject = base.Drop(player);
            try
            {
                player.PostProcessBeam -= this.PostProcessBeam;
                player.PostProcessProjectile -= this.PostProcessProjectile;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log($"damn,\n {ex}");
            }
            debrisObject.GetComponent<EnemyPrinter>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect
        {
            HealthMultiplier = 0.7f,
            CooldownMultiplier = 0.5f,
            OverheadVFX = overheadder,
            duration = 10,
        };

        public static UnityEngine.GameObject overheadder = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
    }
}
