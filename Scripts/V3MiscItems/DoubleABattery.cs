using ItemAPI;
using UnityEngine;

namespace katmod
{
    class DoubleABattery : PassiveItem
    {
        public static void Init()
        {
            string itemName = "AA Battery";
            string resourceName = "katmod/Resources/V3MiscItems/AAbattery";
            GameObject obj = new GameObject();
            DoubleABattery item = obj.AddComponent<DoubleABattery>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Shocking";
            string longDesc = "Charges your active item/s upon room clear.\n\nJump start your actives using the power of Zeus himself.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(119);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRoomClearEvent += ChargeActives;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRoomClearEvent -= ChargeActives;
            return base.Drop(player);
        }

        private void ChargeActives(PlayerController obj)
        {
            if (obj && obj.activeItems.Count > 0)
            {
                foreach (PlayerItem item in obj.activeItems)
                {
                    if (item.IsOnCooldown)
                    {
                        item.CurrentDamageCooldown = Mathf.Max(0, item.CurrentDamageCooldown - 150);
                        if (BoxOTools.BasicRandom(0.66f))
                        {
                            item.CurrentRoomCooldown = Mathf.Max(0, item.CurrentRoomCooldown - 1);
                        }
                    }
                }
            }
        }
    }
}
