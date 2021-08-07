using System;
using Gungeon;
using ItemAPI;
using UnityEngine;
using static ProjectileModule;

namespace katmod
{
	public class Shotstool : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = global::ETGMod.Databases.Items.NewGun("Shotstool", "shotstool");
			Gungeon.Game.Items.Rename("outdated_gun_mods:shotstool", "psm:shotstool");
			gun.gameObject.AddComponent<Shotstool>();
			gun.SetShortDescription("Mush Gun");
			gun.SetLongDescription("A hybrid of a toadstool and a shotgun.\n\nNot to be confused with a ShotStol, a hybrid of a pistol and a shotgun.");
			gun.SetupSprite(null, "monarchshotgun_idle_001", 1);
			Gun gun3 = PickupObjectDatabase.GetById(51) as Gun;
			for (int i = 0; i < 4; i++)
			{
				gun.AddProjectileModuleFrom("klobb", true, false);
			}
			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				DontDestroyOnLoad(projectile);
				projectile.transform.parent = gun.barrelOffset;
				projectile.baseData.damage = 7;
				projectile.AdditionalScaleMultiplier *= 1.2f;
				projectileModule.projectiles.Add(projectile);
				projectileModule.cooldownTime = 1f;
				projectileModule.numberOfShotsInClip = 1;
				projectileModule.angleVariance = 8f;
				projectileModule.customAmmoType = gun3.DefaultModule.customAmmoType;
				projectileModule.ammoType = gun3.DefaultModule.ammoType;
				if (projectileModule != gun.DefaultModule)
				{
					projectileModule.ammoCost = 0;
				}
				projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			}
			gun.InfiniteAmmo = false;
			gun.damageModifier = 1;
			gun.reloadTime = 1.5f;
			gun.SetBaseMaxAmmo(150);
			gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0f);
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.encounterTrackable.EncounterGuid = "shotstool";
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
			AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", base.gameObject);
			projectile.gameObject.AddComponent<ParasiteProjectile>().thingy = gun.CurrentOwner as PlayerController;
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
		public class HitEnemyBehaviour : BraveBehaviour
		{
		}

	}
}
