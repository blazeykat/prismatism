using ItemAPI;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class MagmaticBlood : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Molten Blood";
            string resourceName = "katmod/Resources/V2MiscItems/moltenblood.png";
            GameObject obj = new GameObject();
            MagmaticBlood item = obj.AddComponent<MagmaticBlood>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Turn counter clockwise";
            string longDesc = "Emits deadly fire when damaged.\n\nA form of gasoline, safe for both consumption and injection.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            goopDefs = new List<GoopDefinition>();
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            item.quality = ItemQuality.EXCLUDED;
            item.AddPassiveStatModifier(PlayerStats.StatType.Health, 1);
            item.RemovePickupFromLootTables();

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
            item.PlaceItemInAmmonomiconAfterItemById(313);

            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[12]).DefaultModule.projectiles[0];
            Projectile proj1 = Instantiate(projectile2);
            {
                proj1.baseData.damage = 12;
                proj1.SetProjectileSpriteRight("bloodyspear_001", 19, 6);
                proj1.gameObject.SetActive(false);
                proj1.baseData.speed = 16;
                Object.DontDestroyOnLoad(proj1);
                FakePrefab.MarkAsFakePrefab(proj1.gameObject);
            }
            bloodySpear = proj1;
        }

        private void DoLiquidEffect(PlayerController player)
        {
            if (player.PlayerHasActiveSynergy("Blood Types"))
            {
                for (int counter = 0; counter < 4; counter++)
                {
                    GameObject gameObject = SpawnManager.SpawnProjectile(bloodySpear.gameObject, player.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, 90 * counter), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    if (component != null)
                    {
                        component.Owner = Owner;
                        component.AddHoming(720, 480);
                    }
                }
            }
            else
            {
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]).TimedAddGoopCircle(player.specRigidbody.UnitCenter, Radius, 1f, false);
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.DoLiquidEffect;
            player.healthHaver.damageTypeModifiers.Add(this.m_fireImmunity);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= this.DoLiquidEffect;
            player.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (Owner)
            {
                Owner.OnReceivedDamage -= this.DoLiquidEffect;
                Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            }
            base.OnDestroy();
        }

        private readonly DamageTypeModifier m_fireImmunity = new DamageTypeModifier()
        {
            damageMultiplier = 0,
            damageType = CoreDamageTypes.Fire
        };

        private static float Radius = 10f;

        private static List<GoopDefinition> goopDefs;

        private static readonly string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset"
        };

        private static Projectile bloodySpear;
    }
}
