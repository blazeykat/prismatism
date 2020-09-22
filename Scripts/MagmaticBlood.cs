using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using Gungeon;
using System.Collections;

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
            MagmaticBlood.goopDefs = new List<GoopDefinition>();
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            item.quality = PickupObject.ItemQuality.C;
            item.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            foreach (string text in MagmaticBlood.goops)
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
                MagmaticBlood.goopDefs.Add(goopDefinition);
            }
            item.PlaceItemInAmmonomiconAfterItemById(313);
        }
        private void DoLiquidEffect(PlayerController player)
        {
            this.ForceGoop(player);
            
        }
        public void ForceGoop(PlayerController player)
        {
            DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(MagmaticBlood.goopDefs[0]).TimedAddGoopCircle(player.specRigidbody.UnitCenter, this.Radius, 1f, false);
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
            Owner.OnReceivedDamage -= this.DoLiquidEffect;
            Owner.healthHaver.damageTypeModifiers.Remove(this.m_fireImmunity);
            base.OnDestroy();
        }

        private readonly DamageTypeModifier m_fireImmunity = new DamageTypeModifier()
        {
            damageMultiplier = 0,
            damageType = CoreDamageTypes.Fire
        };

        public float Radius = 10f;

        private static List<GoopDefinition> goopDefs;

        private static readonly string[] goops = new string[]
        {
            "assets/data/goops/napalmgoopthatworks.asset"
        };
    }
}
