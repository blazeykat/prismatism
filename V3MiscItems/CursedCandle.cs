using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class CursedCandle : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Cursed Candle";
            string resourceName = "katmod/Resources/V3MiscItems/cursedcandle2";
            GameObject obj = new GameObject();
            CursedCandle item = obj.AddComponent<CursedCandle>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Constant Heat";
            string longDesc = "Randomly spawns pools of fire whilst in combat.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(65);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnEnteredCombat += OnRoomStart;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnEnteredCombat -= OnRoomStart;
            return base.Drop(player);
        }

        private void OnRoomStart()
        {
            PostRoomStart = false;
            StartCoroutine(PreRoomStart());
        }

        protected override void Update()
        {
            base.Update();
            if (Owner != null && Owner.IsInCombat && PostRoomStart && OnCooldown)
            {
                OnCooldown = false;
                StartCoroutine(BasicBoolDown());
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Gasoline.goopDefs[0]).TimedAddGoopCircle((Vector2)Owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1), 2.25f);
            }
        }

        private bool OnCooldown = true;

        private bool PostRoomStart = true;

        public IEnumerator BasicBoolDown()
        {
            yield return new WaitForSeconds(1f);
            OnCooldown = true;
            yield break;
        }

        public IEnumerator PreRoomStart()
        {
            yield return new WaitForSeconds(3f);
            PostRoomStart = true;
            yield break;
        }

        public DamageTypeModifier firey = new DamageTypeModifier
        {
            damageMultiplier = 0,
            damageType = CoreDamageTypes.Fire
        };
    }
}
