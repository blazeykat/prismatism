﻿using ItemAPI;
using UnityEngine;

namespace katmod
{
    public class RescueRanger : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Rescue Rifle", "rescuerifle");
            Gungeon.Game.Items.Rename("outdated_gun_mods:rescue_rifle", "psm:rescue_rifle");
            gun.gameObject.AddComponent<RescueRanger>();
            gun.SetShortDescription("Guess that'll do.");
            gun.SetLongDescription("Shoots a bolt.\n\nA makeshift construction tool with some combat potential.");
            gun.SetupSprite(null, "rescuerifle_idle_001");
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            Gun gun3 = PickupObjectDatabase.GetById(12) as Gun;
            Gun gun4 = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom("klobb", true, false);
            gun.SetBaseMaxAmmo(128);
            gun.DefaultModule.customAmmoType = gun4.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = gun4.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.damageModifier = 1;
            gun.reloadTime = 1.25f;
            gun.DefaultModule.cooldownTime = 0.75f;
            gun.DefaultModule.numberOfShotsInClip = 4;
            gun.DefaultModule.angleVariance = 0f;
            gun.quality = PickupObject.ItemQuality.C;
            gun.encounterTrackable.EncounterGuid = "rescuerifle";
            gun.gunClass = GunClass.FULLAUTO;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.damage = 17;
            projectile.AdditionalScaleMultiplier *= 1.1f;
            projectile.SetProjectileSpriteRight("rescueprojectile_001", 8, 6);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            gun.PlaceItemInAmmonomiconAfterItemById(54);
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            base.PostProcessProjectile(projectile);
            if (((PlayerController)gun.CurrentOwner).PlayerHasActiveSynergy("Sentry Goin' Up"))
            {
                projectile.gameObject.AddComponent(new HomingModifier() { HomingRadius = 720, AngularVelocity = 360 });
            }
        }

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            bool flag = gun.IsReloading && this.HasReloaded;
            if (flag)
            {
                this.HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_crossbow_reload_01", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_crossbowshotgun_shot_01", base.gameObject);
        }
        private bool HasReloaded;
    }
}
