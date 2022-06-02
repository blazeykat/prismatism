using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Jeremy : ComplexProjectileModifier
    {
        public static void Init()
        {
            string itemName = "Jeremy the Blobulon";
            string resourceName = "katmod/Resources/johnblobbeththethird.png";
            GameObject obj = new GameObject();
            Jeremy item = obj.AddComponent<Jeremy>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Hungry boy";
            string longDesc = "Spits out a skull upon killing an enemy.\n\nJeremy consumes the corpse of his enemies, digesting everything but the skull. Jeremy doesn't like skulls.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.C;
            item.SetupUnlockOnStat(TrackedStats.TIMES_CLEARED_SEWERS, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 3);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Clear the Sewers three times.");
            item.PlaceItemInAmmonomiconAfterItemById(301);
        }
        private void OnEnemyKilled(PlayerController player, HealthHaver enemy)
        {
            if (enemy != null && enemy.specRigidbody != null)
            {
                player.HandleProjectile(15f, 7f, 45, false, Vector2.zero, true);
            }
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += this.OnEnemyKilled;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemyContext -= this.OnEnemyKilled;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner)
            { 
                Owner.OnKilledEnemyContext -= OnEnemyKilled;
            }
            base.OnDestroy();
        }
    }
}
