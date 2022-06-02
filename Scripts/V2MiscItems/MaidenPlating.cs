using ItemAPI;
using UnityEngine;

namespace katmod
{

    internal class MaidenPlating : PassiveItem
    {

        public static void Init()
        {
            string name = "Maiden Plating";
            string resourcePath = "katmod/Resources/V2MiscItems/maidenplating";
            GameObject gameObject = new GameObject(name);
            MaidenPlating item = gameObject.AddComponent<MaidenPlating>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Ruined";
            string longDesc = "Shoots 8 lead maiden projectiles upon taking damage.\n\nNow *you* can be the game ruiner!";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(564);

            Projectile projectile2 = ((Gun)ETGMod.Databases.Items[58]).DefaultModule.projectiles[0];
            Projectile proj1 = UnityEngine.Object.Instantiate<Projectile>(projectile2);
            {
                proj1.baseData.damage = 25;
                proj1.SetProjectileSpriteRight("friendly_maiden_spear_001", 23, 10);
                proj1.gameObject.SetActive(false);
                proj1.baseData.speed = 5;
                UnityEngine.Object.DontDestroyOnLoad(proj1);
                FakePrefab.MarkAsFakePrefab(proj1.gameObject);
            }
            spearBullet = proj1;
        }

        public static Projectile spearBullet;

        private void OnPlayerHit(PlayerController player)
        {
            for (int counter = 0; counter < 8; counter++)
            {
                GameObject gameObject = SpawnManager.SpawnProjectile(spearBullet.gameObject, player.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (45 * counter)), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                if (component != null)
                {
                    component.Owner = player;
                    component.Shooter = player.specRigidbody;
                    projectile.gameObject.AddComponent(new HomingModifier() { HomingRadius = 720, AngularVelocity = 180 });
                    player.DoPostProcessProjectile(component);
                }
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += this.OnPlayerHit;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= this.OnPlayerHit;
            return base.Drop(player);
        }
    }
}
