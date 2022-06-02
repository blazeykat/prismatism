using ItemAPI;
using UnityEngine;


namespace katmod
{
    class VultureFeather : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Magic Vulture Feather";
            string resourceName = "katmod/Resources/vulturefeather";
            GameObject obj = new GameObject();
            VultureFeather item = obj.AddComponent<VultureFeather>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "God Approaches";
            string longDesc = "On killing a cursed enemy, has a chance to drop an item. Chance increases with curse.\n\nWhile vulture feathers are naturally gray, these ones are purple. It's not for a magic reason, they're just dyed that way.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.S;
            item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Master the forge's boss");
            item.PlaceItemInAmmonomiconAfterItemById(307);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
            item.AddPassiveStatModifier(PlayerStats.StatType.Curse, 3f);
            //BuildPrefab();
        }

        public static void BuildPrefab()
        {
            bool flag = PurpleGuonStone.orbitalPrefab != null;
            if (!flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("katmod/Resources/V2MiscItems/purpleguonstoneorbital.png", null);
                gameObject.name = "vultureFeatherSynergyOrbital";

                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
                orbitalPrefab.shouldRotate = true;
                orbitalPrefab.orbitRadius = 2.5f;
                orbitalPrefab.SetOrbitalTier(0);

                //we'll put the animation stuff here later

                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
            }
        }

        // the old "I don't lnow how to balance my items" version
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null)
            {
                if (fatal)
                {
                    if (BoxOTools.BasicRandom(.985f) && !GameStatsManager.Instance.IsRainbowRun)
                    {
                        GameManager.Instance.RewardManager.SpawnTotallyRandomItem(enemy.specRigidbody.UnitCenter);
                    }
                }
            }
        }

        public static PlayerOrbital orbitalPrefab;

        /* public GameObject thingToDestroy;

         public static bool SynergyExists;

         /*protected override void Update()
         {
             base.Update();
             bool NewSynergyCheck = Owner.PlayerHasActiveSynergy("Lucky Bird");
             if (NewSynergyCheck != SynergyExists)
             {
                 SynergyExists = NewSynergyCheck;
                 if (SynergyExists)
                 {
                     ETGModConsole.Log("ar");
                     thingToDestroy = PlayerOrbitalItem.CreateOrbital(Owner, orbitalPrefab.gameObject, true);
                 }
                 else if (thingToDestroy)
                 {
                     Destroy(thingToDestroy);
                 }
             }
         }*/

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            //player.OnRoomClearEvent += ItemGiver;
            player.OnKilledEnemyContext += Player_OnKilledEnemyContext;
        }

        private void Player_OnKilledEnemyContext(PlayerController arg1, HealthHaver arg2)
        {
            float curse = arg1.stats.GetStatValue(PlayerStats.StatType.Curse);
            if (BoxOTools.BasicRandom(.87f - 0.0025f * curse) && arg2 && arg2.aiActor && arg2.aiActor.IsBlackPhantom)
            {
                if (Owner.PlayerHasActiveSynergy("Lucky Bird"))
                {
                    //This synergy fucking sucks you gotta remove it
                    GameManager.Instance.RewardManager.SpawnTotallyRandomItem(arg2.specRigidbody.UnitCenter, ItemQuality.A, ItemQuality.S);
                }
                else
                {
                    GameManager.Instance.RewardManager.SpawnTotallyRandomItem(arg2.specRigidbody.UnitCenter);
                }
                LootEngine.DoDefaultItemPoof(arg2.specRigidbody.UnitCenter);
            }
        }

        private void ItemGiver(PlayerController player)
        {
            if (BoxOTools.BasicRandom(.91f) && player && player.specRigidbody && !player.CurrentRoom.PlayerHasTakenDamageInThisRoom)
            {
                if (Owner.PlayerHasActiveSynergy("Lucky Bird"))
                {
                    GameManager.Instance.RewardManager.SpawnTotallyRandomItem(player.specRigidbody.UnitCenter, ItemQuality.A, ItemQuality.S);
                }
                else
                {
                    GameManager.Instance.RewardManager.SpawnTotallyRandomItem(player.specRigidbody.UnitCenter);
                }
                LootEngine.DoDefaultItemPoof(player.specRigidbody.UnitCenter);
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRoomClearEvent -= ItemGiver;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner != null)
            {
                base.Owner.OnAnyEnemyReceivedDamage -= this.OnEnemyDamaged;
            }
            base.OnDestroy();
        }

    }

}
