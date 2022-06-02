using ItemAPI;
using System.Collections;
using UnityEngine;

namespace katmod
{

    internal class PlagueBullets : GunVolleyModificationItem
    {

        public static void Init()
        {
            string name = "Plague Bullets";
            string resourcePath = "katmod/Resources/V2MiscItems/plaguebullets";
            GameObject gameObject = new GameObject(name);
            PlagueBullets item = gameObject.AddComponent<PlagueBullets>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Green Death";
            string longDesc = "Has a chance to shoot a bouncing plague bullet on fire.\n\nTheir cure is most efficient.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(640);
        }

        private void PostProcessProjectile(Projectile projectile, float Chance)
        {
            if (BoxOTools.BasicRandom(0.7f) && !CoolAsIce)
            {
                ShootPlagueShit();
            }
        }


        private void PostProcessBeamChanceTick(BeamController beamController)
        {
            if (BoxOTools.BasicRandom(0.7f) && !CoolAsIce)
            {
                ShootPlagueShit();
            }
        }

        private void ShootPlagueShit()
        {
            CoolAsIce = true;
            StartCoroutine(StartCooldown());
            for (int counter = 0; counter < (Owner.PlayerHasActiveSynergy("Plaguebringer Goliath") ? 3 : 1); counter++)
            {
                Projectile component = Owner.HandleProjectile(20f, 10f, 207, true, Vector2.zero, false, UnityEngine.Random.Range(0.0f, 360.0f), 250);
                BounceProjModifier bouncy = component.gameObject.AddComponent<BounceProjModifier>();
                bouncy.numberOfBounces = 10;
                bouncy.onlyBounceOffTiles = true;
            }
        }


        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
            this.CoolAsIce = false;
            yield break;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.PostProcessProjectile += this.PostProcessProjectile;
            player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.PostProcessProjectile -= this.PostProcessProjectile;
            player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
            return base.Drop(player);
        }

        private bool CoolAsIce = false;

    }
}
