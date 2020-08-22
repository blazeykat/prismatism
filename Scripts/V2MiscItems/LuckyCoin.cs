using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using Gungeon;
using UnityEngine;

namespace katmod
{
    class LuckyCoin : PassiveItem
    {
        public static void Init()
        {
            string name = "Luckful Coin";
            string resourcePath = "katmod/Resources/V2MiscItems/luckycoin.png";
            GameObject gameObject = new GameObject(name);
            LuckyCoin item = gameObject.AddComponent<LuckyCoin>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "50/50?";
            string longDesc = "Enemies have a chance to drop casings on damage.\n\nLucky coin was taken by NN's items so it is now luckful.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.A;
            item.SetupUnlockOnFlag(GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET, true);
            item.PlaceItemInAmmonomiconAfterItemById(272);
        }
        private void PostProcessBeam(BeamController sourceBeam)
        {
            try
            {
                Projectile projectile = sourceBeam.projectile;
                projectile.OnHitEnemy += this.OnHitEnemy; 
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
        {
            try
            {
                sourceProjectile.OnHitEnemy += this.OnHitEnemy;
            }
            catch (Exception ex)
            {
                global::ETGModConsole.Log(ex.Message, false);
            }
        }
        private void OnHitEnemy(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
        {
            if (arg2 != null && arg2.aiActor != null && Owner != null)
            {
                if (Utilities.UncoolRandom(0.985f))
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject,arg2.UnitCenter, Vector2.zero, 1f, false, true, false);
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeam += this.PostProcessBeam;
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject debrisObject = base.Drop(player);
            try
            {
                player.PostProcessBeam -= this.PostProcessBeam;
                player.PostProcessProjectile -= this.PostProcessProjectile;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log($"damn,\n {ex}");
            }
            debrisObject.GetComponent<LuckyCoin>().m_pickedUpThisRun = true;
            return debrisObject;
        }

    }
}
