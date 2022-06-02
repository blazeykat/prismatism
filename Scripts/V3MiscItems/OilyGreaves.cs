using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MultiplayerBasicExample;

namespace katmod
{
    class OilyGreaves : PegasusBootsItem
    {
        public static void Init()
        {
            string name = "Oily Greaves";
            string resourceName = "katmod/Resources/V3MiscItems/galoshes";
            GameObject gaming = new GameObject(name);
            OilyGreaves item = gaming.AddComponent<OilyGreaves>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gaming);
            string shortDesc = "BelowMine";
            string longDesc = "Spawns a puddle of oil upon rolling.\n\nOil cannot be consumed for sustenance. trust me, I've tried.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(163);
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            OilyGreaves.goopDefs = new List<GoopDefinition>();
            foreach (string text in OilyGreaves.goops)
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

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.healthHaver.damageTypeModifiers.Add(fireimmunity);
            player.OnIsRolling += HandleRollFrame;
            player.OnPreDodgeRoll += PreRoll;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.healthHaver.damageTypeModifiers.Remove(fireimmunity);
            player.OnIsRolling -= HandleRollFrame;
            player.OnPreDodgeRoll -= PreRoll;
            return base.Drop(player);
        }

        public void HandleRollFrame(PlayerController yaboi)
        {
            if (yaboi.CurrentRollState == PlayerController.DodgeRollState.OnGround && !rollCheck)
            {
                DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefs[0]).TimedAddGoopCircle(yaboi.sprite.WorldCenter, 3, 0.125f);
                rollCheck = true;
            }
        }

        public void PreRoll(PlayerController player)
        {
            rollCheck = false;
        }

        private readonly DamageTypeModifier fireimmunity = new DamageTypeModifier
        {
            damageMultiplier = 0,
            damageType = CoreDamageTypes.Fire
        };

        private static List<GoopDefinition> goopDefs;

        private static readonly string[] goops = new string[]
        {
            "assets/data/goops/oil goop.asset"
        };

        private bool rollCheck = false;
    }
}
