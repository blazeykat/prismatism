using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class BlueExplosive : SwarmPickup
    {
        public static void Init()
        {
            string name = "bluesplode";
            string resourcePath = "katmod/Resources/Swarm/bluesplode.png";
            GameObject gameObject = new GameObject(name);
            BlueExplosive item = gameObject.AddComponent<BlueExplosive>();
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
                    Exploder.DoDefaultExplosion(activeEnemies[RandomEnemy].specRigidbody.UnitCenter, default(Vector2), damageTypes:CoreDamageTypes.Ice);
                }
            }
        }
    }
}
