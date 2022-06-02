using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class HeartyKey : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Hearty Key";
            string resourceName = "katmod/Resources/V3MiscItems/heartykey";
            GameObject obj = new GameObject();
            HeartyKey item = obj.AddComponent<HeartyKey>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Healthey";
            string longDesc = "Opening chests gives you hearts.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(166);
            item.SetupUnlockOnCustomMaximum(CustomTrackedMaximums.MAXIMUM_HEALTH, 6.99f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Have more than 7 heart containers in one run");
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
        }

        public override void Pickup(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen += Chnest;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            ETGMod.Chest.OnPostOpen -= Chnest;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            ETGMod.Chest.OnPostOpen -= Chnest;
            base.OnDestroy();
        }

        public void Chnest(Chest chest, PlayerController player)
        {
            if (player.healthHaver)
            {
                AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", base.gameObject);
                if (player.characterIdentity != PlayableCharacters.Robot) { player.healthHaver.ApplyHealing(1f); } else { player.healthHaver.Armor += 1; }
                player.BloopItemAboveHead(this.sprite);
            }
        }
    }
}
