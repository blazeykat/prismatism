using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Kebab : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Kebab";
            string resourceName = "katmod/Resources/V3MiscItems/godrounds";
            GameObject obj = new GameObject();
            Kebab item = obj.AddComponent<Kebab>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Stick on a Meat";
            string longDesc = "Increases damage by a lot.\n\nMade from the rare mineral Mythril.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnNewFloorLoaded += NewFloor;
        }

        public void NewFloor(PlayerController player)
        {
            ThanksGaming = !ThanksGaming;
            if (ThanksGaming)
            {
                if (player.characterIdentity != PlayableCharacters.Robot) { player.healthHaver.ApplyHealing(1f); } else { player.healthHaver.Armor += 1; }
            }
        }

        private bool ThanksGaming;
    }
}
