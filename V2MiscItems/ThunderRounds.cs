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
	class ThunderRounds : PassiveItem
	{
		public static void Init()
		{
			string name = "Thunder Shells";
			string resourcePath = "katmod/Resources/V2MiscItems/thundershells.png";
			GameObject gameObject = new GameObject(name);
			ThunderRounds item = gameObject.AddComponent<ThunderRounds>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Tesla";
			string longDesc = "Creates chain lightning from the bullets position.\n\nThese bullets have been charged with electricity, and now they're zapping everything.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(298);
		}
		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			try
			{
				sourceProjectile.OnDestruction += this.Removinator;
				ActiveProjectiles.Add(sourceProjectile);

				ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
				chainGlitchPreventinator = complexProjectileModifier.ChainLightningMaxLinkDistance;
			}
			catch (Exception ex)
			{
				global::ETGModConsole.Log(ex.Message, false);
			}
		}

		private void Removinator (Projectile projectile)
        {
			ActiveProjectiles.Remove(projectile);
        }
		protected override void Update()
		{
			try
			{
				base.Update();
				if (this.m_pickedUp && this.m_owner != null)
				{
					if (ActiveProjectiles.Count != 0 && ActiveProjectiles[0] && ActiveProjectiles != null)
					{
						bool flag2 = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
						if (flag2)
						{
							foreach (AIActor aiactor in this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
							{
								if (aiactor.GetComponent<ThunderRounds.ZappedEnemyBehaviour>() == null) 
								{
									if (Vector2.Distance(aiactor.CenterPosition, (this.zapperMainProjectile == null) ? ActiveProjectiles[0].transform.position : this.zapperMainProjectile.transform.position) < chainGlitchPreventinator)
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
											component.gameObject.AddComponent<ThunderRounds.ZapperProjectileBehaviour>().manuallyAssignedEnemy = aiactor;
											ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
											ChainLightningModifier orAddComponent = gameObject.gameObject.GetOrAddComponent<ChainLightningModifier>();
											orAddComponent.LinkVFXPrefab = complexProjectileModifier.ChainLightningVFX;
											orAddComponent.damageTypes = complexProjectileModifier.ChainLightningDamageTypes;
											orAddComponent.maximumLinkDistance = complexProjectileModifier.ChainLightningMaxLinkDistance;
											orAddComponent.damagePerHit = m_owner.HasPassiveItem(ElectricRounds.ID) || m_owner.HasGun(13) ? 3 : complexProjectileModifier.ChainLightningDamagePerHit;
											orAddComponent.damageCooldown = complexProjectileModifier.ChainLightningDamageCooldown;
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

											chainGlitchPreventinator += Vector2.Distance(aiactor.CenterPosition, (this.zapperMainProjectile == null) ? ActiveProjectiles[0].transform.position : this.zapperMainProjectile.transform.position);
										}
										LightningProjectiles.Add(component);
										aiactor.gameObject.AddComponent<ThunderRounds.ZappedEnemyBehaviour>();
									}
								}
							}
						}
						bool flag6 = this.zapperMainProjectile == null;
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
								component2.gameObject.AddComponent<ThunderRounds.ZapperMainProjectileBehaviour>().manuallyAssignedPlayer = this.ActiveProjectiles[0];
								ComplexProjectileModifier complexProjectileModifier2 = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
								ChainLightningModifier orAddComponent2 = gameObject2.gameObject.GetOrAddComponent<ChainLightningModifier>();
								orAddComponent2.LinkVFXPrefab = complexProjectileModifier2.ChainLightningVFX;
								orAddComponent2.damageTypes = complexProjectileModifier2.ChainLightningDamageTypes;
								orAddComponent2.maximumLinkDistance = complexProjectileModifier2.ChainLightningMaxLinkDistance;
								orAddComponent2.damagePerHit = m_owner.HasPassiveItem(ElectricRounds.ID) || m_owner.HasGun(13) ? 3 : complexProjectileModifier2.ChainLightningDamagePerHit;
								orAddComponent2.damageCooldown = complexProjectileModifier2.ChainLightningDamageCooldown;
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
						}
					}
					if (ActiveProjectiles.Count == 0)
					{
						foreach (Projectile projectile in LightningProjectiles)
						{
							if (projectile)
							{
								projectile.DieInAir();
							}
						}

						bool safetyflag = this.m_owner.CurrentRoom != null && this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All) != null;
						if (safetyflag)
						{
							foreach (AIActor aiactor in this.m_owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
							{
								Destroy(aiactor.gameObject.GetComponent<ThunderRounds.ZappedEnemyBehaviour>());
							}
						}
					}
				}
			} catch (Exception Exc)
            {
				ETGModConsole.Log($"{Exc}");
            }
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			try
			{
				player.PostProcessProjectile -= this.PostProcessProjectile;
			}
			catch (Exception ex)
			{
				ETGModConsole.Log($"damn,\n {ex}");
			}
			debrisObject.GetComponent<ThunderRounds>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		private Projectile zapperMainProjectile = null;

		public class ZappedEnemyBehaviour : BraveBehaviour
		{
		}


		public List<Projectile> ActiveProjectiles = new List<Projectile>();

		private readonly List<Projectile> LightningProjectiles = new List<Projectile>();

		public class ZapperProjectileBehaviour : BraveBehaviour
		{
			private void Update()
			{
				bool flag = this.manuallyAssignedEnemy == null || (this.manuallyAssignedEnemy.healthHaver != null && this.manuallyAssignedEnemy.healthHaver.IsDead);
				if (flag)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					base.transform.position = this.manuallyAssignedEnemy.sprite.WorldCenter.ToVector3ZisY(0f);
					base.specRigidbody.Reinitialize();
				}
			}

			public AIActor manuallyAssignedEnemy;
		}

		public float chainGlitchPreventinator;
		public class ZapperMainProjectileBehaviour : BraveBehaviour
		{
			private void Update()
			{
				bool flag = this.manuallyAssignedPlayer == null;
				if (flag)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					base.transform.position = this.manuallyAssignedPlayer.sprite.WorldCenter.ToVector3ZisY(0f);
					base.specRigidbody.Reinitialize();
				}
			}

			public Projectile manuallyAssignedPlayer;
		}
	}
}
