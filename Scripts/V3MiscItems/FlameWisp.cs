using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using UnityEngine;

namespace katmod
{
    class FlameWisp : PassiveItem
    {
        public static void Init()
        {
            string name = "Flame Wisp Piece";
            string resource = "katmod/Resources/V3MiscItems/firewisp";
            GameObject obj = new GameObject(name);
            FlameWisp item = obj.AddComponent<FlameWisp>();
            ItemBuilder.AddSpriteToObject(name, resource, obj);
            string shortd = "blazey";
            string longd = "Explosions cause fire.\n\nA piece of a dead flame wisp.\n\n There's still some life within the piece of the rock. Maybe with some fuel it could regrow.";
            ItemBuilder.SetupItem(item, shortd, longd);
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(452);
            Hook hook = new Hook(typeof(Exploder).GetMethod("DoExplode", BindingFlags.Instance | BindingFlags.NonPublic), typeof(FlameWisp).GetMethod("Hawt"));
        }

        public static void Hawt(CoolerAction<Exploder, Vector3, ExplosionData, Vector2, Action, bool, CoreDamageTypes, bool> action, Exploder explode, Vector3 vector3, ExplosionData data, Vector2 vector2, Action deathaction, bool queue, CoreDamageTypes damageType, bool ahh)
        {
            if (GameManager.Instance.PrimaryPlayer.HasMTGConsoleID("psm:flame_wisp") || (GameManager.Instance.SecondaryPlayer && GameManager.Instance.SecondaryPlayer.HasMTGConsoleID("psm:flame_wisp")))
            {
                action(explode, vector3, data, vector2, deathaction, queue, CoreDamageTypes.Fire, ahh);
            }
            else
            {
                action(explode, vector3, data, vector2, deathaction, queue, damageType, ahh);
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.healthHaver.damageTypeModifiers.Add(m_fireImmunity);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.healthHaver.damageTypeModifiers.Remove(m_fireImmunity);
            return base.Drop(player);
        }

        private readonly DamageTypeModifier m_fireImmunity = new DamageTypeModifier
        {
            damageMultiplier = 0f,
            damageType = CoreDamageTypes.Fire
        };

        public delegate void CoolerAction<in T1, in t2, in t3, in t4, in t5, in t6, in t7, in t8>(T1 a, t2 b, t3 c, t4 d, t5 e, t6 f, t7 g, t8 h);
    }
}
