using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class RedExplosive : SwarmPickup
    {
        public static void Init()
        {
            string name = "redsplode";
            string resourcePath = "katmod/Resources/Swarm/redsplode.png";
            GameObject gameObject = new GameObject(name);
            RedExplosive item = gameObject.AddComponent<RedExplosive>();
            SpeculativeRigidbody specRig = gameObject.AddComponent<SpeculativeRigidbody>();
            PixelCollider collide = new PixelCollider
            {
                IsTrigger = true,
                ManualWidth = 19,
                ManualHeight = 22,
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.PlayerBlocker,
                ManualOffsetX = 0,
                ManualOffsetY = 0
            };
            specRig.PixelColliders = new List<PixelCollider>
            {
                collide
            };
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "mayo";
            string longDesc = "naise";
            item.SetupItem(shortDesc, longDesc);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear);
            if (activeEnemies != null && player.CurrentRoom != null)
            {
                int RandomEnemy = UnityEngine.Random.Range(0, activeEnemies.Count);
                if (activeEnemies[RandomEnemy] != null && activeEnemies[RandomEnemy].specRigidbody)
                {
                    Exploder.DoDefaultExplosion(activeEnemies[RandomEnemy].specRigidbody.UnitCenter, default(Vector2), damageTypes:CoreDamageTypes.Fire);
                }
            }
        }
    }
}
