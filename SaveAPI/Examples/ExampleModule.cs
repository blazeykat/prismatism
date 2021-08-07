/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;

namespace SaveAPI
{
    public class ExampleModule// : ETGModule
    {
        public void Init()
        {
            //Setups SaveAPI with the mod prefix "example"
            SaveAPIManager.Setup("example");
        }

        public void Start()
        {
            //Adds test commands
            ETGModConsole.Commands.AddGroup("saveapi_example");
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("get_flag", delegate(string[] args) 
            {
                ETGModConsole.Log("CustomDungeonFlags.EXAMPLE_FLAG's value: " + SaveAPIManager.GetFlag(CustomDungeonFlags.EXAMPLE_FLAG).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("set_flag", delegate (string[] args)
            {
                if(!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.SetFlag(CustomDungeonFlags.EXAMPLE_FLAG, bool.Parse(args[0]));
                ETGModConsole.Log("CustomDungeonFlags.EXAMPLE_FLAG's new value: " + SaveAPIManager.GetFlag(CustomDungeonFlags.EXAMPLE_FLAG).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("get_stat", delegate (string[] args)
            {
                ETGModConsole.Log("CustomTrackedStats.EXAMPLE_STATS's value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("set_stat", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.SetStat(CustomTrackedStats.EXAMPLE_STATS, float.Parse(args[0]));
                ETGModConsole.Log("CustomTrackedStats.EXAMPLE_STATS's new value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("increment_stat", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.RegisterStatChange(CustomTrackedStats.EXAMPLE_STATS, float.Parse(args[0]));
                ETGModConsole.Log("CustomTrackedStats.EXAMPLE_STATS's new value: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.EXAMPLE_STATS).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("get_maximum", delegate (string[] args)
            {
                ETGModConsole.Log("CustomTrackedMaximums.EXAMPLE_MAXIMUM's value: " + SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("set_maximum", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.UpdateMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM, float.Parse(args[0]));
                ETGModConsole.Log("CustomTrackedMaximums.EXAMPLE_MAXIMUM's new value: " + SaveAPIManager.GetPlayerMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("get_char_flag", delegate (string[] args)
            {
                ETGModConsole.Log("CustomDungeonFlags.EXAMPLE_FLAG's value: " + SaveAPIManager.GetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG).ToString());
            });
            ETGModConsole.Commands.GetGroup("saveapi_example").AddUnit("set_char_flag", delegate (string[] args)
            {
                if (!ETGModConsole.ArgCount(args, 1))
                {
                    return;
                }
                SaveAPIManager.SetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG, bool.Parse(args[0]));
                ETGModConsole.Log("CustomDungeonFlags.EXAMPLE_FLAG's new value: " + SaveAPIManager.GetCharacterSpecificFlag(CustomCharacterSpecificGungeonFlags.EXAMPLE_CHARACTER_SPECIFIC_FLAG).ToString());
            });
            //setups custom unlocks
            Game.Items["mustache"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_FLAG, true);
            Game.Items["easy_reload_bullets"].SetupUnlockOnCustomStat(CustomTrackedStats.EXAMPLE_STATS, 10.5f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Game.Items["metronome"].SetupUnlockOnCustomMaximum(CustomTrackedMaximums.EXAMPLE_MAXIMUM, 92f, DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO);
            //adds a new tier to Ox and Cadence's shop
            Game.Items["oiled_cylinder"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTMETA_1, true); // setups the first item's unlock
            Game.Items["smoke_bomb"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTMETA_2, true); // setups the second item's unlock
            Game.Items["armor_of_thorns"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTMETA_3, true); //setups the third item's unlock
            BreachShopTool.AddBaseMetaShopTier(Game.Items["oiled_cylinder"].PickupObjectId, 129, Game.Items["smoke_bomb"].PickupObjectId, 786, Game.Items["armor_of_thorns"].PickupObjectId, 465); // adds the new tier
            //adds new items to other breach shops
            Game.Items["rolling_eye"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTBEETLEE, true); //setups rolling eye's unlock
            Game.Items["rolling_eye"].AddItemToDougMetaShop(214, 0); //adds rolling eye to doug's breach shop as the first item
            Game.Items["magic_sweet"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTGOOP, true); //setups rolling eye's unlock
            Game.Items["magic_sweet"].AddItemToGooptonMetaShop(132, 0); //adds rolling eye to goopton's breach shop as the first item
            Game.Items["laser_sight"].SetupUnlockOnCustomFlag(CustomDungeonFlags.EXAMPLE_BLUEPRINTTRUCK, true); //setups rolling eye's unlock
            Game.Items["laser_sight"].AddItemToTrorcMetaShop(1123); //adds rolling eye to trorc's breach shop as the last item
            ETGModConsole.Log("hello");
        }

        public void Exit() { }
    }
}*/
