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
				Projectile fishProjectile = Owner.HandleProjectile(20, 4, 7, false, Vector2.zero, true, wackyRandomiser += i * (Owner.PlayerHasActiveSynergy("Shark Bait") ? 60 : 90));
				projectile.gameObject.AddComponent(new HomingModifier() { HomingRadius = 180, AngularVelocity = 720 });
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
		}

		public override DebrisObject Drop(PlayerController player)
		{
			player.PostProcessProjectile -= this.PostProcessProjectile;
			return base.Drop(player);
		}

		protected override void OnDestroy()
		{
			if (Owner)
			{
				Owner.PostProcessProjectile -= PostProcessProjectile;
			}
			base.OnDestroy();
		}

		static bool CoolAsIce = false;
	}
}
