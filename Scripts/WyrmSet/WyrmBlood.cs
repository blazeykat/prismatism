using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace katmod
{
    class WyrmBlood : PassiveItem
    {
        public static void Init()
        {
            string name = "Wyrm Poison";
            string resourcePath = "katmod/Resources/WyrmSet/poison2";
            GameObject gameObject = new GameObject(name);
            WyrmBlood item = gameObject.AddComponent<WyrmBlood>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Higher beings";
            string longDesc = "Heavily decreases enemy health on hit.\n\nMany researchers come to the Gungeon, with the goal of manipulating the Gungeon's bizarre concept of time. However, after the wyrm was created, all we knew went missing with it's creator.\n\nA vial of this was found in the top floor of the breach.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.S;
            item.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_FORGE, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 15);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Clear the forge 15 times");
            item.PlaceItemInAmmonomiconAfterItemById(167);
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
            if (arg2 != null && arg2.aiActor && arg2.healthHaver != null && Owner != null)
            {
                arg2.aiActor.ApplyEffect(EnemyDebuff, 1f, null);
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
            debrisObject.GetComponent<WyrmBlood>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect
        {
            HealthMultiplier = 0.72f,
            OverheadVFX = overheadder,
            duration = 10,
        };

        public GameActorWitherEffect wither = new GameActorWitherEffect
        {
            duration = 20,
            TintColor = Color.black,
            AppliesTint = true
        };

        public static UnityEngine.GameObject overheadder = ResourceCache.Acquire("Global VFX/VFX_Debuff_Status") as GameObject;
    }
}
