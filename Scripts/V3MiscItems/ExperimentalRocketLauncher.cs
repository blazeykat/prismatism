using ItemAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class ExperimentalRocketLauncher : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Experimental Rocket Launcher";
            string resourceName = "katmod/Resources/V3MiscItems/experimentalrocketlauncher.png";
            GameObject obj = new GameObject();
            ExperimentalRocketLauncher item = obj.AddComponent<ExperimentalRocketLauncher>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Big Boom";
            string longDesc = "Shoots a remote controlled missile upon use.\n\nAn attempt to make a gun which requires no ammo, whatsoever. It mostly works, except for the overheating, and the occassional detionation.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            ItemBuilder.SetCooldownType(item, ItemBuilder.CooldownType.Damage, 1000);
            InitProjectile();
        }

        static Projectile rocketProjectile;

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            StartCoroutine(FireRockets(user));
        }

        IEnumerator FireRockets(PlayerController user)
        {
            bool SynergyCheck = false; //user.PlayerHasActiveSynergy("Control Group");
            for (int i = 0; i < (SynergyCheck ? 6 : 3); i++)
            {
                if (user != null)
                {
                    GameObject missileObject = SpawnManager.SpawnProjectile(ExperimentalRocketLauncher.rocketProjectile.gameObject, user.specRigidbody.UnitCenter, Quaternion.Euler(0, 0, (user.CurrentGun == null) ? 0f : user.CurrentGun.CurrentAngle - 90 + i * (SynergyCheck ? 45 : 90)));
                    Projectile projectile = missileObject.gameObject.GetComponent<Projectile>();
                    if (projectile != null)
                    {
                        projectile.Owner = user;
                        projectile.Shooter = user.specRigidbody;
                        user.DoPostProcessProjectile(projectile);
                    }
                    AkSoundEngine.PostEvent("Play_WPN_yarirocketlauncher_shot_01", gameObject);
                    yield return new WaitForSeconds(0.33f);
                }
            }
        }

        public static void InitProjectile()
        {
            Projectile missile = (PickupObjectDatabase.GetById(372) as Gun).DefaultModule.projectiles[0];
            Projectile initMissile = UnityEngine.Object.Instantiate(missile);

            List<string> spriteIds = new List<string>
            {
                "experimentalmissile_001",
                "experimentalmissile_002",
                "experimentalmissile_003",
                "experimentalmissile_004",
                "experimentalmissile_005",
                "experimentalmissile_006",
                "experimentalmissile_007",
                "experimentalmissile_008",
            };
            List<IntVector2> intVectors = new List<IntVector2>
            {
                new IntVector2(12, 6),
                new IntVector2(12, 6),
                new IntVector2(12, 6),
                new IntVector2(12, 6),
                new IntVector2(12, 6),
                new IntVector2(12, 6),
                new IntVector2(12, 6),
                new IntVector2(12, 6),
            };


            {
                initMissile.AnimateProjectile(spriteIds, 18, true, intVectors, dontEvenKnowWhatThisIs, anchors, changesColliders, fixesScale, vector2dotZero, offsets, offsets, WhenSpAPIInevitablySeesThisAndCallsItBadCodeLetItBeKnownIHadAbsolutelyNoIdeaOnWhatToPutHere);
                initMissile.gameObject.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(initMissile.gameObject);
                FakePrefab.MarkAsFakePrefab(initMissile.gameObject);
            }
            rocketProjectile = initMissile;
        }

        static readonly List<Projectile> WhenSpAPIInevitablySeesThisAndCallsItBadCodeLetItBeKnownIHadAbsolutelyNoIdeaOnWhatToPutHere = new List<Projectile>
            {
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
            };

        static readonly List<IntVector2?> offsets = new List<IntVector2?>
            {
                IntVector2.Zero,
                IntVector2.Zero,
                IntVector2.Zero,
                IntVector2.Zero,
                IntVector2.Zero,
                IntVector2.Zero,
                IntVector2.Zero,
                IntVector2.Zero,
            };

        static List<Vector3?> vector2dotZero = new List<Vector3?>
            {
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero
            };

        static List<bool> fixesScale = new List<bool>
            {
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
            };

        static List<bool> dontEvenKnowWhatThisIs = new List<bool>
            {
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
            };
        static List<tk2dBaseSprite.Anchor> anchors = new List<tk2dBaseSprite.Anchor>
            {
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter
            };
        static List<bool> changesColliders = new List<bool>
            {
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
            };
    }
}
