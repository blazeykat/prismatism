using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using MultiplayerBasicExample;
using UnityEngine;
using XInputDotNetPure;

namespace katmod
{
    public class NuclearShotgun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Retro Shotgun", "retroshotgun");
            Gungeon.Game.Items.Rename("outdated_gun_mods:retro_shotgun", "psm:retro_shotgun");
            gun.gameObject.AddComponent<NuclearShotgun>();
            gun.SetShortDescription("The Struggle Continues");
            gun.SetLongDescription("Enemies have a chance to drop");
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
                projectileModule.cooldownTime = 0.25f;
                projectileModule.numberOfShotsInClip = 7;
                projectileModule.angleVariance = 8f;
                projectileModule.customAmmoType = gun3.DefaultModule.customAmmoType;
                projectileModule.ammoType = gun3.DefaultModule.ammoType;
                projectileModule.ammoCost = 0;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            }
            gun.InfiniteAmmo = false;
            gun.damageModifier = 1;
            gun.reloadTime = 0.75f;
            gun.SetBaseMaxAmmo(55);
            gun.barrelOffset.transform.localPosition += new Vector3(0.2f, -0.1f, 0f);
            gun.quality = PickupObject.ItemQuality.EXCLUDED;
            gun.encounterTrackable.EncounterGuid = "retroshotgun";
            gun.gunClass = GunClass.SHOTGUN;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
            global::ETGMod.Databases.Items.Add(gun, null, "ANY");
        }
        public static Projectile AfterProjectile;

        protected void Update()
        {
            if (this.gun.CurrentOwner != null)
            {
                bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
                if (flag3)
                {
                    this.HasReloaded = true;
                }
                NuclearPlayerController nuclear = this.gun.CurrentOwner.gameObject.GetComponent<NuclearPlayerController>();
                if (nuclear)
                {
                    if (this.gun.ammo != nuclear.Shells)
                    {
                        this.gun.ammo = nuclear.Shells;
                    }
                } else
                {
                    this.gun.CurrentOwner.gameObject.AddComponent<NuclearPlayerController>();
                }
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            try
            {
                PlayerController x = this.gun.CurrentOwner as PlayerController;
                bool flag = x == null;
                if (flag)
                {
                    this.gun.ammo = this.gun.GetBaseMaxAmmo();
                }
                this.gun.DefaultModule.ammoCost = 1;
                ThisIsAReallyJankWorkaround++;
                if (ThisIsAReallyJankWorkaround == 4)
                {
                    this.gun.ammo = this.gun.CurrentOwner.gameObject.GetComponent<NuclearPlayerController>().Shells -= 1;
                    ThisIsAReallyJankWorkaround = 0;
                }
            }
            catch (Exception error)
            {
                ETGModConsole.Log($"{error}");
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
                AkSoundEngine.PostEvent("Play_WPN_magnum_reload_01", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_seriouscannon_shot_01", base.gameObject);
        }
        private bool HasReloaded;

        public int ThisIsAReallyJankWorkaround = 0;

        public override void OnDropped()
        {
            base.OnDropped();
        }
    }


}
