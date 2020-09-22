using System;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	public class RedAndWhite : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = global::ETGMod.Databases.Items.NewGun("Red and White", "redandwhite");
			Gungeon.Game.Items.Rename("outdated_gun_mods:red_and_white", "psm:red_and_white");
			gun.gameObject.AddComponent<RedAndWhite>();
			gun.SetShortDescription("True Piercing");
			gun.SetLongDescription("Can pierce walls and enemies.\n\nThe gun is the color of a ghoul!!!");
			gun.SetupSprite(null, "redandwhite_idle_001", 1);
			gun.SetAnimationFPS(gun.shootAnimation, 24);
			Gun gun3 = PickupObjectDatabase.GetById(38) as Gun;
			gun.AddProjectileModuleFrom("klobb", true, false);
			gun.SetBaseMaxAmmo(210);
			gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.damageModifier = 1;
			gun.reloadTime = 0.45f;
			gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0f);
			gun.DefaultModule.cooldownTime = 0.09f;
			gun.DefaultModule.numberOfShotsInClip = 5;
			gun.DefaultModule.angleVariance = 10f;
			gun.quality = PickupObject.ItemQuality.A;
			gun.encounterTrackable.EncounterGuid = "redandwhite";
			gun.gunClass = GunClass.FULLAUTO;
			gun.CanBeDropped = true;
			gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
			global::ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			PierceProjModifier pp = projectile.gameObject.AddComponent<PierceProjModifier>();
			pp.penetratesBreakables = true;
			pp.penetration = 2;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 13f;
			projectile.AdditionalScaleMultiplier *= 1.1f;
			projectile.PenetratesInternalWalls = true;
			gun.DefaultModule.projectiles[0] = projectile;
			gun.PlaceItemInAmmonomiconAfterItemById(14);
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
				AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
		}
		private bool HasReloaded;
	}
}
