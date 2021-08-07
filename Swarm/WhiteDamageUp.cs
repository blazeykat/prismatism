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
    class WhiteDamageUp : SwarmPickup
    {
        public static void Init()
        {
            string name = "whiteup";
            string resourcePath = "katmod/Resources/Swarm/blackup.png";
            GameObject gameObject = new GameObject(name);
            WhiteDamageUp item = gameObject.AddComponent<WhiteDamageUp>();
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
            ETGModConsole.Log("epic enumerator works");
            yield return new WaitForSeconds(8);
            player.ownerlessStatModifiers.Remove(modifier);
            player.stats.RecalculateStats(player);
            yield break;
        }

        private StatModifier modifier = new StatModifier
        {
            amount = 1.5f,
            statToBoost = PlayerStats.StatType.Damage,
            modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE
        };

    }
}
