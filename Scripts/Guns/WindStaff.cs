using System;
using Gungeon;
using ItemAPI;
using UnityEngine;
using static ProjectileModule;

namespace katmod
{
	public class WindStaff : global::GunBehaviour
	{
		public static void Add()
		{
			Gun gun = global::ETGMod.Databases.Items.NewGun("Tornado Staff", "tornadostaff");
			Gungeon.Game.Items.Rename("outdated_gun_mods:tornado_staff", "psm:tornado_staff");
			gun.gameObject.AddComponent<WindStaff>();
			gun.SetShortDescription("Smartinator");
			gun.SetLongDescription("Gotta wonder who stuck a tome of infinite wisdom on a stick...");
			gun.SetupSprite(null, "tornadostaff_idle_001", 1);
			gun.SetAnimationFPS(gun.shootAnimation, 8);
			Gun gun3 = PickupObjectDatabase.GetById(520) as Gun;
			Gun gun4 = PickupObjectDatabase.GetById(574) as Gun;
			for (int i = 0; i < 3; i++)
			{
				gun.AddProjectileModuleFrom("wonderboy", true, true);
			}
			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				Projectile projectile = gun3.DefaultModule.projectiles[0];
				projectileModule.projectiles.Add(projectile);
				projectileModule.cooldownTime = 0.5f;
				projectileModule.numberOfShotsInClip = 20;
				projectileModule.angleVariance = 10f;
				projectileModule.customAmmoType = gun3.DefaultModule.customAmmoType;
				projectileModule.ammoType = gun3.DefaultModule.ammoType;
				projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				bool flag = projectileModule != gun.DefaultModule;
				if (flag)
				{
					projectileModule.ammoCost = 0;
				}
				if (projectileModule == gun4.DefaultModule)
                {
					projectileModule.GetCurrentProjectile().baseData.damage = 20f;
				}
			}
			gun.InfiniteAmmo = false;
			gun.SetBaseMaxAmmo(100);
			gun.damageModifier = 1;
			gun.reloadTime = 1.2f;
			gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0f);
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.encounterTrackable.EncounterGuid = "tornadostaff";
			gun.gunClass = GunClass.NONE;
			gun.CanBeDropped = true;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
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
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_blasphemy_reload_01", base.gameObject);
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
		}
		private bool HasReloaded;
	}
}
