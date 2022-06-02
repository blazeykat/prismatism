using ItemAPI;
using System.Collections;
using UnityEngine;

namespace katmod
{
    class SaplingBullets : PassiveItem
    {
        public static void Init()
        {
            string name = "Sapling Bullets";
            string resourcePath = "katmod/Resources/seedbullet";
            GameObject gameObject = new GameObject(name);
            SaplingBullets item = gameObject.AddComponent<SaplingBullets>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Sap-Fling";
            string longDesc = "Shoots a leaf along with each shot.\n\nThese seeds are planted inside of the bullets, and they feed upon gunpowder to grow. The speeds of the bullets cause them to soak up more gunpowder and grow quicker.\n\n\"Oh, please. Did you really think you'd be that lucky?\"";
            ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, -0.1f, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = PickupObject.ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(640);
        }

        private void PostProcessProjectile(Projectile projectile, float Chance)
        {
            if (BoxOTools.BasicRandom(Owner.PlayerHasActiveSynergy("Mahoguny Sapling") ? 0.6f : 0.75f) && CoolAsIce && Owner)
            {
                ShootLeafShit();
            }
        }

        private void ShootLeafShit()
        {
            CoolAsIce = false;
            StartCoroutine(StartCooldown());
            Projectile component = Owner.HandleProjectile(20f, Owner.PlayerHasActiveSynergy("Leaf Orb") ? 9f : 7f, 620, true, Vector2.zero, true);
            projectile.gameObject.AddComponent(new HomingModifier() { HomingRadius = 360, AngularVelocity = 270 });
        }


        private void PostProcessBeamChanceTick(BeamController beamController)
        {
            if (BoxOTools.BasicRandom(Owner.PlayerHasActiveSynergy("Mahoguny Sapling") ? 0.6f : 0.75f) && CoolAsIce)
            {
                ShootLeafShit();
            }
        }


        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
            SaplingBullets.CoolAsIce = true;
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

        private static bool CoolAsIce = true;
    }
}
