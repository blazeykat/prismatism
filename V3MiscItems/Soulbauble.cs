using ItemAPI;
using UnityEngine;

namespace katmod
{
    class Soulbauble : PassiveItem
    {
        public static void Init()
        {
            string name = "Soulbauble";
            string resourcePath = "katmod/Resources/V3MiscItems/soulstone";
            GameObject gameObject = new GameObject(name);
            Soulbauble item = gameObject.AddComponent<Soulbauble>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Blood for Blood";
            string longDesc = "Has a chance to decrease enemy health drastically. Chance increase with less HP.\n\nAn ancient artifact used by necromancers in training, to hone their abilities.";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Curse, 3f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            item.quality = ItemQuality.S;
            item.PlaceItemInAmmonomiconAfterItemById(525);
        }

        public override void Pickup(PlayerController player)
        {
            ETGMod.AIActor.OnPostStart += MoreItems;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            ETGMod.AIActor.OnPostStart -= MoreItems;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            ETGMod.AIActor.OnPostStart -= MoreItems;
            base.OnDestroy();
        }

        public void MoreItems(AIActor actor)
        {
            if (actor && actor.healthHaver)
            {
                float ChanceToActivate = 0.5f;
                if (Owner.characterIdentity != PlayableCharacters.Robot || Owner.healthHaver.GetMaxHealth() <= 0)
                {
                    ChanceToActivate = Owner.healthHaver.GetCurrentHealthPercentage();
                }
                if (BoxOTools.BasicRandom(ChanceToActivate))
                {
                    float BaseHealth = actor.healthHaver.GetMaxHealth();
                    actor.healthHaver.SetHealthMaximum(BaseHealth * 0.5f, null, true);
                }
            }
        }
    }
}
