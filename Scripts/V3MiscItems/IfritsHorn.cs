using ItemAPI;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class IfritsHorn : PassiveItem
    {
        public static void Init()
        {
            string name = "Infernal Horn";
            string resourcePath = "katmod/Resources/V3MiscItems/ifritshorn";
            GameObject gameObject = new GameObject(name);
            IfritsHorn item = gameObject.AddComponent<IfritsHorn>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "The Infernal King";
            string longDesc = "Has a chance to make your projectiles spawn fiery goop on destruction.\n\nA horn, ripped straight from a flaming beast from a far off planet";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;

            goopDefs = new List<GoopDefinition>();
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
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

            item.PlaceItemInAmmonomiconAfterItemById(452);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += PostProcessProjectile;
            player.healthHaver.damageTypeModifiers.Add(m_fireImmunity);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= PostProcessProjectile;
            player.healthHaver.damageTypeModifiers.Remove(m_fireImmunity);
            return base.Drop(player);
        }

        private void PostProcessProjectile(Projectile projectile, float arg2)
        {
            if (projectile && BoxOTools.BasicRandom(0.85f))
            {
                projectile.AdjustPlayerProjectileTint(Color.red, 5);
                projectile.OnDestruction += PushinPeopleToTheGround;
            }
        }

        private void PushinPeopleToTheGround(Projectile projectile)
        {
            if (projectile)
            {
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]).TimedAddGoopCircle(projectile.specRigidbody.UnitCenter, 2f);
            }
        }

        private DamageTypeModifier m_fireImmunity = new DamageTypeModifier
        {
            damageMultiplier = 0f,
            damageType = CoreDamageTypes.Fire
        };

        private static List<GoopDefinition> goopDefs;

        private static readonly string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset"
        };
    }
}
