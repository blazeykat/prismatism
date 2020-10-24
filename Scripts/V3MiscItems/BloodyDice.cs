using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class BloodyDice : PlayerItem
    {
        public static PlayerItem item;

        public static void Init()
        {
            string itemName = "Bloody Dice";
            string resourceName = "katmod/Resources/Dice/bloodydie";
            GameObject obj = new GameObject(itemName);
            BloodyDice item = obj.gameObject.AddComponent<BloodyDice>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Blood Gamble";
            string longDesc = "Gamble your life away.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(203);
            item.SetCooldownType(ItemBuilder.CooldownType.Timed, 2f);
            item.consumable = false;
            BloodyDice.item = item;
        }

        /*public override bool CanBeUsed(PlayerController user)
        {
            //return user.healthHaver.GetCurrentHealth() < 0.5f && CurrentTimeCooldown == 2f;
        }*/

        protected override void DoEffect(PlayerController user)
        {
            try
            {

                base.DoEffect(user);
                user.healthHaver.ApplyDamage(0.5f, Vector2.zero, "Crippling Gambling Addiction");
                int num = (int)UnityEngine.Random.Range(1, 7);
                user.BloopItemAboveHead(item.sprite);
                switch (num)
                {
                    case 1:
                        break;
                    case 2:
                        user.carriedConsumables.Currency += 4;
                        break;
                    case 3:
                        user.Blanks += 1;
                        break;
                    case 4:
                        user.carriedConsumables.KeyBullets += 1;
                        break;
                    case 5:
                        if (user)
                        {
                            user.inventory.AddGunToInventory(PickupObjectDatabase.GetRandomGun());
                        }
                        break;
                    case 6:
                        Chest chest = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value);
                        chest.IsLocked = false;
                        break;
                }
                AkSoundEngine.PostEvent("Play_OBJ_power_up_01", base.gameObject);
            } catch (Exception lerror)
            {
                ETGModConsole.Log(lerror.ToString()); 
            }
        }
    }
}
