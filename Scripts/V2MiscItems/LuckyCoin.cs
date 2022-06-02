using ItemAPI;
using System;
using UnityEngine;

namespace katmod
{
    class LuckyCoin : PassiveItem
    {
        public static void Init()
        {
            string name = "Enchanted Coin";
            string resourcePath = "katmod/Resources/V2MiscItems/luckycoin.png";
            GameObject gameObject = new GameObject(name);
            LuckyCoin item = gameObject.AddComponent<LuckyCoin>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "50/50?";
            string longDesc = "Enemies have a chance to drop casings on damage.\n\nUsed by pirate captain's, to make a profit from pillaging.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.SetupUnlockOnCustomMaximum(CustomTrackedMaximums.MOST_MONEY, 149.9f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Have more than 150 casings in one run.");
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
            if (arg2 != null && arg2.aiActor != null && Owner != null && arg2.healthHaver && arg2.healthHaver.IsVulnerable)
            {
                if (BoxOTools.BasicRandom(0.97f))
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(68).gameObject, arg2.UnitCenter, Vector2.zero, 1f, false, true, true);
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
            player.PostProcessBeam -= this.PostProcessBeam;
            player.PostProcessProjectile -= this.PostProcessProjectile;
            return base.Drop(player);
        }

    }
}
