using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MultiplayerBasicExample;
using System.Collections;

namespace katmod
{

	class CatSnack : PassiveItem
	{
		public static void Init()
		{
			string name = "Cat Snack";
			string resourcePath = "katmod/Resources/fishsnack.png";
			GameObject gameObject = new GameObject(name);
			CatSnack item = gameObject.AddComponent<CatSnack>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Fishy Fishy";
			string longDesc = "Has a chance to imbue your shots with the power of feesh.\n\n...And I followed the sound of music...";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.A;
			item.PlaceItemInAmmonomiconAfterItemById(301);
		}

		private void PostProcessProjectile(Projectile projectile, float chungo)
		{
			if (BoxOTools.BasicRandom(base.Owner.CurrentGun.PickupObjectId == 7 ? 0.4f : 0.9f) && !CoolAsIce && projectile)
			{
				CoolAsIce = true;
				projectile.AdjustPlayerProjectileTint(Color.blue, 5);
				projectile.OnDestruction += Wave;
			}
		}

        private void Wave(Projectile obj)
        {
			int wackyRandomiser = UnityEngine.Random.Range(0, 90);
			StartCoroutine(StartCooldown());
			for (int i = 0; i < (Owner.PlayerHasActiveSynergy("Shark Bait") ? 6 : 4); i++)
            {
				Projectile fishProjectile = obj.sprite.ShootProjectileFromSprite(Owner, 20, 4, 7, wackyRandomiser += i * (Owner.PlayerHasActiveSynergy("Shark Bait") ? 60 : 90));
				fishProjectile.AddHoming(180, 720);
            }
        }

        private void PostProcessBeamChanceTick(BeamController beamController)
		{
			if (BoxOTools.BasicRandom(base.Owner.CurrentGun.PickupObjectId == 7 ? 0.4f : 0.9f) && !CoolAsIce && beamController && beamController.projectile)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				beamController.projectile.AdjustPlayerProjectileTint(Color.blue, 5);
				beamController.projectile.OnDestruction += Wave;
			}
		}

		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(1f);
			CatSnack.CoolAsIce = false;
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

		protected override void OnDestroy()
		{
			if (Owner)
			{
				Owner.PostProcessProjectile -= PostProcessProjectile;
				Owner.PostProcessBeamChanceTick -= PostProcessBeamChanceTick;
			}
			base.OnDestroy();
		}

		static bool CoolAsIce = false;
	}
}
