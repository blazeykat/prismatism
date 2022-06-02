using ItemAPI;
using UnityEngine;

namespace katmod
{
    class MythrilBullets : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Mythril Rounds";
            string resourceName = "katmod/Resources/V3MiscItems/godrounds";
            GameObject obj = new GameObject();
            MythrilBullets item = obj.AddComponent<MythrilBullets>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Hardened";
            string longDesc = "Increases damage by a lot.\n\nMade from the rare mineral Mythril.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 0.5f, StatModifier.ModifyMethod.ADDITIVE);
            item.PlaceItemInAmmonomiconAfterItemById(528);
            item.SetupUnlockOnCustomMaximum(CustomTrackedMaximums.MAXIMUM_DAMAGE, 2.249f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Have more than 2.25 damage in one run");
        }
    }
}
