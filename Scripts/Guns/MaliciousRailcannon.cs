using Gungeon;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class MaliciousRailcannon : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Malicious Railcannon", "maliciousrailcannon");
            Gungeon.Game.Items.Rename("outdated_gun_mods:malicious_railcannon", "psm:malicious_railcannon");
            gun.gameObject.AddComponent<MaliciousRailcannon>();
            gun.SetShortDescription("EXPLODED");
            gun.SetLongDescription("Shoots a hitscan beam, which spawns an explosion wherever it hits. Use at your own risk.\n\ndevilmayquake.com");
            gun.SetupSprite(null, "maliciousrailcannon_idle_001", 1);
            gun.SetAnimationFPS(gun.reloadAnimation, 1);

            Gun gun3 = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom("klobb", true, false);
            gun.SetBaseMaxAmmo(210);

            gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.angleVariance = 0;

            gun.damageModifier = 1;
            gun.reloadTime = 3f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.quality = PickupObject.ItemQuality.S;
            gun.gunClass = GunClass.RIFLE;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Projectile projectile = Instantiate<Projectile>(gun3.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);


            GameObject linkVFXPrefab = FakePrefab.Clone(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX);

            projectile.transform.parent = gun.barrelOffset;
            projectile.sprite.renderer.enabled = false;
            projectile.baseData.speed = 12;
            projectile.hitEffects = null;
            projectile.DestroyMode = Projectile.ProjectileDestroyMode.None;
            projectile.specRigidbody.CollideWithOthers = false;
            projectile.specRigidbody.CollideWithTileMap = false;
            MaliciousBeamHandler beamHandler = projectile.gameObject.AddComponent<MaliciousBeamHandler>();
            beamHandler.linkVFXPrefab = linkVFXPrefab;
            beamHandler.wait = 0.25f;
            projectile.baseData.damage = 0f;
            gun.DefaultModule.projectiles[0] = projectile;
        }

        private class MaliciousBeamHandler : HitscanHelper
        {
            public override void OnBeamHit(Vector2 contact, Projectile projectile)
            {
                if (projectile)
                {
                    ExplosionData explosion = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultExplosionData;
                    explosion.damage = 130; explosion.doDestroyProjectiles = true;

                    Exploder.Explode(contact, explosion, Vector2.zero, damageTypes: projectile.damageTypes);
                }
            }

            public override void PostBeamRender(tk2dTiledSprite beam)
            {
                Material material = beam.GetComponent<Renderer>().material;
                material.SetFloat("_BlackBullet", 0.995f);
                material.SetFloat("_EmissiveColorPower", 4.9f);
            }
        }
    }
}
