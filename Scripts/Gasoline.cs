using ItemAPI;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class Gasoline : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Gasoline";
            string resourceName = "katmod/Resources/gasoline";
            GameObject obj = new GameObject();
            Gasoline item = obj.AddComponent<Gasoline>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Delicious";
            string longDesc = "Killed enemies spawn pools of fire.\n\nWhile gasoline itself is not safe to drink, in an emergency, it can be used to cook food.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            Gasoline.goopDefs = new List<GoopDefinition>();
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            item.quality = PickupObject.ItemQuality.B;
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_MINI_FUSELIER, true);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Kill fuselier");
            item.PlaceItemInAmmonomiconAfterItemById(452);

            foreach (string text in goops)
            {
                GoopDefinition goopDefinition;
                try
                {
                    GameObject gameObject2 = assetBundle.LoadAsset(text) as GameObject;
                    goopDefinition = gameObject2.GetComponent<GoopDefinition>();
                }
                catch
                {
                    goopDefinition = (assetBundle.LoadAsset(text) as GoopDefinition);
                }
                goopDefinition.name = text.Replace("assets/data/goops/", "").Replace(".asset", "");
                goopDefs.Add(goopDefinition);
            }
        }

        private void OnEnemyKilled(PlayerController player, HealthHaver enemy)
        {
            if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null)
            {
                float duration = 0.75f;
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]).TimedAddGoopCircle(enemy.specRigidbody.UnitCenter, player.PlayerHasActiveSynergy("Fireproof") ? (player.PlayerHasActiveSynergy("Rainpour") ? 8f : 6f) : player.PlayerHasActiveSynergy("Rainpour") ? 6f : 4f, duration, false);
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += this.OnEnemyKilled;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnKilledEnemyContext -= this.OnEnemyKilled;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.healthHaver.damageTypeModifiers.Remove(m_fireImmunity);
                Owner.OnKilledEnemyContext -= OnEnemyKilled;
            }
            base.OnDestroy();
        }

        private DamageTypeModifier m_fireImmunity = new DamageTypeModifier
        {
            damageMultiplier = 0f,
            damageType = CoreDamageTypes.Fire
        };

        public float Radius = 0.75f;

        public static List<GoopDefinition> goopDefs;

        private static readonly string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset"
        };
    }
}
