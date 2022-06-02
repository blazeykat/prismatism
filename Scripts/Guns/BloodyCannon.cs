using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
using static ProjectileModule;

namespace katmod
{
	public class BloodyCannon : global::GunBehaviour
	{
		public static void Add()
		{
			Gun gun = global::ETGMod.Databases.Items.NewGun("Bloody Cannon", "bloodycannon");
			Gungeon.Game.Items.Rename("outdated_gun_mods:bloody_cannon", "psm:bloody_cannon");
			gun.gameObject.AddComponent<BloodyCannon>();
			gun.SetShortDescription("Heart Breaking");
			gun.SetLongDescription("Uses blood to obliterate your enemies.\n\nPreviously a hammer, now a gun.");
			gun.SetupSprite(null, "beegun_idle_001", 1);
			Gun gun3 = PickupObjectDatabase.GetById(480) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(393) as Gun;
			for (int i = 0; i < 4; i++)
			{
				gun.AddProjectileModuleFrom("klobb", true, false);
			}
			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				ChargeProjectile projectile = new ChargeProjectile();
				projectile.Projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.chargeProjectiles[0].Projectile);
				projectile.Projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.Projectile.gameObject);
                DontDestroyOnLoad(projectile.Projectile);
				projectile.Projectile.transform.parent = gun.barrelOffset;
				projectile.Projectile.AdditionalScaleMultiplier *= 1.1f;
				projectile.ChargeTime = 2.5f;
				projectile.Projectile.gameObject.AddComponent(new BounceProjModifier());
				projectile.Projectile.DefaultTintColor = Color.red;
				projectile.Projectile.HasDefaultTint = true;
				projectileModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile> { projectile };
				projectileModule.cooldownTime = 2f;
				projectileModule.numberOfShotsInClip = 1;
				projectileModule.angleVariance = 10f;
				projectileModule.customAmmoType = gun3.DefaultModule.customAmmoType;
				projectileModule.ammoType = gun3.DefaultModule.ammoType;
				projectileModule.ammoCost = 1;
				projectileModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			}
			gun.DefaultModule.chargeProjectiles.Remove(gun4.DefaultModule.chargeProjectiles[0]);
			gun.InfiniteAmmo = true;
			gun.damageModifier = 1;
			gun.reloadTime = 2.1f;
			gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0f);
			gun.quality = PickupObject.ItemQuality.S;
			gun.encounterTrackable.EncounterGuid = "bloodycannon";
			gun.gunClass = GunClass.SHOTGUN;
			gun.CanBeDropped = true;
			gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
			global::ETGMod.Databases.Items.Add(gun, null, "ANY");
			
		}
		protected void Update()
		{
			PlayerController playerController = this.gun.CurrentOwner as PlayerController;
			bool flag = playerController && playerController != null;
			if (flag)
			{
				bool flag2 = !this.gun.PreventNormalFireAudio;
				if (flag2)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag3)
				{
					this.HasReloaded = true;
				}
			}
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController x = this.gun.CurrentOwner as PlayerController;
			bool flag = x == null;
			bool flag2 = flag;
			if (flag2)
			{
				this.gun.ammo = this.gun.GetBaseMaxAmmo();
			}
			this.gun.DefaultModule.ammoCost = 1;
			if (x.IsInCombat)
			{
				x.healthHaver.ApplyDamage(1.5f, Vector2.zero, "Blood Sacrifice", global::CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
			}
			AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", base.gameObject);
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_dartgun_reload_01", base.gameObject);
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			AkSoundEngine.PostEvent("Play_WPN_seriouscannon_charge_01", base.gameObject);
		}
		private bool HasReloaded;
	}
}
