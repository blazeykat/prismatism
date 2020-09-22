using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;


namespace katmod
{
    class HoodedShells : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Hooded Shells";
            string resourceName = "katmod/Resources/V2MiscItems/hoodedshells";
            GameObject obj = new GameObject();
            HoodedShells item = obj.AddComponent<HoodedShells>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Giant Slayer IV";
            string longDesc = "Does more damage to enemies with higher max HP.\n\nThe bigger they are, the harder they fall.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.A;
            item.SetupUnlockOnFlag(GungeonFlags.BOSSKILLED_DEMONWALL, true);
            item.PlaceItemInAmmonomiconAfterItemById(323);
        }

        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.specRigidbody.OnPreRigidbodyCollision += OnHitEnemy;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            if (otherRigidbody != null && otherRigidbody.aiActor != null && myRigidbody != null && myRigidbody.projectile && otherRigidbody.aiActor.healthHaver)
            {
                float hpercent = otherRigidbody.aiActor.healthHaver.GetMaxHealth() / 600;
                if (hpercent > 0.6f) { hpercent = 0.6f; }
                myRigidbody.projectile.baseData.damage *= 1 + hpercent;
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }

    }

}
