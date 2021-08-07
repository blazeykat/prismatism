using Gungeon;
using ItemAPI;
using System;
using System.Collections.Generic;

namespace katmod
{
    static class LandOfTheSynergies
    {
        public static void AddSynergies()
        {
            try
            {
                List<string> StickOnRocketLauncher = new List<string>
                {
                    "psm:stick_on_rocket_launcher"
                };
                List<string> RocketKing = new List<string>
                {
                    "yari_launcher",
                    "com4nd0"
                };
                if (Game.Items.ContainsID("nn:rocket_man")) { RocketKing.Add("nn:rocket_man"); RocketKing.Add("nn:rocket_pistol"); }
                CustomSynergies.Add("Rocket King", StickOnRocketLauncher, RocketKing);

                List<string> leafOrb = new List<string>
                {
                    "psm:sapling_bullets",
                    "life_orb"
                };
                CustomSynergies.Add("Leaf orb", leafOrb);
                List<string> theList = new List<string>
                {
                    "psm:sapling_bullets",
                    "mahoguny"
                };
                CustomSynergies.Add("Mahoguny Sapling", theList);

                List<string> mimicSuit = new List<string>
                {
                    "psm:mimic_skin_cape",
                    "mimic_tooth_necklace"
                };
                CustomSynergies.Add("Mimic Suit", mimicSuit, null, true);


                List<string> fireproof = new List<string>
                {
                    "psm:gasoline",
                    "ring_of_fire_resistance"
                };
                CustomSynergies.Add("Fireproof", fireproof, null, true);

                List<string> gasoline = new List<string>
                {
                    "psm:gasoline"
                };

                List<string> rainpour = new List<string>
                {
                    "psm:warrior's_syringe",
                    "psm:toxic_fungus",
                    "psm:royal_capacitor"
                };
                if (Game.Items.ContainsID("bny:sky_grass")) { rainpour.Add("bny:sky_grass"); }
                CustomSynergies.Add("Rainpour", gasoline, rainpour);

                List<string> soundsmithMoment = new List<string>
                {
                    "psm:cat_snack",
                    "barrel"
                };
                CustomSynergies.Add("feesh", soundsmithMoment, null, true);
                List<string> sharks = new List<string>
                {
                    "psm:cat_snack",
                    "compressed_air_tank"
                };
                CustomSynergies.Add("Shark Bait", sharks);

                List<string> bravery = new List<string>
                {
                    "psm:medal_of_bravery",
                    "galactic_medal_of_valor"
                };
                CustomSynergies.Add("True Bravery", bravery);

                List<string> ColdAsIce = new List<string>
                {
                    "psm:cool_ammo",
                    "ice_cube"
                };
                CustomSynergies.Add("Cold as Ice", ColdAsIce);

                List<string> mirrorCannon = new List<string>
                {
                    "psm:petrifying_mirror",
                    "glass_cannon"
                };
                CustomSynergies.Add("Mirror Cannon", mirrorCannon, null, true);

                List<string> Sterling = new List<string>
                {
                    "psm:plague_bullets",
                    "plague_pistol"
                };
                CustomSynergies.Add("Plaguebringer Goliath", Sterling, null, true);

                List<string> purpleBase = new List<string>
                {
                    "psm:blurple_guon_stone"
                };
                List<string> purpleOpt = new List<string>
                {
                    "+1_bullets",
                    "gundromeda_strain",
                    "amulet_of_the_pit_lord"
                };
                CustomSynergies.Add("Blurpler guon stone", purpleBase, purpleOpt);

                List<string> infirmary = new List<string>
                {
                    "psm:thunder_shells",
                    "psm:electric_shells"
                };
                CustomSynergies.Add("The infirmary", infirmary, null, true);
                List<string> clapClap = new List<string>
                {
                    "psm:thunder_shells",
                    "thunderclap"
                };
                CustomSynergies.Add("Clap Clap", clapClap, null, true);

                List<string> toxicCombBees = new List<string>
                {
                    "jar_of_bees",
                    "bee_hive",
                    "honeycomb"
                };

                List<string> toxicComb = new List<string>
                {
                    "psm:toxic_honeycomb"
                };


                List<string> mandatoryConsoleIDs = new List<string>
                {
                    "psm:trick_o'_treater",
                    "monster_blood"
                };
                //CustomSynergies.Add("Monster Candy", mandatoryConsoleIDs, null, true);

                /*List<string> toxicCombPoisons = new List<string>
                {
                    "plague_pistol",
                    "the_membrane",
                    "poxcannon",
                    "psm:plague_bullets"
                };
                if (Game.Items.ContainsID("nn:graceful_goop")) { toxicCombPoisons.Add("nn:graceful_goop"); }
                if (Game.Items.ContainsID("nn:hive_holster")) { toxicCombBees.Add("nn:hive_holster"); }
                CustomSynergies.Add("Bubeenic", toxicComb, toxicCombPoisons);
                CustomSynergies.Add("bzzzzzzzzzzzzzzz", toxicComb, toxicCombBees);*/

                List<string> keysight = new List<string>
                {
                    "psm:bronze_key",
                    "yellow_chamber"
                };
                CustomSynergies.Add("keysight", keysight, null, true);

                List<string> featherBase = new List<string>
                {
                    "psm:parrot's_feather"
                };
                List<string> ofAFeather = new List<string>
                {
                    "owl",
                    "wax_wings",
                    "weird_egg"
                };
                List<string> flockTogether = new List<string>
                {
                    "charm_horn",
                    "charming_rounds",
                    "yellow_chamber",
                    "battle_standard"
                };
                if (Game.Items.ContainsID("nn:kaliber's_eye")) { flockTogether.Add("nn:kaliber's_eye"); }
                CustomSynergies.Add("Birds of a Feather", featherBase, ofAFeather);
                CustomSynergies.Add("Flock Together", featherBase, flockTogether);

                List<string> rockman = new List<string>
                {
                    "psm:stone_sword",
                    "psm:stone_ammolet"
                };
                CustomSynergies.Add("Withered away", rockman);

                List<string> bookBase = new List<string>
                {
                    "psm:enchanted_book"
                };
                List<string> Scholar = new List<string>
                {
                    "book_of_chest_anatomy",
                    "psm:cookbook",
                    "psm:codex_umbra"
                };
                if (Game.Items.ContainsID("nn:book_of_mimic_anatomy")) { Scholar.Add("nn:book_of_mimic_anatomy"); }
                int ID;
                if (Game.Items.ContainsID("nn:book_of_mimic_anatomy")) { ID = Game.Items["nn:book_of_mimic_anatomy"].PickupObjectId; }
                CustomSynergies.Add("Book Worm", bookBase, Scholar);

                List<string> Reanimate = new List<string>
                {
                    "psm:bronze_key",
                    "shelleton_key"
                };
                CustomSynergies.Add("REANIMATE", Reanimate);

                List<string> smoreBase = new List<string>
                {
                    "psm:s'more"
                };
                List<string> campfireStory = new List<string>
                {
                    "psm:gasoline",
                    "psm:shoddy_lighter",
                    "hot_lead"
                };
                CustomSynergies.Add("Campfire Story", smoreBase, campfireStory);

                List<string> Mines = new List<string>
                {
                    "psm:depthometer",
                    "psm:miner's_helmet"
                };
                CustomSynergies.Add("Into the Depths", Mines);
                List<string> retro = new List<string>
                {
                    "psm:sweeper",
                    "psm:wastelanders_shotgun"
                };
                CustomSynergies.Add(":retroswept:", retro);

                List<string> twentyInch = new List<string>
                {
                    "psm:abyssal_knife"
                };
                List<string> nails = new List<string>
                {
                    "buzzkill",
                    "super_meat_gun"
                };
                if (Game.Items.ContainsID("cel:sawblade")) { nails.Add("cel:sawblade"); }
                CustomSynergies.Add("Hung from her septum", twentyInch, nails);

                List<string> sushiFish = new List<string>
                {
                    "psm:sushi",
                    "psm:cat_snack"
                };
                CustomSynergies.Add("Fish Can Roll", sushiFish);

                List<string> permit = new List<string>
                {
                    "psm:gun_permit"
                };
                List<string> printMoney = new List<string>
                {
                    "gilded_bullets",
                    "coin_crown"
                };
                if (Game.Items.ContainsID("nn:gold_guon_stone")) { printMoney.Add("nn:gold_guon_stone"); }
                CustomSynergies.Add("Print Money", permit, printMoney);

                List<string> ranger = new List<string>
                {
                    "psm:rescue_rifle"
                };
                List<string> rangerSentry = new List<string>
                {
                    "r2g2",
                    "portable_turret"
                };
                CustomSynergies.Add("Sentry Goin' Up", ranger, rangerSentry);

                List<string> rumbleOfTheDrums = new List<string>
                {
                    "psm:cats_eye",
                    "drum_clip"
                };
                CustomSynergies.Add("Rumble of the Drums", rumbleOfTheDrums);

                List<string> coldAsIce = new List<string>
                {
                    "psm:cookbook",
                    "ice_bomb"
                };
                List<string> ThisTune = new List<string>
                {
                    "psm:cookbook"
                };
                List<string> BombingBuddies = new List<string>
                {
                    "lil_bomber",
                    "bomb",
                    "ibomb_companion_app",
                    "roll_bomb"
                };
                if (Game.Items.ContainsID("nn:bombinomicon")) { BombingBuddies.Add("nn:bombinomicon"); }
                CustomSynergies.Add("Bomb Buds", ThisTune, BombingBuddies);
                CustomSynergies.Add("Cold as Ice", coldAsIce);

                List<string> DoubleTheFall = new List<string>
                {
                    "psm:shady_key",
                    "psm:shady_blank"
                };
                CustomSynergies.Add("Twice the Pride", DoubleTheFall);

                List<string> BloodyDice = new List<string>
                {
                    "psm:bloody_dice",
                };
                List<string> bloodThirsty = new List<string>
                {
                    "antibody",
                    "blood_brooch"
                };
                List<string> D12 = new List<string>
                {
                    "chance_bullets",
                    "lament_configurum",
                    "chaos_bullets"
                };
                CustomSynergies.Add("Bloodthirsty", BloodyDice, bloodThirsty);
                CustomSynergies.Add("Bloody D12", BloodyDice, D12);

                List<string> LuckyBird = new List<string>
                {
                    "seven_leaf_clover",
                    "psm:magic_vulture_feather"
                };
                CustomSynergies.Add("Lucky Bird", LuckyBird);

                List<string> suicideKing = new List<string>
                {
                    "psm:big_boom",
                    "psm:bulletrangs"
                };
                CustomSynergies.Add("Suicide King", suicideKing);

                List<string> boomerangBros = new List<string>
                {
                    "boomerang",
                    "psm:bulletrangs"
                };
                CustomSynergies.Add("Boomerang Bros", boomerangBros);

                List<string> Oink = new List<string>
                {
                    "pig",
                    "psm:lemon_pig"
                };

                List<string> LemonPig = new List<string>
                {
                    "psm:lemon_pig"
                };
                List<string> KeeperOfTheCoin = new List<string>
                {
                    "coin_crown",
                    "iron_coin"
                };
                List<string> OrangePig = new List<string>
                {
                    "psm:lemon_pig",
                    "orange"
                };
                CustomSynergies.Add("Orange Pig", OrangePig);
                CustomSynergies.Add("Oink", Oink);
                CustomSynergies.Add("Keeper of the Coin", LemonPig, KeeperOfTheCoin);

                List<string> babyGoodRobot = new List<string>()
                {
                    "psm:baby_good_robot"
                };
                List<string> robotInfluence = new List<string>()
                {
                    "nanomachines",
                    "robots_left_hand",
                    "psm:rustic_cog"
                };
                if (Game.Items.ContainsID("bny:baby_good_modular")) { robotInfluence.Add("bny:baby_good_modular"); }
                List<string> theRobotsHere = new List<string>()
                {
                    "gilded_bullets"
                };
                CustomSynergies.Add("Robot Influence", babyGoodRobot, robotInfluence);
                CustomSynergies.Add("If the robot's paid", babyGoodRobot, theRobotsHere);
            }
            catch (Exception error)
            {
                Prismatism.synergiesLoad = error.ToString();
            }
            Prismatism.LogStart();
        }
    }
}
