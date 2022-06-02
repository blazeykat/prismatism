using Dungeonator;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class YVSwarmPickup : SwarmPickup
    {
        public static void Init()
        {
            string name = "yvswarm";
            string resourcePath = "katmod/Resources/Swarm/music.png";
            GameObject gameObject = new GameObject(name);
            YVSwarmPickup item = gameObject.AddComponent<YVSwarmPickup>();
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
            player.ownerlessStatModifiers.Add(modifier);
            player.stats.RecalculateStats(player);
            ETGMod.StartGlobalCoroutine(Enumerator(player));
        }

        private IEnumerator Enumerator(PlayerController player)
        {
            yield return new WaitForSeconds(8);
            player.ownerlessStatModifiers.Remove(modifier);
            player.stats.RecalculateStats(player);
            yield break;
        }

        private readonly StatModifier modifier = new StatModifier
        {
            amount = 20f,
            statToBoost = PlayerStats.StatType.ExtremeShadowBulletChance,
            modifyType = StatModifier.ModifyMethod.ADDITIVE
        };
        
    }
}
