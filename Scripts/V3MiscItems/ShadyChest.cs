using Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class ShadyChest : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Calamity Box";
            string resourcePath = "katmod/Resources/V3MiscItems/calamityboxempty";
            GameObject obj = new GameObject(itemName);
            ShadyChest item = obj.AddComponent<ShadyChest>();
            ItemBuilder.AddSpriteToObject(itemName, resourcePath, obj);
            string shortDesc = "Shady chest";
            string longDesc = "A wooden box which grows heavier with curse.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            item.PlaceItemInAmmonomiconAfterItemById(490);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
            item.consumable = true;

            processedSprites.Add(item.sprite.spriteId);
            foreach (string spriteID in additionalSprites)
            {
                int SpriteID = SpriteBuilder.AddSpriteToCollection(spriteID, item.sprite.Collection);
                processedSprites.Add(SpriteID);
            }
        }

        private static List<int> processedSprites = new List<int>();

        private static List<string> additionalSprites = new List<string>()
        {
            "katmod/Resources/V3MiscItems/calamityboxhalf",
            "katmod/Resources/V3MiscItems/calamityboxfull"
        };

        private enum SpriteState
        {
            Empty,
            Half,
            Full
        };

        private SpriteState state = SpriteState.Empty;

        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.stats.GetStatValue(PlayerStats.StatType.Curse) >= 9)
            {
                if (state != SpriteState.Full)
                {
                    state = SpriteState.Full;
                    sprite.SetSprite(processedSprites[2]);
                }
            } else if (user && user.stats.GetStatValue(PlayerStats.StatType.Curse) >= 5 && state != SpriteState.Half)
            {
                state = SpriteState.Half;
                sprite.SetSprite(processedSprites[1]);
            } else if (user && user.stats.GetStatValue(PlayerStats.StatType.Curse) < 5 && state != SpriteState.Empty)
            {
                state = SpriteState.Empty;
                sprite.SetSprite(processedSprites[0]);
            }
            return base.CanBeUsed(user);
        }

        protected override void DoEffect(PlayerController user)
        {
            if (user.CurrentRoom == null)
            {
                return;
            }
            base.DoEffect(user);
            try
            {
                float curse = user.stats.GetStatValue(PlayerStats.StatType.Curse);
                LootEngine.DoDefaultPurplePoof(user.specRigidbody.UnitCenter);
                if (curse < 1)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.Junk).gameObject, user.specRigidbody.UnitCenter, Vector2.zero, 1);
                    return;
                }
                if (curse < 2)
                {
                    Chest chest = GameManager.Instance.RewardManager.D_Chest;
                    SpawnJamChest(chest, user);
                    return;
                }
                if (curse < 3)
                {
                    Chest chest = GameManager.Instance.RewardManager.C_Chest;
                    SpawnJamChest(chest, user);
                    return;
                }
                if (curse < 5)
                {
                    Chest chest = GameManager.Instance.RewardManager.B_Chest;
                    SpawnJamChest(chest, user);
                    return;
                }
                if (curse < 7)
                {
                    Chest chest = GameManager.Instance.RewardManager.A_Chest;
                    SpawnJamChest(chest, user);
                    return;
                }
                if (curse < 9)
                {
                    Chest chest = GameManager.Instance.RewardManager.S_Chest;
                    SpawnJamChest(chest, user);
                    return;
                }

                Chest rainbowChest = GameManager.Instance.RewardManager.Rainbow_Chest;
                SpawnJamChest(rainbowChest, user);
                if (curse >= 15)
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.GoldJunk).gameObject, user.specRigidbody.UnitCenter, Vector2.zero, 1);
                    return;
                }
            } catch (Exception lerror)
            {
                ETGModConsole.Log(lerror.ToString());
            }
        }

        private void SpawnJamChest(Chest chest, PlayerController user)
        {
            Chest jamChest = Chest.Spawn(chest, user.sprite.WorldCenter + Vector2.down, user.CurrentRoom, true);
            jamChest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
            jamChest.ForceUnlock();
        }
    }
}
