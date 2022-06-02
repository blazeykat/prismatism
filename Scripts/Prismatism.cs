using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    public class Prismatism : ETGModule
    {
        public static readonly string MOD_NAME = "Prismatism";
        public static readonly string VERSION = "0.2.7 (This time without the console logging)";
        public static readonly string TEXT_COLOR = "#7289da";

        public override void Init()
        {
            try
            {
                DaveAPIManager.Setup("psm");
            }
            catch (Exception E)
            {
                ETGModConsole.Log($"{E}");
            }
        }

        
        public override void Start()
        {
            try
            {

                FakePrefabHooks.Init();
                ItemBuilder.Init();
                PlayerEventAdder.Init();
                UnlockHookInators.AddHooks();

                MagmaticBlood.Init();
                MimicSkin.Init(); //Added to the doc
                SaplingBullets.Init(); //Added to the doc
                SlightlyLargerBullets.Init(); //Added to the doc
                WarriorsSyringe.Init(); //Added to the doc
                VultureFeather.Init(); //Added to the doc
                StraponRPG.Init(); //Added to the doc
                Jeremy.Init(); //Added to the doc
                Gasoline.Init(); //Added to the doc
                CatSnack.Init(); //Added to the doc
                ClassicCatSnack.Init();
                JunkSynthesizer.Init();

                WyrmBlood.Init(); //Added to the doc

                //GreenCandy.Init();
                //KeyCandy.Init();
                //GoldenCandy.Init();
                //BlueCandy.Init();
                //RedCandy.Init();
                //HeartCandy.Init();
                //MonsterCandy.Init();
                ImprovedCandies.CandiesInit();
                ImprovedCandies.PositiveEffectsInit();
                //TrickOTreater.Init(); //Added to the doc

                BlackSkull.Init();
                StoneSword.Init();
                StoneAmmolet.Init();
                Randy.Init();

                BeeGun.Add();
                //BloodyCannon.Add();
                //WindStaff.Add();
                RedAndWhite.Add();
                RoyalShotgun.Add();
                //Shotstool.Add();
                RescueRanger.Add();
                NuclearShotgun.Add();
                NuclearAmmoPickup.Init();
                SweeperGun.Add();
                MaliciousRailcannon.Add();
                Superbug.Add();

                PetrifyingMirror.Init();
                EnchantedTome.Init();
                BronzeKey.Init();
                StackOfCash.Init();
                ExecutionShells.Init();
                BloodBullets.Init();
                HoodedShells.Init();
                ClockworkCog.Init();
                HighPriestCloak.Init();
                LuckyCoin.Init();
                LuckyHorseshoe.Init();
                ToxicFungus.Init();
                PurpleGuonStone.Init();
                GarbageBin.Init();
                ColdAmmo.Init();
                MaidenPlating.Init();
                PlagueBullets.Init();
                CrownOfBlood.Init();
                GalacticChest.Init();
                Bravery.Init();
                RaidersAxe.Init();
                BulletSynthesizer.Init();
                ElectricRounds.Init();
                ThunderRounds.Init();
                ToxicHoneycomb.Init();

                BoomerangBullets.Init();
                OilyGreaves.Init();
                ShadyChest.Init();
                //CandyBag.Init();
                MinerHelmet.Init();
                Depthmeter.Init();
                Smore.Init();
                KeyDoubler.Init();
                BlankDoubler.Init();
                CursedCandle.Init();
                MyNameIsYoshikageKira.Init();
                MythrilBullets.Init();
                Charcoal.Init();
                CodexUmbra.Init();
                BloodyDice.Init();
                StarFruit.Init();
                HeartyKey.Init();
                DeadlyKnife.Init();
                Overloader.Init();
                Sushi.Init();
                AmmoEnhancer.Init();
                BanditHat.Init();
                Ushanka.Init();
                BigBoom.Init();
                Seashell.Init();
                //GoldenBox.Init();
                CobaltCoin.Init();

                RingOfWealth.Init();
                QuartzCrystal.Init();
                DeadRinger.Init();
                RoyalCapacitator.Init();
                Cookbook.Init();
                ParrotsFeather.Init();

                Soulbauble.Init();
                FlameWisp.Init();

                CatsEye.Init();
                GunPermit.Init();
                DormantStabiliser.Init();
                CaveCarrot.Init();

                BlueAlbum.Init();

                DoubleABattery.Init();
                ScarecrowsHead.Init();
                OnyxBracelet.Init();
                LemonPig.Init();

                IfritsHorn.Init();
                EnderHelmet.Init();
                BabyGoodRobot.Init();
                CaptainsBrooch.Init();
                SoulInator.Init();

                TwoOfHearts.Init();
                ParasiticFungus.Init();
                ExperimentalRocketLauncher.Init();

                AbyssalShield.Init();

                Coil.Init();

                //Restock.Init();
                //ChainOfBeing.Init();
                //RangeCrystal.Init();
                //BloodyLockpick.Init();
                //Bitey.Init();
                //JestersHat.Init();
                //PrismaticLexicon.Init();
                //GhoulBullets.Init();

                IncubusStartingPassive.Init();
                IncubusStartingActive.Init();

                /*BlueExplosive.Init();
                RedExplosive.Init();
                WhiteExplosive.Init();
                SwarmSneak.Init();
                YVSwarmPickup.Init();
                WhiteDamageUp.Init();
                Drone.Init();*/

                ETGModConsole.Commands.AddGroup("psmdebug", delegate (string[] args)
                {
                    Log("Please specify a command.", TEXT_COLOR);
                });
                ETGModConsole.Commands.GetGroup("psmdebug").AddUnit("guaranteeproc", delegate (string[] args)
                {
                    DebugChance = !DebugChance;
                    Log($"Guarantee Proc is now: {DebugChance}", TEXT_COLOR);
                });
                ETGModConsole.Commands.GetGroup("psmdebug").AddUnit("unlocks", delegate (string[] args)
                {
                    int totalItems = 0;
                    int unlockedItems = 0;
                    List<PickupObject> list = new List<PickupObject>();
                    List<PickupObject> list2 = new List<PickupObject>();
                    foreach (PickupObject item in ItemBuilder.AddedItems)
                    {
                        EncounterTrackable trolling = item.GetComponent<EncounterTrackable>();
                        if (trolling && trolling.prerequisites != null && trolling.prerequisites.Length > 0)
                        {
                            totalItems++;
                            if (trolling.PrerequisitesMet())
                            {
                                unlockedItems++;
                                list2.Add(item);
                            } else
                            {
                                list.Add(item);
                            }
                        }
                    }
                    Log($"Unlocked: {unlockedItems}/{totalItems}", TEXT_COLOR);
                    if (list2.Count > 0)
                    {
                        Log("Items Unlocked:", TEXT_COLOR);
                        foreach (PickupObject item in list2)
                        {
                            Log(item.name, TEXT_COLOR);
                        }
                    }
                    if (list.Count > 0)
                    {
                        Log("Items Left:", TEXT_COLOR);
                        foreach (PickupObject item in list)
                        {
                            Log($"{item.name}: {Unlocks[item.PickupObjectId]}", TEXT_COLOR);
                        }
                    }
                });
                ETGModConsole.Commands.GetGroup("psmdebug").AddUnit("getcustommaximums", delegate (string[] args)
                {
                    Log($"Most Damage Had: {DaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.MAXIMUM_DAMAGE)}", TEXT_COLOR);
                    Log($"Most Health Had: {DaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.MAXIMUM_HEALTH)}", TEXT_COLOR);
                    Log($"Most Money Had: {DaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.MOST_MONEY)}", TEXT_COLOR);
                });
            }
            catch (Exception ex)
            {
                itemsLoad = $"{ex}";
            }

            ETGMod.StartGlobalCoroutine(IfOtherModsExist());
        }

        public static IEnumerator IfOtherModsExist()
        {
            yield return null;
            LandOfTheSynergies.AddSynergies();
        }

        public static void LogStart()
        {
            Log($"{MOD_NAME} V{VERSION} has started.", TEXT_COLOR);

            Log("Checklist:", TEXT_COLOR);
            Log($"Items: {itemsLoad}", TEXT_COLOR);
            Log($"Synergies: {synergiesLoad}", TEXT_COLOR);
            if (itemsLoad == "Loaded." && synergiesLoad == "Loaded.")
                Log("Enjoy the mod? Give me your wallet.", TEXT_COLOR);
            else
                Log("oops", TEXT_COLOR);
        }

        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }

        public static string itemsLoad = "Loaded.";

        public static string synergiesLoad = "Loaded.";

        public static string shrinesLoad = "Loaded.";

        public static string enemiesLoad = "Loaded.";

        public static bool DebugChance = false;

        public static Dictionary<int, string> Unlocks = new Dictionary<int, string>();
    }
}
