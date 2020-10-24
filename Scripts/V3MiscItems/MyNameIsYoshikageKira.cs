using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace katmod
{
    class MyNameIsYoshikageKira : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Deadly Charm";
            string resourceName = "katmod/Resources/V3MiscItems/bitez";
            GameObject obj = new GameObject();
            MyNameIsYoshikageKira item = obj.AddComponent<MyNameIsYoshikageKira>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Kablewy";
            string longDesc = "Blows up 3 enemies upon taking damage";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(817);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += onDam;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= onDam;
            return base.Drop(player);
        }

        void onDam(PlayerController player)
        {
            List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
            if (activeEnemies != null && player.CurrentRoom != null && player.healthHaver && player.IsInCombat)
            {
                for (int i = 0; i < 3; i++)
                {
                    int RandomEnemy = UnityEngine.Random.Range(0, activeEnemies.Count);
                    if (activeEnemies[RandomEnemy] != null && activeEnemies[RandomEnemy].specRigidbody)
                    {
                        Exploder.DoDefaultExplosion(activeEnemies[RandomEnemy].specRigidbody.UnitCenter, default(Vector2));
                    }
                }
            }
        }
    }
}
