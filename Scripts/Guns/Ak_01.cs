using System;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Items
{
	internal class Ak_01 : global::GunBehaviour
	{
		public static void Add()
		{
			Gun gun = global::ETGMod.Databases.Items.NewGun("Ak-01", "ak01");
			Gungeon.Game.Items.Rename("outdated_gun_mods:ak-01", "psm:ak-01");
			gun.gameObject.AddComponent<Ak_01>();
			Gun gun3 = PickupObjectDatabase.GetById(598) as Gun;
			gun.SetShortDescription("Very old gun");
			gun.SetLongDescription("The very first AK47, made with rocks and by rubbing sticks together.");
			gun.SetupSprite(null, "ak-01_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 4);
			gun.SetAnimationFPS(gun.reloadAnimation, 2);
			gun.AddProjectileModuleFrom("ak-47", true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = .6f;
			gun.DefaultModule.angleVariance = 0f;
			gun.DefaultModule.cooldownTime = 0.1f;
			gun.DefaultModule.numberOfShotsInClip = 20;
			Gun gun2 = PickupObjectDatabase.GetById(151) as Gun;
			gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
			gun.SetBaseMaxAmmo(300);
			gun.quality = PickupObject.ItemQuality.A;
			gun.sprite.IsPerpendicular = true;
			gun.gunClass = GunClass.FULLAUTO;
			global::ETGMod.Databases.Items.Add(gun, null, "ANY");
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			projectile.baseData.damage = 7;
			gun.DefaultModule.projectiles[0] = projectile;
			gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
		}
		public override void OnPostFired(PlayerController player, Gun gun)
		{
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_zorgun_shot_01", base.gameObject);
		}
    }
}
