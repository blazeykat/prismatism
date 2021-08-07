using Dungeonator;
using ItemAPI;
using System;
using System.Collections;
using UnityEngine;

namespace katmod
{
    class ElectricRounds : PassiveItem
    {
        public static int ID;
        public static void Init()
        {
            string name = "Electric Shells";
            string resourcePath = "katmod/Resources/V2MiscItems/electricshells.png";
            GameObject gameObject = new GameObject(name);
            ElectricRounds item = gameObject.AddComponent<ElectricRounds>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Mind Electric";
            string longDesc = "Creates chain lightning on hit.\n\nThy genius sates a thirst for trouble.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(298);
            ElectricRounds.ID = item.PickupObjectId;
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
                sourceProjectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(Projectile projectile, SpeculativeRigidbody enemy, bool fatal)
        {
            ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
            float chainGlitchPreventinator = complexProjectileModifier.ChainLightningMaxLinkDistance;
            if (Owner && m_owner.CurrentRoom != null && m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null)
            {
                foreach (AIActor aiactor in this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
                {
                    if (Vector2.Distance(aiactor.CenterPosition, enemy.UnitCenter) < chainGlitchPreventinator)
                    {
                        GameObject gameObject = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(38) as Gun).DefaultModule.projectiles[0].gameObject, aiactor.sprite.WorldCenter, Quaternion.identity, true);
                        Projectile component = gameObject.GetComponent<Projectile>();
                        bool flag4 = component != null;
                        if (flag4)
                        {
                            component.sprite.renderer.enabled = false;
                            component.specRigidbody.CollideWithOthers = false;
                            component.specRigidbody.CollideWithTileMap = false;
                            component.baseData.damage = 0f;
                            component.baseData.range = float.MaxValue;
                            component.baseData.speed = 0f;
                            component.Owner = this.m_owner;
                            component.Shooter = this.m_owner.specRigidbody;
                            ChainLightningModifier orAddComponent = gameObject.gameObject.GetOrAddComponent<ChainLightningModifier>();
                            orAddComponent.LinkVFXPrefab = complexProjectileModifier.ChainLightningVFX;
                            orAddComponent.damageTypes = complexProjectileModifier.ChainLightningDamageTypes;
                            orAddComponent.maximumLinkDistance = complexProjectileModifier.ChainLightningMaxLinkDistance;
                            orAddComponent.damagePerHit = m_owner.HasMTGConsoleID("psm:thunder_shells") ? 3 : complexProjectileModifier.ChainLightningDamagePerHit;
                            orAddComponent.damageCooldown = 0.1f;
                            StartCoroutine(DelProjectel(component));
                            bool flag5 = complexProjectileModifier.ChainLightningDispersalParticles != null;
                            if (flag5)
                            {
                                orAddComponent.UsesDispersalParticles = true;
                                orAddComponent.DispersalParticleSystemPrefab = complexProjectileModifier.ChainLightningDispersalParticles;
                                orAddComponent.DispersalDensity = complexProjectileModifier.ChainLightningDispersalDensity;
                                orAddComponent.DispersalMinCoherency = complexProjectileModifier.ChainLightningDispersalMinCoherence;
                                orAddComponent.DispersalMaxCoherency = complexProjectileModifier.ChainLightningDispersalMaxCoherence;
                            }
                            else
                            {
                                orAddComponent.UsesDispersalParticles = false;
                            }

                            chainGlitchPreventinator += Vector2.Distance(aiactor.CenterPosition, enemy.UnitCenter);
                        }
                    }
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
            player.PostProcessBeam -= this.PostProcessBeam;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }

        public class ZappedEnemyBehaviour : BraveBehaviour
        {
        }

        private IEnumerator DelProjectel(Projectile project)
        {
            yield return null;
            project.ForceDestruction();
            yield break;
        }
    }
}
