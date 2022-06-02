using Dungeonator;
using ItemAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class ImprovedCandies
    {
        public static List<Action<PlayerController>> PositiveCandyEffects = new List<Action<PlayerController>>();

        public static void CandiesInit()
        {
            string name = "Lolly";
            string resourcePath = "katmod/Resources/Candies/heartpop";
            GameObject gameObject = new GameObject(name);
            CandyPickup item = gameObject.AddComponent<CandyPickup>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "you will literally never see this";
            string longDesc = "bitch ass";
            item.SetupItem(shortDesc, longDesc);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
            item.RemovePickupFromLootTables();
            foreach (string spriteID in candySprites)
            {
                GameObject spriteObject = new GameObject(spriteID.Substring("katmod/Resources/Candies/".Length));
                ItemBuilder.AddSpriteToObject(spriteID.Substring("katmod/Resources/Candies/".Length), spriteID, spriteObject);
                candySpritesProcessed.Add(spriteObject.GetComponent<tk2dSprite>());
                //ETGModConsole.Log($"Init'd candy sprite {spriteID}");
            }
            SpriteBuilder.AddSpriteToCollection("katmod/Resources/Candies/happypilldude", SpriteBuilder.ammonomiconCollection);
        }

        public static void PositiveEffectsInit()
        {
            //PositiveCandyEffects.Add(PositiveEffects.FullHealth);
            PositiveCandyEffects.Add(PositiveEffects.ArmouredUp);
        }

        private static List<string> candySprites = new List<string>
        {
            "katmod/Resources/Candies/heartpop",
            "katmod/Resources/Candies/greenpop",
            "katmod/Resources/Candies/redpop",
            "katmod/Resources/Candies/goldenpop",
            "katmod/Resources/Candies/bluepop",
            "katmod/Resources/Candies/cursepop"
        };

        public static List<tk2dBaseSprite> candySpritesProcessed = new List<tk2dBaseSprite>();
    }

    class CandyPickup : PlayerItem
    {
        protected override void Start()
        {
            try
            {
                base.Start();
                tk2dBaseSprite baseSprite = BraveUtility.RandomElement(ImprovedCandies.candySpritesProcessed);
                if (baseSprite)
                    sprite.SetSprite(baseSprite.spriteId);
                else
                    ETGModConsole.Log("dumbass bro");
            } catch (Exception error)
            {
                error.ToString().Log();
            }
        }

        protected override void DoEffect(PlayerController player)
        {
            BraveUtility.RandomElement(ImprovedCandies.PositiveCandyEffects).Invoke(player);
            player.BloopItemAboveHead(base.sprite);
            
            ETGModConsole.Log("GJ idiot ...");
        }
    }
}
