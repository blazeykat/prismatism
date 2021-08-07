using ItemAPI;
using System.Collections;
using UnityEngine;

namespace katmod
{
    class EnderHelmet : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Ender Helmet";
            string resourceName = "katmod/Resources/V3MiscItems/enderhelmet";
            GameObject obj = new GameObject();
            EnderHelmet item = obj.AddComponent<EnderHelmet>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "SkyBlocked";
            string longDesc = "Teleports at enemies upon killing them, gives temporary invincibilty upon killing.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.PlaceItemInAmmonomiconAfterItemById(312);
            item.quality = ItemQuality.B;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            if (afterImage == null)
            {
                afterImage = player.gameObject.AddComponent<ImprovedAfterImage>();
                afterImage.spawnShadows = false;
                afterImage.dashColor = Color.magenta;
                afterImage.shadowLifetime = 1f;
            }
            player.OnKilledEnemyContext += Player_OnKilledEnemyContext;
        }

        private ImprovedAfterImage afterImage;

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemyContext -= Player_OnKilledEnemyContext;
            return base.Drop(player);
        }

        private void Player_OnKilledEnemyContext(PlayerController player, HealthHaver arg2)
        {
            if (arg2 && arg2.specRigidbody && player && player.CurrentRoom != null)
            {
                Vector2 position = arg2.specRigidbody.UnitCenterRight;
                Kills++;
                if (player.IsValidPlayerPosition(position))
                {
                    player.WarpToPoint(position);
                    LootEngine.DoDefaultItemPoof(position);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (m_owner && Kills > 0)
            {
                if (Duration < Kills)
                {
                    afterImage.spawnShadows = true;
                    m_owner.baseFlatColorOverride = Color.magenta;
                    m_owner.healthHaver.IsVulnerable = false;
                } else
                {
                    Kills = 0;
                    Duration = 0;
                    m_owner.healthHaver.IsVulnerable = true;
                    m_owner.baseFlatColorOverride.a = 0;
                    afterImage.spawnShadows = false;
                }
                Duration += BraveTime.DeltaTime;
            }
        }
        private static float Kills;

        private static float Duration;
    }
}
