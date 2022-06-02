using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class SweeperGun : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Sweeper", "sweepergun");
            Gungeon.Game.Items.Rename("outdated_gun_mods:sweeper", "psm:sweeper");
            gun.gameObject.AddComponent<SweeperGun>();
            gun.SetShortDescription("MineSwept");
            gun.SetLongDescription("Lays mines.\n\nTraditionally used to lay mines quickly and efficiently, but has been discarded in favor of more fashionable newer versions.");
            gun.SetupSprite(null, "sweepergun_idle_001", 1);
            gun.SetAnimationFPS(gun.shootAnimation, 16);
            Gun gun2 = PickupObjectDatabase.GetById(129) as Gun;
            Gun gun3 = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom("magnum", true, false);
            gun.SetBaseMaxAmmo(100);
            gun.DefaultModule.customAmmoType = gun2.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = gun2.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.reloadTime = 1.5f;
            gun.DefaultModule.cooldownTime = 0.5f;
            gun.DefaultModule.numberOfShotsInClip = 3;
            gun.DefaultModule.angleVariance = 10f;
            gun.quality = PickupObject.ItemQuality.C;
            gun.gunClass = GunClass.PISTOL;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            projectile.baseData.damage = 25f;
            projectile.baseData.range = 25f;
            projectile.baseData.speed = 7f;
            projectile.SetProjectileSpriteRight("sweeperprojectile_001", 19, 19);
            projectile.transform.parent = gun.barrelOffset;
            gun.DefaultModule.projectiles[0] = projectile;

            gun.PreventNormalFireAudio = true;

            gun.PlaceItemInAmmonomiconAfterItemById(129);
            gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.SWEEPERGUN_META_FLAG, true);
            BreachShopTool.AddBaseMetaShopTier(Gungeon.Game.Items["psm:red_and_white"].PickupObjectId, 24, Gungeon.Game.Items["psm:wastelanders_shotgun"].PickupObjectId, 40, gun.PickupObjectId, 18);
            Prismatism.Unlocks.Add(gun.PickupObjectId, "Buy it");
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
            base.PostProcessProjectile(projectile);
            SlowdownProjectile slow = projectile.gameObject.AddComponent<SlowdownProjectile>();
            slow.m_speed = projectile.baseData.speed / 30;
            projectile.gameObject.AddComponent<ExplodeNearEnemiesProjectile>();
        }

        public void GiantWithAnAppetite(Projectile projectile)
        {
            try
            {
                Vector3 pos = projectile.sprite.WorldCenter;
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

        public class ExplodeNearEnemiesProjectile : MonoBehaviour
        {
            protected void Start()
            {
                projectile = base.GetComponent<Projectile>();
                player = (PlayerController)projectile.Owner;
            }

            protected void Update()
            {
                if (player && player.CurrentRoom != null && player.CurrentRoom.GetNearestEnemy(projectile.sprite.WorldCenter, out float distance))
                {
                    if (distance < 3f)
                    {
                        Exploder.DoDefaultExplosion(projectile.sprite.WorldCenter, Vector2.zero);
                        if (player && player.PlayerHasActiveSynergy(":retroswept:"))
                        {
                            FlakTime(projectile, player);
                        }
                        Destroy(gameObject);
                    }
                }
            }

            Projectile projectile;

            PlayerController player;
        }

        public static void FlakTime(Projectile projectile, PlayerController playerController)
        {
            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[38]).DefaultModule.projectiles[0];
            for (float f = 0; f < UnityEngine.Random.Range(2f, 12f); f++)
            {
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 359)), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component)
                {
                    component.SpawnedFromOtherPlayerProjectile = true;
                    component.Owner = playerController;
                    component.Shooter = playerController.specRigidbody;
                    component.baseData.speed = 11.5f;
                    component.baseData.damage *= 0.9f;
                    component.AdditionalScaleMultiplier = 0.8f;
                    component.baseData.range *= 0.5f;
                    BounceProjModifier bounceProjModifier = component.gameObject.AddComponent<BounceProjModifier>();
                    bounceProjModifier.numberOfBounces = 2;
                }
            }
            for (float f = 0; f < UnityEngine.Random.Range(4f, 10f); f++)
            {
                GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 359)), true);
                Projectile component = gameObject2.GetComponent<Projectile>();
                if (component != null)
                {
                    component.SpawnedFromOtherPlayerProjectile = true;
                    component.Owner = playerController;
                    component.Shooter = playerController.specRigidbody;
                    component.baseData.speed = 14.5f;
                    component.baseData.damage *= 1.25f;
                    component.AdditionalScaleMultiplier = 1.1f;
                    component.baseData.range *= 0.25f;
                    BounceProjModifier bounceProjModifier2 = component.gameObject.AddComponent<BounceProjModifier>();
                    bounceProjModifier2.numberOfBounces = 2;
                    PierceProjModifier pierceProjModifier = projectile.gameObject.AddComponent<PierceProjModifier>();
                    pierceProjModifier.penetration = 1;
                }
            }
            for (float f = 0; f < UnityEngine.Random.Range(6f, 8f); f++)
            {
                GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile2.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 359)), true);
                Projectile component = gameObject3.GetComponent<Projectile>();
                if (component)
                {
                    component.SpawnedFromOtherPlayerProjectile = true;
                    component.Owner = playerController;
                    component.Shooter = playerController.specRigidbody;
                    component.baseData.speed = 17f;
                    component.baseData.damage *= 1.6f;
                    component.AdditionalScaleMultiplier = 1.65f;
                    component.baseData.range *= 0.25f;
                    BounceProjModifier bounceProjModifier3 = component.gameObject.AddComponent<BounceProjModifier>();
                    bounceProjModifier3.numberOfBounces = 2;
                    PierceProjModifier pierceProjModifier2 = projectile.gameObject.AddComponent<PierceProjModifier>();
                    pierceProjModifier2.penetration = 1;
                }
            }
        }
    }
}
