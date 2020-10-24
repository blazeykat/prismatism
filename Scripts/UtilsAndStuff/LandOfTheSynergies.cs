using ItemAPI;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace katmod
{
    static class LandOfTheSynergies
    {
        public static void AddSynergies()
        {
            List<string> rocketKingNec = new List<string>
            {
                "psm:stick_on_rocket_launcher"
            };
            List<string> rocketKingOpti = new List<string>
            {
                "yari_launcher",
                "com4nd0"
            };
            CustomSynergies.Add("Rocket King", rocketKingNec, rocketKingOpti);

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

            List<string> vaporOption = new List<string>
            {
                "psm:warrior's_syringe",
                "psm:toxic_fungus"
            };
            CustomSynergies.Add("Rainpour", gasoline, vaporOption);

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
            CustomSynergies.Add("Thank god for me.", Sterling, null, true);

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

            List<string> toxicCombPoisons = new List<string>
            {
                "plague_pistol",
                "the_membrane",
                "poxcannon",
                "psm:plague_bullets"
            };
            CustomSynergies.Add("Bubeenic", toxicComb, toxicCombPoisons);
            CustomSynergies.Add("bzzzzzzzzzzzzzzz", toxicComb, toxicCombBees);

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
                "psm:cookbook"
            };
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
                "psm:retro_shotgun",
                "sunglasses"
            };
            CustomSynergies.Add(":retroswept:", retro);

            List<string> twentyInch = new List<string>
            {
                "psm:fatal_knife"
            };
            List<string> nails = new List<string>
            {
                "buzzkill",
                "super_meat_gun"
            };
            CustomSynergies.Add("Hung from her septum", twentyInch, nails);

            List<string> sushiFish = new List<string>
            {
                "psm:sushi",
                "psm:cat_snack"
            };
            CustomSynergies.Add("Fish Can Roll", sushiFish);
        }
    }
}
