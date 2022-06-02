using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class TwoOfHearts : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Two of Hearts";
            string resourceName = "katmod/Resources/V3MiscItems/2ofhearts";
            GameObject obj = new GameObject();
            TwoOfHearts item = obj.AddComponent<TwoOfHearts>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Have a heart";
            string longDesc = "Increases the chance for hearts and armor to spawn from room clear, increases coolness.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
            item.AddPassiveStatModifier(PlayerStats.StatType.Coolness, 2);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            foreach (FloorRewardData rewardData in GameManager.Instance.RewardManager.FloorRewardData)
            {
                if (rewardData.SingleItemRewardTable.defaultItemDrops.elements != null && rewardData.SingleItemRewardTable.defaultItemDrops.elements.Count != 0)
                {
                    foreach (WeightedGameObject weightedObject in rewardData.SingleItemRewardTable.defaultItemDrops.elements)
                    {
                        if (weightedObject.gameObject && weightedObject.gameObject.GetComponent<HealthPickup>())
                        {
                            weightedObject.weight *= 2;
                        }
                    }
                }
            }
        }

        public override DebrisObject Drop(PlayerController player)
        {
            foreach (FloorRewardData rewardData in GameManager.Instance.RewardManager.FloorRewardData)
            {
                if (rewardData.SingleItemRewardTable.defaultItemDrops.elements != null && rewardData.SingleItemRewardTable.defaultItemDrops.elements.Count != 0)
                {
                    foreach (WeightedGameObject weightedObject in rewardData.SingleItemRewardTable.defaultItemDrops.elements)
                    {
                        if (weightedObject.gameObject && weightedObject.gameObject.GetComponent<HealthPickup>())
                        {
                            weightedObject.weight /= 2;
                        }
                    }
                }
            }
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            foreach (FloorRewardData rewardData in GameManager.Instance.RewardManager.FloorRewardData)
            {
                if (rewardData.SingleItemRewardTable.defaultItemDrops.elements != null && rewardData.SingleItemRewardTable.defaultItemDrops.elements.Count != 0)
                {
                    foreach (WeightedGameObject weightedObject in rewardData.SingleItemRewardTable.defaultItemDrops.elements)
                    {
                        if (weightedObject.gameObject && weightedObject.gameObject.GetComponent<HealthPickup>())
                        {
                            weightedObject.weight /= 2;
                        }
                    }
                }
            }
            base.OnDestroy();
        }
    }
}
