using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class CandyBag : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Bottomless Bag of Candy";
            string resourceName = "katmod/Resources/V3MiscItems/bagofcandy";
            GameObject obj = new GameObject(itemName);
            CandyBag item = obj.gameObject.AddComponent<CandyBag>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Neverending";
            string longDesc = "A bag full of candy.\n\nUpon closer inspection, you can see something else down there, beneath the candy. Something jammed.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.S;
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            item.PlaceItemInAmmonomiconAfterItemById(663);
            item.consumable = true;
            ETGMod.Chest.OnPostOpen += PostOnOpen;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            for (int i = 0; i < 15; i++)
            {/*
                switch (rando.Next(1, 7))
                {
                    default:
                    case 1:
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(GreenCandy.GreenCandyID).gameObject, base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector2(), Vector2.zero, 1f, false, true, false);
                        break;
                    case 2:
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(BlueCandy.BlueCandyID).gameObject, base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector2(), Vector2.zero, 1f, false, true, false);
                        break;
                    case 3:
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(GoldenCandy.GoldenCandyID).gameObject, base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector2(), Vector2.zero, 1f, false, true, false);
                        break;
                    case 4:
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(KeyCandy.GreyCandyID).gameObject, base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector2(), Vector2.zero, 1f, false, true, false);
                        break;
                    case 5:
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(RedCandy.RedCandyID).gameObject, base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector2(), Vector2.zero, 1f, false, true, false);
                        break;
                    case 6:
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(HeartCandy.HeartCandyID).gameObject, base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(1, 1).ToVector2(), Vector2.zero, 1f, false, true, false);
                        break;
                }*/

            }
            List<PickupObject> contents = new List<PickupObject>()
            {
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.A, GameManager.Instance.RewardManager.ItemsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.A, GameManager.Instance.RewardManager.GunsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.S, GameManager.Instance.RewardManager.ItemsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.S, GameManager.Instance.RewardManager.ItemsLootTable, false),
                LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.S, GameManager.Instance.RewardManager.ItemsLootTable, false),
            };
            Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.C_Chest, user.sprite.WorldCenter + Vector2.down, user.sprite.WorldCenter.GetAbsoluteRoom(), true);
            chest.IsLocked = false;
            chest.contents = contents;
            chest.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
            chest.sprite.usesOverrideMaterial = true;
            thechest = chest;
        }

        static void PostOnOpen(Chest chester, PlayerController user)
        {
            if (chester == thechest)
            {
                StatModifier modify = new StatModifier();
                modify.modifyType = StatModifier.ModifyMethod.ADDITIVE;
                modify.statToBoost = PlayerStats.StatType.Curse;
                modify.amount = 10;
                user.ownerlessStatModifiers.Add(modify);
                user.stats.RecalculateStats(user);
            }
        }

        private static Chest thechest;
    }
}
