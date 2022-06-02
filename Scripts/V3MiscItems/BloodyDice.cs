using ItemAPI;
using System;
using UnityEngine;

namespace katmod
{
    class BloodyDice : PlayerItem
    {
        static int D12SpriteID;

        static int BloodyDiceSpriteID;

        public static void Init()
        {
            string itemName = "Bloody Dice";
            string resourceName = "katmod/Resources/Dice/bloodydie";
            GameObject obj = new GameObject(itemName);
            BloodyDice item = obj.gameObject.AddComponent<BloodyDice>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Blood Gamble";
            string longDesc = "Gamble your life away.\n\nA red D6. Just one roll couldn't hurt...";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(203);
            item.SetCooldownType(ItemBuilder.CooldownType.Timed, 2.5f);
            item.consumable = false;
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);

            BloodyDiceSpriteID = SpriteBuilder.AddSpriteToCollection("katmod/Resources/Dice/bloodydie", item.sprite.Collection);
            D12SpriteID = SpriteBuilder.AddSpriteToCollection("katmod/Resources/Dice/bloodyd12butcooler", item.sprite.Collection);
        }

        public static bool SynergyExists;

        public override void Update()
        {
            base.Update();
            if (LastOwner)
            {
                bool NewSynergyCheck = LastOwner.PlayerHasActiveSynergy("Bloody D12");
                if (NewSynergyCheck != SynergyExists)
                {
                    SynergyExists = NewSynergyCheck;
                    if (SynergyExists)
                    {
                        this.sprite.SetSprite(D12SpriteID);
                    }
                    else
                    {
                        this.sprite.SetSprite(BloodyDiceSpriteID);
                    }
                }
            }
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user.healthHaver.IsVulnerable && user.CurrentRoom != null)
            {
                return base.CanBeUsed(user);
            }
            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            try
            {

                base.DoEffect(user);
                if (!user.PlayerHasActiveSynergy("Bloodthirsty") || !BoxOTools.BasicRandom(0.66f))
                {
                    user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Blood Sacrifice");
                }
                int num = (int)UnityEngine.Random.Range(1, user.PlayerHasActiveSynergy("Bloody D12") ? 13 : 7);
                user.BloopItemAboveHead(base.sprite);
                switch (num)
                {
                    default:
                    case 1:
                        BoxOTools.Notify("1", "Nothing Happened.", "katmod/Resources/Dice/bloodydie");
                        break;
                    case 2:
                        BoxOTools.Notify("2", "+4 Casings.", "katmod/Resources/Dice/bloodydie");
                        user.carriedConsumables.Currency += 4;
                        break;
                    case 3:
                        BoxOTools.Notify("3", "+1 Blank.", "katmod/Resources/Dice/bloodydie");
                        user.Blanks += 1;
                        break;
                    case 4:
                        BoxOTools.Notify("4", "+1 Key.", "katmod/Resources/Dice/bloodydie");
                        user.carriedConsumables.KeyBullets += 1;
                        break;
                    case 5:
                        if (user.specRigidbody.UnitCenter != null)
                        {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.SpreadAmmoPickup).gameObject, user.specRigidbody.UnitCenter, Vector2.zero, 1f);
                        BoxOTools.Notify("5", "Free spread ammo box", "katmod/Resources/Dice/bloodydie");
                        }
                        break;
                    case 6:
                        if (user.specRigidbody.UnitCenter != null)
                        {
                            LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.AmmoPickup).gameObject, user.specRigidbody.UnitCenter, Vector2.zero, 1f);
                            BoxOTools.Notify("6", "Free ammo box", "katmod/Resources/Dice/bloodydie");
                        }
                        break;
                    case 7:
                        LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(GlobalItemIds.Map).gameObject, user);
                        BoxOTools.Notify("7", "Floor Revealed", "katmod/Resources/Dice/bloodydie");
                        break;
                    case 8:
                        LootEngine.GivePrefabToPlayer(LootEngine.GetItemOfTypeAndQuality<PickupObject>(ItemQuality.B, GameManager.Instance.RewardManager.ItemsLootTable, true).gameObject, user);
                        BoxOTools.Notify("8", "Free passive", "katmod/Resources/Dice/bloodydie");
                        break;
                    case 9:
                        if (user)
                        {
                            user.inventory.AddGunToInventory(PickupObjectDatabase.GetRandomGun());
                            BoxOTools.Notify("9", "Free Gun", "katmod/Resources/Dice/bloodydie");
                        }
                        break;
                    case 10:
                        if (user.CurrentRoom != null)
                        {
                            Chest chest = Chest.Spawn(GameManager.Instance.RewardManager.Synergy_Chest, new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value);
                            BoxOTools.Notify("10", "Free synergy chest", "katmod/Resources/Dice/bloodydie");
                        }
                        break;
                    case 11:
                        if (user.CurrentRoom != null)
                        {
                            Chest chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value);
                            BoxOTools.Notify("11", "Free chest", "katmod/Resources/Dice/bloodydie");
                        }
                        break;
                    case 12:
                        if (user.CurrentRoom != null)
                        {
                            Chest unlockedChest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value);
                            unlockedChest.ForceUnlock();
                            BoxOTools.Notify("12", "Free unlocked chest", "katmod/Resources/Dice/bloodydie");
                        }
                        break;
                }
                AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
            }
            catch (Exception lerror)
            {
                ETGModConsole.Log(lerror.ToString());
            }
        }
    }
}
