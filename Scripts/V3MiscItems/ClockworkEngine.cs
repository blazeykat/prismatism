using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace katmod
{
    class ClockworkEngine : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Clockwork Dynamo";
            string resourceName = "katmod/Resources/V3MiscItems/smallerclockwork";
            GameObject obj = new GameObject(itemName);
            ClockworkEngine item = obj.AddComponent<ClockworkEngine>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Fully Charged, Doc";
            string longDesc = "Increases all electricity damage you do.\n\nI know it doesn't really like look a clockwork dynamo, but neither does the thing it's referencing so don't sue me.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.D;
            item.PlaceItemInAmmonomiconAfterItemById(454);
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_DRAGUN_WITH_ROBOT, true);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            ETGMod.AIActor.OnPreStart += FeelPainLol;
            player.healthHaver.damageTypeModifiers.Add(nullifier);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            ETGMod.AIActor.OnPreStart -= FeelPainLol;
            player.healthHaver.damageTypeModifiers.Remove(nullifier);

            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            ETGMod.AIActor.OnPreStart -= FeelPainLol;
            base.Owner.healthHaver.damageTypeModifiers.Remove(nullifier);
            base.OnDestroy();
        }

        public void FeelPainLol(AIActor bezos)
        {
            bezos.healthHaver.damageTypeModifiers.Add(amplifier);
        }

        DamageTypeModifier amplifier = new DamageTypeModifier
        {
            damageMultiplier = 2f,
            damageType = CoreDamageTypes.Electric
        };

        DamageTypeModifier nullifier = new DamageTypeModifier
        {
            damageMultiplier = 0f,
            damageType = CoreDamageTypes.Electric
        };

    }
}
