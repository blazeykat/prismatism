using System;
using UnityEngine;
using katmod.ExpandAudio;
using ItemAPI;
using Dungeonator;
using System.IO;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace katmod
{
    public class Module : ETGModule
    {
        public static readonly string MOD_NAME = "Prismatism";
        public static readonly string VERSION = "0.1.9";
        public static readonly string TEXT_COLOR = "#7289da";

        public override void Init()
        {
            try
            {
                AudioResourceLoader.InitAudio();
            } catch (Exception E)
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

                MagmaticBlood.Init();
                MimicSkin.Init();
                SaplingBullets.Init();
                SlightlyLargerBullets.Init();
                WarriorsSyringe.Init();
                VultureFeather.Init();
                StraponRPG.Init();
                Jeremy.Init();
                Gasoline.Init();
                CatSnack.Init();
                JunkSynthesizer.Init();

                WyrmBlood.Init();

                GreenCandy.Init();
                KeyCandy.Init();
                GoldenCandy.Init();
                BlueCandy.Init();
                MonsterCandy.Init();
                HeartCandy.Init();
                RedCandy.Init();
                TrickOTreator.Init();

                BlackSkull.Init();
                Randy.Init();
                StoneSword.Init();
                StoneAmmolet.Init();

                BeeGun.Add();
                //Ak_01.Add(); 

                PetrifyingMirror.Init();
                EnchantedTome.Init();
                YellowKey.Init();
                StackOfCash.Init();
                ExecutionShells.Init();
                BloodBullets.Init();
                HoodedShells.Init();
                ClockworkCog.Init();
                HyperBullets.Init();
                LuckyCoin.Init();
                LuckyHorseshoe.Init();
                BoilingFungus.Init();
                PurpleGuonStone.Init();
                GarbageBin.Init();
                ColdAmmo.Init();
                MaidenPlating.Init();
                PlagueBullets.Init();
                CrownOfBlood.Init();
                GalacticChest.Init();
                Bravery.Init();
                RaidersAxe.Init();
                LittleDisciple.Init();
                PrismaticSnail.Init();
                ElectricRounds.Init();
                ThunderRounds.Init();
                ToxicHoneycomb.Init();
            }
            catch (Exception ex)
            {
                itemsLoad = $"{ex}";
            }
            /*try
            {

                GungeonInit.Init();
                GooptonShrine.Add();
                LowPriestShrine.Add();
                GooptonIcon.Init();
                LowPriestIcon.Init();
            } catch (Exception E)
            {
                shrinesLoad = $"{E}";
            }*/
            /*try
            {
                BlueBubbler.Init();
            } catch (Exception e)
            {
                enemiesLoad = $"{e}";
            }*/
            Log($"{MOD_NAME} v{VERSION} has started.", TEXT_COLOR);
            Log("Checklist:", TEXT_COLOR);
            Log($"Items: {itemsLoad}", TEXT_COLOR);
            Log("bonbo's world", TEXT_COLOR);
            //Log($"Enemies: {enemiesLoad}", TEXT_COLOR);
        }


        public static void Log(string text, string color="FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public static string ConfigDirectory = Path.Combine(global::ETGMod.ResourcesDirectory, "katmod");
        public override void Exit() { }

        public static bool dededebug = false;

        public string itemsLoad = "Loaded.";

        public string shrinesLoad = "Loaded.";

        public string enemiesLoad = "Loaded.";
    }
}
