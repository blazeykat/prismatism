using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace katmod
{
    class LemonPig : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Lemon Pig";
            string resourceName = "katmod/Resources/V3MiscItems/lemonpig";
            GameObject obj = new GameObject();
            LemonPig item = obj.AddComponent<LemonPig>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Omen of Death";
            string longDesc = "Increases money dropped by enemies the less HP you have.\n\nFlesh Rots\nMetal Rusts\nBone Breaks";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 2f);
            item.PlaceItemInAmmonomiconAfterItemById(451);
            item.SetupUnlockOnCustomMaximum(CustomTrackedMaximums.MAXIMUM_HEALTH, 4.99f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Have more than 5 heart containers in one run");

            Hook hook = new Hook(typeof(CompanionController).GetMethod("HandleRoomCleared", BindingFlags.NonPublic | BindingFlags.Instance), typeof(LemonPig).GetMethod("PigHook"));
            InitProjectile();
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += Player_PostProcessProjectile;
        }

        private void Player_PostProcessProjectile(Projectile arg1, float arg2)
        {
            if (Owner && coinProjectile && coinProjectile.gameObject && BoxOTools.BasicRandom(0.9f) && Owner.PlayerHasActiveSynergy("Keeper of the Coin"))
            {
                GameObject coinObject = SpawnManager.SpawnProjectile(LemonPig.coinProjectile.gameObject, Owner.CurrentGun.barrelOffset.position, Quaternion.Euler(0, 0, (Owner.CurrentGun == null) ? 0f : Owner.CurrentGun.CurrentAngle));
                Projectile projectile = coinObject.gameObject.GetComponent<Projectile>();
                if (projectile)
                {
                    projectile.Owner = Owner;
                    projectile.Shooter = Owner.specRigidbody;
                    projectile.gameObject.AddComponent(new HomingModifier() { HomingRadius = 720, AngularVelocity = 180 });
                }
                AkSoundEngine.PostEvent("Play_OBJ_ironcoin_wish_01", gameObject);
            }
        }

        protected override void Update()
        {
            base.Update();
            if (Owner && Owner.healthHaver)
            {
                bool HasSynergy = Owner.PlayerHasActiveSynergy("Orange Pig");
                bool RobotSTOP = Owner.characterIdentity == PlayableCharacters.Robot || Owner.healthHaver.GetMaxHealth() <= 0;
                float health = RobotSTOP ? 1 : Owner.healthHaver.GetCurrentHealthPercentage();
                if (health != lastHealth || HasSynergy != lastHasSynergy)
                {
                    lastHasSynergy = HasSynergy;
                    lastHealth = health;
                    this.RemoveStat(PlayerStats.StatType.MoneyMultiplierFromEnemies);
                    this.RemoveStat(PlayerStats.StatType.GlobalPriceMultiplier);
                    if (HasSynergy) { this.AddStat(PlayerStats.StatType.GlobalPriceMultiplier, -.25f);}
                    this.AddStat(PlayerStats.StatType.MoneyMultiplierFromEnemies, (1 - health) / 1.333333333f);
                    Owner.stats.RecalculateStats(Owner);
                }
            }
        }

        private static bool lastHasSynergy;

        private static float lastHealth;

        public static void PigHook(Action<CompanionController, PlayerController> orig, CompanionController companion, PlayerController owner)
        {
            orig(companion, owner);
            if (companion && companion.specRigidbody && companion.name.StartsWith("Pig") && owner.PlayerHasActiveSynergy("Oink"))
            {
                if (BoxOTools.BasicRandom(0.4f))
                {
                    LootEngine.SpawnCurrency(companion.specRigidbody.UnitCenter, 2);
                }
            }
        }

        private static Projectile coinProjectile;

        public static void InitProjectile()
        {
            Projectile coin = (PickupObjectDatabase.GetById(15) as Gun).DefaultModule.projectiles[0];
            Projectile initCoin = UnityEngine.Object.Instantiate(coin);

            List<string> spriteIds = new List<string>
            {
                "bronzecoin_001",
                "bronzecoin_002",
                "bronzecoin_003",
                "bronzecoin_004",
                "bronzecoin_005",
                "bronzecoin_006",
                "bronzecoin_007",
                "bronzecoin_008",
                "bronzecoin_009",
                "bronzecoin_010",
                "bronzecoin_011",
                "bronzecoin_012",
            };
            List<IntVector2> intVectors = new List<IntVector2>
            {
                new IntVector2(15, 15),
                new IntVector2(13, 15),
                new IntVector2(8, 15),
                new IntVector2(2, 15),
                new IntVector2(8, 15),
                new IntVector2(13, 15),
                new IntVector2(15, 15),
                new IntVector2(13, 15),
                new IntVector2(8, 15),
                new IntVector2(2, 15),
                new IntVector2(8, 15),
                new IntVector2(13, 15),
            };
            {
                initCoin.AnimateProjectile(spriteIds, 24, true, intVectors, dontEvenKnowWhatThisIs, anchors, changesColliders, fixesScale, vector2dotZero, offsets, offsets, uh);
                initCoin.shouldRotate = true;
                initCoin.baseData.damage = 15;
                initCoin.baseData.speed = 12;
                /*initCoin.gameObject.SetActive(false);
                UnityEngine.Object.DontDestroyOnLoad(initCoin.gameObject);
                FakePrefab.MarkAsFakePrefab(initCoin.gameObject);*/
            }
            coinProjectile = initCoin;
            coinProjectile.gameObject.SetActive(false);
            UnityEngine.Object.DontDestroyOnLoad(coinProjectile.gameObject);
            FakePrefab.MarkAsFakePrefab(coinProjectile.gameObject);
        }

        static readonly List<Projectile> uh = new List<Projectile>
            {
                null,
                null,
                null,
                null,
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
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
            };

        static readonly List<Vector3?> vector2dotZero = new List<Vector3?>
            {
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
            };

        static readonly List<bool> fixesScale = new List<bool>
            {
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
            };

        static readonly List<bool> dontEvenKnowWhatThisIs = new List<bool>
            {
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
            };
        static readonly List<tk2dBaseSprite.Anchor> anchors = new List<tk2dBaseSprite.Anchor>
            {
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
                tk2dBaseSprite.Anchor.MiddleCenter,
            };
        static readonly List<bool> changesColliders = new List<bool>
            {
                true,
                true,
                true,
                true,
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
