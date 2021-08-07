using ItemAPI;
using UnityEngine;

namespace katmod
{
    class ChainOfBeing : PlayerItem
    {
        public static void Init()
        {
            string itemName = "great chain of ballin";
            string resourceName = "katmod/Resources/V3MiscItems/jarfullofsouls";
            GameObject obj = new GameObject(itemName);
            ChainOfBeing item = obj.gameObject.AddComponent<ChainOfBeing>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Disrupt the Flow";
            string longDesc = "placeholder text";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.D;
            item.SetCooldownType(ItemBuilder.CooldownType.Timed, 0);
            item.consumable = false;

            GameObject vfx = new GameObject("chainvfx");
            SpriteBuilder.SpriteFromResource("katmod/Resources/V3MiscItems/epicplaceholdersprite", vfx);
            vfxPrefab = vfx;

            GameActorChainedEffect chained = new GameActorChainedEffect()
            {
                SpeedMultiplier = 0,
                CooldownMultiplier = 0,
                duration = 8,
                OverheadVFX = vfxPrefab,
                PlaysVFXOnActor = true
            };
            effect = chained;
        }

        public static GameActorChainedEffect effect;
        public static GameObject vfxPrefab;

        protected override void DoEffect(PlayerController user)
        {
            base.DoEffect(user);
            AIActor actor = user.CurrentRoom.GetNearestEnemy(user.specRigidbody.UnitCenter, out _, false);
            actor.ApplyEffect(effect);
        }

        public class GameActorChainedEffect : GameActorSpeedEffect
        {
            public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
            {
                base.OnEffectApplied(actor, effectData, partialAmount);
                float MathIsCool = actor.specRigidbody.UnitWidth / (effectData.instanceOverheadVFX.GetBounds().extents.x * 16);
                effectData.instanceOverheadVFX.transform.localScale = new Vector3(MathIsCool, MathIsCool);
            }
        }
    }
}
