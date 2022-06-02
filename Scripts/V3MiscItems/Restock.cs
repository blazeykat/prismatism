using ItemAPI;
using System.Reflection;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System;

namespace katmod
{
    class Restock : PassiveItem
    {
        public static void Init()
        {
            string name = "Restock";
            string resourcePath = "katmod/Resources/V3MiscItems/gunpermit.png";
            GameObject gameObject = new GameObject(name);
            Restock item = gameObject.AddComponent<Restock>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Controlled";
            string longDesc = "Decreases shop prices, increase damage of purchased guns by 30%.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(115);
            item.AddPassiveStatModifier(PlayerStats.StatType.GlobalPriceMultiplier, -1f, StatModifier.ModifyMethod.ADDITIVE);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
            Hook mySanityIsDwindling = new Hook(typeof(ShopItemController).GetMethod("Interact", BindingFlags.Public | BindingFlags.Instance), typeof(Restock).GetMethod("TheJank"));
            Hook theStagewasSetForWar = new Hook(typeof(BaseShopController).GetMethod("PurchaseItem", BindingFlags.Public | BindingFlags.Instance), typeof(Restock).GetMethod("OnPurchase"));
        }

        public static void OnPurchase(Action<BaseShopController, ShopItemController, bool, bool> orig, BaseShopController self, ShopItemController item, bool flag1, bool flag2)
        {
            ETGModConsole.Log("god");
            if (GameManager.Instance.PrimaryPlayer.HasMTGConsoleID("psm:restock") && item.item.quality != ItemQuality.SPECIAL)
            {
                ETGModConsole.Log("shitkeeper");
                item.item.PersistsOnPurchase = true;
                FieldInfo _pickedUp = typeof(ShopItemController).GetField("pickedUp", BindingFlags.NonPublic | BindingFlags.Instance);
                _pickedUp.SetValue(item, false);
                LootEngine.DoDefaultItemPoof(item.specRigidbody.UnitCenter);
                if (item.item.quality != ItemQuality.COMMON && item.item.quality != ItemQuality.SPECIAL)
                {
                    ETGModConsole.Log("fart sex");
                    item.Initialize(self.shopItemsGroup2.defaultItemDrops.SelectByWeight().GetComponent<PickupObject>(), self);
                }
            } else
            {
                item.item.PersistsOnPurchase = false;
                FieldInfo _pickedUp = typeof(ShopItemController).GetField("pickedUp", BindingFlags.NonPublic | BindingFlags.Instance);
                _pickedUp.SetValue(item, true);
            }
            orig(self, item, flag1, flag2);
        }

        public static void TheJank(Action<ShopItemController, PlayerController> orig, ShopItemController self, PlayerController player)
        {
            /*if (player.HasMTGConsoleID("psm:restock"))
            {
                self.item.PersistsOnPurchase = true;
                LootEngine.DoDefaultItemPoof(self.gameObject.transform.position);
                if (self.item.quality != ItemQuality.COMMON && self.item.quality != ItemQuality.SPECIAL)
                {
                    ETGModConsole.Log("nuts and wet");
                    FieldInfo _parentShop = typeof(ShopItemController).GetField("m_parentShop", BindingFlags.NonPublic | BindingFlags.Instance);
                    ShopController shop = (ShopController)_parentShop.GetValue(self);
                    if (shop)
                    {
                        self.Initialize(shop.shopItemsGroup2.SelectByWeight().GetComponent<PickupObject>(), shop);
                    }
                    else
                    {
                        ETGModConsole.Log("loosen up");
                        FieldInfo _parentBaseShop = typeof(ShopItemController).GetField("m_baseParentShop", BindingFlags.NonPublic | BindingFlags.Instance);
                        BaseShopController baseShop = (BaseShopController)_parentBaseShop.GetValue(self);
                        if (baseShop)
                        {
                            ETGModConsole.Log("dont forget about it");
                            self.Initialize(baseShop.shopItemsGroup2.SelectByWeight().GetComponent<PickupObject>(), baseShop);
                        }
                        else
                        {
                            ETGModConsole.Log("fuck shit up");
                        }
                    }
                }
            } else
            {
                self.item.PersistsOnPurchase = false;
            }*/
            FieldInfo _parentBaseShop = typeof(ShopItemController).GetField("pickedUp", BindingFlags.NonPublic | BindingFlags.Instance);
            ETGModConsole.Log(_parentBaseShop.GetValue(self).ToString());
            ETGModConsole.Log("yes, everything ran correctly");
            orig(self, player);
        }

        /*public override void Pickup(PlayerController player)
        {
            player.OnItemPurchased += Restocker;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnItemPurchased -= Restocker;
            return base.Drop(player);
        }*/

        private void Restocker(PlayerController arg1, ShopItemController arg2)
        {
            arg2.item.PersistsOnPurchase = true;
            if (!arg2.gameObject || !arg2)
            {
                ETGModConsole.Log("my coffins all i see");
            }
            ETGModConsole.Log("time to drop");
            FieldInfo _parentShop = typeof(ShopItemController).GetField("m_parentShop", BindingFlags.NonPublic | BindingFlags.Instance);
            ShopController shop = (ShopController)_parentShop.GetValue(arg2);
            if (shop)
            {
                arg2.Initialize(Gungeon.Game.Items["psm:randy"], shop);
            }
            else
            {
                ETGModConsole.Log("loosen up");
                FieldInfo _parentBaseShop = typeof(ShopItemController).GetField("m_baseParentShop", BindingFlags.NonPublic | BindingFlags.Instance);
                BaseShopController baseShop = (BaseShopController)_parentBaseShop.GetValue(arg2);
                if (baseShop)
                {
                    ETGModConsole.Log("dont forget about it");
                    arg2.Initialize(Gungeon.Game.Items["psm:randy"], baseShop);
                } else
                {
                    ETGModConsole.Log("fuck shit up");
                }
            }
        }
    }
}
