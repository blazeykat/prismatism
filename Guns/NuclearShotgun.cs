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
            Gun gun = ETGMod.Databases.Items.NewGun("Wastelander's Shotgun", "retroshotgun");
            Gungeon.Game.Items.Rename("outdated_gun_mods:wastelander's_shotgun", "psm:wastelanders_shotgun");
            gun.gameObject.AddComponent<NuclearShotgun>();
            gun.SetShortDescription("The Struggle Continues");
            gun.SetLongDescription("Enemies have a chance to drop unique boxes of ammo, which refill only this gun.");
            gun.SetupSprite(null, "retroshotgun_idle_001", 16);
            Gun gun3 = PickupObjectDatabase.GetById(51) as Gun;
            for (int i = 0; i < 5; i++)
            {
                gun.AddProjectileModuleFrom(gun3, true, false);
            }
            foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
            {
                Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
                projectile.gameObject.SetActive(false);
                FakePrefab.MarkAsFakePrefab(projectile.gameObject);
                DontDestroyOnLoad(projectile);
                projectile.transform.parent = gun.barrelOffset;
                projectile.baseData.damage = 7;
                projectile.AdditionalScaleMultiplier *= 1.2f;
                projectileModule.projectiles.Add(projectile);
                projectileModule.cooldownTime = 1f;
                projectileModule.numberOfShotsInClip = 7;
                projectileModule.angleVariance = 12f;
                projectileModule.customAmmoType = gun3.DefaultModule.customAmmoType;
                projectileModule.ammoType = gun3.DefaultModule.ammoType;
                projectileModule.ammoCost = 0;
                projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            }
            gun.InfiniteAmmo = false;
            gun.damageModifier = 1;
            gun.reloadTime = 1.5f;
            gun.SetBaseMaxAmmo(55);
            gun.quality = PickupObject.ItemQuality.C;
            gun.gunClass = GunClass.SHOTGUN;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;
            gun.CanGainAmmo = false;
            ETGMod.Databases.Items.Add(gun);
            gun.PlaceItemInAmmonomiconAfterItemById(51);
            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.NUCLEARSHOTGUN_META_FLAG, true);
            Prismatism.Unlocks.Add(gun.PickupObjectId, "Buy it");
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

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            bool flag = gun.IsReloading && this.HasReloaded;
            if (flag)
            {
                this.HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_shotgun_reload", base.gameObject);
            }
        }

        public override void OnPostFired(PlayerController player, Gun gun)
        {
            this.gun.ammo = this.gun.CurrentOwner.gameObject.GetComponent<NuclearPlayerController>().Shells -= 1;
            AkSoundEngine.PostEvent("Play_WPN_shotgun_shot_01", base.gameObject);
        }
        private bool HasReloaded;

        public override void OnDropped()
        {
            base.OnDropped();
        }
    }


}
