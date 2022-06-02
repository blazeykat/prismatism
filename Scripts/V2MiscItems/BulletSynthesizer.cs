using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class BulletSynthesizer : PassiveItem
    {
        public static void Init()
        {
            string name = "Bullet Synthesizer";
            string resourcePath = "katmod/Resources/V2MiscItems/spinningbullets.png";
            GameObject gameObject = new GameObject(name);
            BulletSynthesizer item = gameObject.AddComponent<BulletSynthesizer>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Bullet Roll";
            string longDesc = "Synthesizes a random homing bullet upon rolling over enemy bullets.\n\nAbsorbs the energy from enemy bullet's conjuring one of your own.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(172);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnIsRolling += this.HandleRollFrame;
            player.OnDodgedBeam += this.HandleDodgedBeam;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnIsRolling -= this.HandleRollFrame;
            player.OnDodgedBeam += this.HandleDodgedBeam;
            return base.Drop(player);
        }

        private void HandleDodgedBeam(BeamController beam, PlayerController player)
        {
            if (Ratt)
            {
                Ratt = false;
                StartCoroutine(Rat());
                ShootRandomProjectile(player);
            }
        }

        private void ShootRandomProjectile(PlayerController player)
        {
            Gun randomGun;
            int pickupObjectId;
            do
            {
                randomGun = PickupObjectDatabase.GetRandomGun();
                pickupObjectId = randomGun.PickupObjectId;
            }
            while (randomGun.HasShootStyle(ProjectileModule.ShootStyle.Beam));
            Projectile bullet = randomGun.DefaultModule.projectiles[0];
            player.HandleProjectile(bullet.baseData.speed, bullet.baseData.damage, pickupObjectId, false, Vector2.zero);
        }

        private IEnumerator Rat()
        {
            yield return new WaitForSeconds(.5f);
            Ratt = true;
            yield break;
        }

        private void HandleRollFrame(PlayerController player)
        {
            if (player.CurrentRollState == PlayerController.DodgeRollState.InAir)
            {
                Vector2 centerPosition = player.CenterPosition;
                for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
                {
                    Projectile projectile = StaticReferenceManager.AllProjectiles[i];
                    if (projectile && projectile.Owner is AIActor)
                    {
                        float sqrMagnitude = (projectile.transform.position.XY() - centerPosition).sqrMagnitude;
                        if (sqrMagnitude < 2)
                        {
                            ShootRandomProjectile(player);
                            projectile.DieInAir();
                            break;
                        }
                    }
                }
            }
        }

        private bool Ratt = true;
    }
}
