﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class ExecutionShells : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Execution Shells";
            string resourceName = "katmod/Resources/V2MiscItems/executionshells";
            GameObject obj = new GameObject();
            ExecutionShells item = obj.AddComponent<ExecutionShells>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "First Strike IV";
            string longDesc = "Doubles damage against enemies with max HP.\n\nUsed by the Executioners of the gungeon, to kill traitorous bullets.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_GUNGEON, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 75);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Clear the Gungeon Proper 75 times");
            item.PlaceItemInAmmonomiconAfterItemById(323);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.specRigidbody.OnPreRigidbodyCollision += OnHitEnemy;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.aiActor != null && myRigidbody != null && myRigidbody.projectile && otherRigidbody.aiActor.healthHaver)
            {
                float hpercent = otherRigidbody.aiActor.healthHaver.GetCurrentHealthPercentage();
                if (hpercent == 1f)
                {
                    myRigidbody.projectile.baseData.damage *= 2;
                }
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
