using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class CaptainsBrooch : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Captain's Brooch";
            string resourceName = "katmod/Resources/V3MiscItems/captainsbrooch";
            GameObject obj = new GameObject(itemName);
            CaptainsBrooch item = obj.gameObject.AddComponent<CaptainsBrooch>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Aye Aye, Captain!";
            string longDesc = "Spawns 1 random chest.\n\nAn old brooch, previously owned by a grizzled captain, before it found it's way to the gungeon. Perhaps there's some way to contact him still.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.A;
            item.PlaceItemInAmmonomiconAfterItemById(573);
            item.SetCooldownType(ItemBuilder.CooldownType.Damage, 1200f);
            item.consumable = false;
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (user && user.CurrentRoom != null)
            {
                return base.CanBeUsed(user);
            }
            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            if (user.CurrentRoom != null && user)
            {
                IntVector2 backupPos = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3);
                if (user.IsValidPlayerPosition(user.specRigidbody.UnitTopCenter.ToIntVector2().ToVector2()))
                {
                    GameManager.Instance.RewardManager.SpawnTotallyRandomChest(user.specRigidbody.UnitTopCenter.ToIntVector2());
                } else
                {
                    GameManager.Instance.RewardManager.SpawnTotallyRandomChest(backupPos);
                }
            }
        }
    }
}
