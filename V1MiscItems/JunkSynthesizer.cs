using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;


namespace katmod
{
    class JunkSynthesizer : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Junk Synthesizer";
            string resourceName = "katmod/Resources/junksynthesizer";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<JunkSynthesizer>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Junk time";
            string longDesc = "Enemies have a chance to drop a junk.\n\nThis mechanism consumes the life force of your enemies in order to spawn literal garbage.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.EXCLUDED;
            item.RemovePickupFromLootTables();
            //item.SetupUnlockOnStat(TrackedStats.WOODEN_CHESTS_BROKEN, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 14);
            //item.PlaceItemInAmmonomiconAfterItemById(641);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null && enemy != null)
            {
                if (fatal)
                {
                    if (BoxOTools.BasicRandom(0.97f))
                    {
                        if (BoxOTools.BasicRandom(0.99f) && !base.Owner.HasPickupID(580) && !base.Owner.HasPickupID(641))
                        {
                            if (BoxOTools.BasicRandom(0.05f) && !base.Owner.HasPickupID(580))
                            {
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(580).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                            }
                            else if (!base.Owner.HasPickupID(641))
                            {
                                LootEngine.SpawnItem(PickupObjectDatabase.GetById(641).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                            }
                        }
                        else
                        {
                            LootEngine.SpawnItem(PickupObjectDatabase.GetById(127).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                        }
                    }
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));

            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnAnyEnemyReceivedDamage -= OnEnemyDamaged;
            }
            base.OnDestroy();
        }

    }

}
