using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    public class RoyalShotgun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Royal Shotgun", "monarchshotgun");
            Gungeon.Game.Items.Rename("outdated_gun_mods:royal_shotgun", "psm:royal_shotgun");
            gun.gameObject.AddComponent<RoyalShotgun>();
            gun.SetShortDescription("Reach the Throne");
            gun.SetLongDescription("Made from the bricks of a nuclear palace.");
            gun.SetupSprite(null, "monarchshotgun_idle_001", 1);
            gun.SetAnimationFPS(gun.shootAnimation, 12);
            Gun gun3 = PickupObjectDatabase.GetById(38) as Gun;
            Gun gun4 = PickupObjectDatabase.GetById(337) as Gun;
            gun.AddProjectileModuleFrom("klobb", true, false);
            gun.SetBaseMaxAmmo(20);
            gun.DefaultModule.customAmmoType = gun4.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = gun4.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.09f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.DefaultModule.angleVariance = 10f;
            gun.quality = PickupObject.ItemQuality.S;
            gun.gunClass = GunClass.SHOTGUN;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
            ETGMod.Databases.Items.Add(gun);

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 30f;
            projectile.baseData.range = 50f;
            projectile.baseData.speed = 7f;
            projectile.SetProjectileSpriteRight("thronebulletbig_001", 32, 32);

            gun.PlaceItemInAmmonomiconAfterItemById(480);
            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.ROYALSHOTGUN_GOOPTON_FLAG, true);
            Prismatism.Unlocks.Add(gun.PickupObjectId, "Buy it");
            gun.AddItemToGooptonMetaShop(80);
            gun.DefaultModule.projectiles[0] = projectile;

            gun.PreventNormalFireAudio = true;

            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[38]).DefaultModule.projectiles[0];
            Projectile proj1 = UnityEngine.Object.Instantiate<Projectile>(projectile2);
            {
                proj1.baseData.damage = 10;
                proj1.SetProjectileSpriteRight("thronebulletmedium_001", 16, 16);
                proj1.gameObject.SetActive(false);
                proj1.baseData.speed = 6;
                UnityEngine.Object.DontDestroyOnLoad(proj1);
                FakePrefab.MarkAsFakePrefab(proj1.gameObject);
            }
            mediumBullet = proj1;
        }

        protected void Update()
        {
            if (this.gun.CurrentOwner != null)
            {
                bool flag3 = !this.gun.IsReloading && !this.HasReloaded;
                if (flag3)
                {
                    this.HasReloaded = true;
                }
            }
        }

        public override void PostProcessProjectile(Projectile projectile)
        {
            try
            {
                base.PostProcessProjectile(projectile);
                SlowdownProjectile slow = projectile.gameObject.AddComponent<SlowdownProjectile>();
                slow.DieAfterStop = true;
                slow.m_speed = projectile.baseData.speed / 15;
                projectile.OnDestruction += this.GiantWithAnAppetite;
            }
            catch (Exception error)
            {
                ETGModConsole.Log($"{error}");
            }

        }

        public void GiantWithAnAppetite(Projectile projectile)
        {
            try
            {
                ETGModConsole.Log("nice fail 2");
                Vector3 pos = projectile.sprite.WorldCenter;
                StartCoroutine(FrozenToTheCore(pos));
            }
            catch (Exception error)
            {
                ETGModConsole.Log($"{error}");
            }
        }

        public IEnumerator FrozenToTheCore(Vector3 position)
        {
            if (gun.CurrentOwner && gun.CurrentOwner is PlayerController player)
                for (int counter = 0; counter < 3; counter++)
                {
                    for (int counter2 = 0; counter2 < 8; counter2++)
                    {
                        GameObject gameObject = SpawnManager.SpawnProjectile(RoyalShotgun.mediumBullet.gameObject, position, Quaternion.Euler(0f, 0f, (45 * counter2)), true);
                        Projectile component = gameObject.GetComponent<Projectile>();
                        if (component != null)
                        {
                            component.Owner = player;
                            component.baseData.speed += 2 * counter;
                            PierceProjModifier pp = component.gameObject.AddComponent<PierceProjModifier>();
                            pp.penetration = 2;
                            player.DoPostProcessProjectile(component);
                        }
                    }
                }
            else
                ETGModConsole.Log("nice fail");
            yield break;
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

        private static Projectile mediumBullet;
    }
}
