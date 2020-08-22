using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections;

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
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
			ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
			float chainGlitchPreventinator = complexProjectileModifier.ChainLightningMaxLinkDistance;
			bool flag = this.m_pickedUp && this.m_owner != null;
			if (flag)
			{
				bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
				if (flag2)
				{
					foreach (AIActor aiactor in this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
					{
						if (Vector2.Distance(aiactor.CenterPosition, arg2.UnitCenter) < chainGlitchPreventinator) 
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
								orAddComponent.damagePerHit = m_owner.HasPassiveItem(ThunderRounds.ID) ? 3 : complexProjectileModifier.ChainLightningDamagePerHit;
								orAddComponent.damageCooldown = 0.1f;
								StartCoroutine(DelProjectel(0.1f, component));
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
								chainGlitchPreventinator += complexProjectileModifier.ChainLightningMaxLinkDistance;
							}
						}
					}
				}
				/*bool flag6 = this.zapperMainProjectile == null;
				if (flag6)
				{
					GameObject gameObject2 = SpawnManager.SpawnProjectile((PickupObjectDatabase.GetById(38) as Gun).DefaultModule.projectiles[0].gameObject, this.m_owner.sprite.WorldCenter, Quaternion.identity, true);
					Projectile component2 = gameObject2.GetComponent<Projectile>();
					bool flag7 = component2 != null;
					if (flag7)
					{
						component2.sprite.renderer.enabled = false;
						component2.specRigidbody.CollideWithOthers = false;
						component2.specRigidbody.CollideWithTileMap = false;
						component2.baseData.damage = 0f;
						component2.baseData.range = float.MaxValue;
						component2.baseData.speed = 0f;
						component2.Owner = this.m_owner;
						component2.Shooter = this.m_owner.specRigidbody;
						ComplexProjectileModifier complexProjectileModifier2 = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
						ChainLightningModifier orAddComponent2 = gameObject2.gameObject.GetOrAddComponent<ChainLightningModifier>();
						orAddComponent2.LinkVFXPrefab = complexProjectileModifier2.ChainLightningVFX;
						orAddComponent2.damageTypes = complexProjectileModifier2.ChainLightningDamageTypes;
						orAddComponent2.maximumLinkDistance = complexProjectileModifier2.ChainLightningMaxLinkDistance;
						orAddComponent2.damagePerHit = complexProjectileModifier2.ChainLightningDamagePerHit;
						orAddComponent2.damageCooldown = complexProjectileModifier2.ChainLightningDamageCooldown;
						StartCoroutine(DelProjectel(orAddComponent2.damageCooldown, component2));
						bool flag8 = complexProjectileModifier2.ChainLightningDispersalParticles != null;
						if (flag8)
						{
							orAddComponent2.UsesDispersalParticles = true;
							orAddComponent2.DispersalParticleSystemPrefab = complexProjectileModifier2.ChainLightningDispersalParticles;
							orAddComponent2.DispersalDensity = complexProjectileModifier2.ChainLightningDispersalDensity;
							orAddComponent2.DispersalMinCoherency = complexProjectileModifier2.ChainLightningDispersalMinCoherence;
							orAddComponent2.DispersalMaxCoherency = complexProjectileModifier2.ChainLightningDispersalMaxCoherence;
						}
						else
						{
							orAddComponent2.UsesDispersalParticles = false;
						}
					}
					this.zapperMainProjectile = component2;
				}*/
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
            debrisObject.GetComponent<ElectricRounds>().m_pickedUpThisRun = true;
            return debrisObject;
        }

		private Projectile zapperMainProjectile = null;

		public class ZappedEnemyBehaviour : BraveBehaviour
		{
		}

		private IEnumerator DelProjectel(float time, Projectile project)
        {
			yield return new WaitForSeconds(time);
			project.ForceDestruction();
			yield break;
        }
	}
}
