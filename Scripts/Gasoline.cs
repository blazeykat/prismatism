using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;

namespace katmod
{
    class Gasoline : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Gasoline";
            string resourceName = "katmod/Resources/gasoline";
            GameObject obj = new GameObject();
            var item = obj.AddComponent<Gasoline>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Delicious";
            string longDesc = "Killed enemies spawn pools of fire.\n\nWhile gasoline itself is not safe to drink, in an emergency, it can be used to cook food.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            Gasoline.goopDefs = new List<GoopDefinition>();
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            item.quality = PickupObject.ItemQuality.A;
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_MINI_FUSELIER, true);
            item.PlaceItemInAmmonomiconAfterItemById(440);
            foreach (string text in Gasoline.goops)
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
                Gasoline.goopDefs.Add(goopDefinition);
            }
            List<string> mandatoryConsoleIDs = new List<string>
            {
                "psm:gasoline",
                "ring_of_fire_resistance"
            };
            CustomSynergies.Add("Fireproof", mandatoryConsoleIDs, null, true);
        }

        private void OnEnemyKilled(PlayerController player, HealthHaver enemy)
        {
            if (enemy.specRigidbody != null && enemy.aiActor != null && base.Owner != null)
            {
                PickupObject byId = PickupObjectDatabase.GetById(310);
                GoopDefinition goopDefinition;
                if (byId == null)
                {
                    goopDefinition = null;
                }
                else
                {
                    WingsItem component = byId.GetComponent<WingsItem>();
                    goopDefinition = ((component != null) ? component.RollGoop : null);
                }
                GoopDefinition goopDef = goopDefinition;
                float duration = 0.75f;
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(Gasoline.goopDefs[0]).TimedAddGoopCircle(enemy.specRigidbody.UnitCenter, player.HasMTGConsoleID("psm:warrior's_syringe") || player.HasMTGConsoleID("psm:boiling_fungus") ? 5f : 3f, duration, false);
            }
        }
        public GameActorFireEffect EnemyDebuff = new GameActorFireEffect
        {
            DamagePerSecondToEnemies = 2,
            AppliesTint = true,
            TintColor = UnityEngine.Color.red,
            FlameVfx = default,
            duration = 20,
        };
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += this.OnEnemyKilled;
            this.m_fireImmunity = new DamageTypeModifier();
            this.m_fireImmunity.damageMultiplier = 0f;
            this.m_fireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            player.OnKilledEnemyContext -= this.OnEnemyKilled;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            debrisObject.GetComponent<Gasoline>().m_pickedUpThisRun = true;
            return debrisObject;
        }
        protected override void OnDestroy()
        {
            Owner.healthHaver.damageTypeModifiers.Remove(m_fireImmunity);
            Owner.OnKilledEnemyContext -= OnEnemyKilled;
            base.OnDestroy();
        }

        private DamageTypeModifier m_fireImmunity;

        public float Radius = 0.75f;

        public static List<GoopDefinition> goopDefs;

        private static string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset"
        };

        public static List<string> necessary = new List<string>
        {
            "psm:gasoline"
        };

        public static List<string> optional = new List<string>
        {
            "psm:warrior's_syringe",
            "psm:boiling_fungus"
        };
    }
}
