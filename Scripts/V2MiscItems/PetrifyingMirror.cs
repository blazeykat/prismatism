using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace katmod
{
    class PetrifyingMirror : PassiveItem
    {
        public static void Init()
        {
            string name = "Petrifying Mirror";
            string resourcePath = "katmod/Resources/V2MiscItems/petrifymirror.png";
            GameObject gameObject = new GameObject(name);
            PetrifyingMirror item = gameObject.AddComponent<PetrifyingMirror>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Ugly";
            string longDesc = "Heavily slows enemies upon damaging them.\n\nImbued with the magic of the Gorgun. Also is not a mask, definitely not a mask, and was never a mask.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            List<string> mandatoryConsoleIDs = new List<string>
            {
                "psm:petrifying_mirror",
                "glass_cannon"
            };
            CustomSynergies.Add("Mirror Cannon", mandatoryConsoleIDs, null, true);
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_MEDUZI, true);
            item.PlaceItemInAmmonomiconAfterItemById(102);
        }

        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                Projectile projectile = sourceBeam.projectile;
                projectile.OnHitEnemy += OnEnemyDamaged;
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
                sourceProjectile.OnHitEnemy += OnEnemyDamaged;
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnEnemyDamaged(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (base.Owner.HasGun(540))
            {
                arg2.aiActor.gameActor.ApplyEffect(EnemyDebuff, 1f, null);
            } else if (arg2.aiActor != arg2.healthHaver.IsBoss)
            {
                if (Utilities.BasicRandom(base.Owner, 0.7f, 50f))
                {
                    arg2.aiActor.gameActor.ApplyEffect(EnemyDebuff, 1f, null);
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
            DebrisObject debrisObject = base.Drop(player);
            try
            {
                player.PostProcessProjectile -= this.PostProcessProjectile;
                player.PostProcessBeam -= this.PostProcessBeam;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log($"damn,\n {ex}");
            }
            debrisObject.GetComponent<PetrifyingMirror>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect
        {
            SpeedMultiplier = 0.1f,
            KeepHealthPercentage = true,
            AppliesTint = true,
            TintColor = UnityEngine.Color.gray,
            duration = 5,
        };
    }
}
