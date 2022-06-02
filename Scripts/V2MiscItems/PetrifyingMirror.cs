﻿using ItemAPI;
using System;
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
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_MEDUZI, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Kill the Gorgun");
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
            if (arg2 && arg2.healthHaver && arg2.aiActor)
            {
                if ((Owner.PlayerHasActiveSynergy("Mirror Cannon") || BoxOTools.BasicRandom(0.9f)) && !arg2.healthHaver.IsBoss)
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
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeam -= this.PostProcessBeam;
            return base.Drop(player);
        }

        public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect
        {
            SpeedMultiplier = 0.25f,
            KeepHealthPercentage = true,
            AppliesTint = true,
            TintColor = UnityEngine.Color.gray,
            duration = 10,
        };
    }
}
