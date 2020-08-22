using System;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	public class BeeGun : global::GunBehaviour
	{
		public static void Add()
		{
			Gun gun = global::ETGMod.Databases.Items.NewGun("Bee Gun", "beegun");
			Gungeon.Game.Items.Rename("outdated_gun_mods:bee_gun", "psm:bee_gun");
			gun.gameObject.AddComponent<BeeGun>();
			gun.SetShortDescription("Control the Swarm");
			gun.SetLongDescription("Weaponised bees were first used outside of the Gungeon, but this version has been enhanced by the Gungeon's raw gun power.\n\nExamples of weaponised bees also include beenades, bee swords, bee bows and bee armor.");
			gun.SetupSprite(null, "beegun_idle_001", 1);
			Gun gun3 = PickupObjectDatabase.GetById(14) as Gun;
			gun.AddProjectileModuleFrom("klobb", true, false);
			gun.SetBaseMaxAmmo(250);
			gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
			gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.damageModifier = 1;
			gun.reloadTime = 2f;
			gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0f);
			gun.DefaultModule.cooldownTime = 0.06f;
			gun.DefaultModule.numberOfShotsInClip = 100;
			gun.DefaultModule.angleVariance = 10f;
			gun.quality = PickupObject.ItemQuality.B;
			gun.encounterTrackable.EncounterGuid = "beegun";
			gun.gunClass = GunClass.FULLAUTO;
			gun.CanBeDropped = true;
			Gun gun4 = PickupObjectDatabase.GetById(339) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			global::ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.damage = 3.2f;
			projectile.AdditionalScaleMultiplier *= 1.1f;
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
				AkSoundEngine.PostEvent("Play_WPN_dartgun_reload_01", base.gameObject);
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			AkSoundEngine.PostEvent("Play_WPN_beehive_shot_01", base.gameObject);
		}
		private bool HasReloaded;
	}
}
