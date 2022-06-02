using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace katmod
{
    class StoneSword : PassiveItem
    {
        public static void Init()
        {
            string name = "Stone Sword";
            string resourcePath = "katmod/Resources/DecaySet/stonesword.png";
            GameObject gameObject = new GameObject(name);
            StoneSword item = gameObject.AddComponent<StoneSword>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "A Terrible Fortress";
            string longDesc = "Makes your enemies wither away upon damage.\n\nA relic of wither skeletons, inhabitors of a fiery fortress.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(167);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                Projectile projectile = sourceBeam.projectile;
                projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
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
                sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.OnHitEnemy));
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                if (BoxOTools.BasicRandom(.85f))
                {
                    GameActorWitherEffect witherEffect = wither;
                    if (Owner.PlayerHasActiveSynergy("Withered away"))
                    {
                        wither.duration *= 2;
                    }
                    arg2.aiActor.ApplyEffect(witherEffect, 1f, null);
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
            DebrisObject debrisObject = base.Drop(player);
            try
            {
                player.PostProcessBeam -= this.PostProcessBeam;
                player.PostProcessProjectile -= this.PostProcessProjectile;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log($"damn,\n {ex}");
            }
            debrisObject.GetComponent<StoneSword>().m_pickedUpThisRun = true;
            return debrisObject;
        }

        public GameActorWitherEffect wither = new GameActorWitherEffect
        {
            duration = 1,
            DamagePerSecondToEnemies = 15,
            TintColor = Color.black,
            AppliesTint = true
        };
    }
}
