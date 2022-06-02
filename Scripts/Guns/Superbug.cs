using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Superbug : GunBehaviour
    {
        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Superbug", "superbug");
            Gungeon.Game.Items.Rename("outdated_gun_mods:superbug", "psm:superbug");
            gun.gameObject.AddComponent<Superbug>();
            gun.SetShortDescription("In my lungs");
            gun.SetLongDescription("Never, ever, ever, stops, and never, ever, gives a fuck");
            gun.SetupSprite(null, "maliciousrailcannon_idle_001", 1);

            Gun gun3 = PickupObjectDatabase.GetById(38) as Gun;
            gun.AddProjectileModuleFrom("klobb", true, false);
            gun.SetBaseMaxAmmo(75);

            gun.DefaultModule.customAmmoType = gun3.DefaultModule.customAmmoType;
            gun.DefaultModule.ammoType = gun3.DefaultModule.ammoType;
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.angleVariance = 0;

            gun.damageModifier = 1;
            gun.reloadTime = 3f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = 1;
            gun.quality = PickupObject.ItemQuality.B;
            gun.gunClass = GunClass.RIFLE;
            gun.CanBeDropped = true;
            gun.muzzleFlashEffects = gun3.muzzleFlashEffects;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

            Projectile projectile = Instantiate(gun3.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);

            projectile.transform.parent = gun.barrelOffset;
            projectile.baseData.speed = 18;
            projectile.baseData.damage = 8f;
            gun.DefaultModule.projectiles[0] = projectile;

            projectile.gameObject.AddComponent<SuperbugProjectile>();
        }


        public class SuperbugProjectile : MonoBehaviour
        {
            private Projectile projectile;

            void Start()
            {
                projectile = gameObject.GetComponent<Projectile>();
                projectile.OnHitEnemy += OnHit;
            }

            public void OnHit(Projectile proj, SpeculativeRigidbody target, bool flag)
            {
                if (proj && proj.specRigidbody)
                {
                    target?.aiActor?.ParentRoom?.BetterDoToEnemiesInRadius(proj.specRigidbody.UnitCenter, 6, delegate (AIActor enemy)
                    {
                        if (BoxOTools.BasicRandom(0.5f))
                        {
                            GameActorHealthEffect poisonEffect = Gungeon.Game.Items["irradiated_lead"].GetComponent<BulletStatusEffectItem>().HealthModifierEffect;
                            enemy.ApplyEffect(poisonEffect);
                        }
                    });
                }
            }
        }
    }
}
