using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
namespace katmod
{
    class LuckyHorseshoe : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Lucky Horseshoe";
            string resourceName = "katmod/Resources/V2MiscItems/horseshoe";
            GameObject obj = new GameObject();
            LuckyHorseshoe item = obj.AddComponent<LuckyHorseshoe>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "All the same.";
            string longDesc = "Enemies have a chance to drop either a heart or a red ammo box.\n\nThe shoe's on the other foot";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(159);
        }

        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (fatal && enemy != null && enemy.specRigidbody != null )
            {
                if (BoxOTools.BasicRandom(0.98f))
                {
                    System.Random random = new System.Random();
                    
                    if (random.Next(1, 3) == 1)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(73).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
                    } else
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(600).gameObject, enemy.specRigidbody.UnitCenter, Vector2.zero, 1f, false, true, false);
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

    }

}
