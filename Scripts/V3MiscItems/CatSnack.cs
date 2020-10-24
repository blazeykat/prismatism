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
			string longDesc = "Has a chance to shoot a fish along with every shot.\n\n...And I followed the sound of music...";
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.B;
			item.PlaceItemInAmmonomiconAfterItemById(301);
		}

		private void PostProcessProjectile(Projectile projectile, float chungo)
		{
			if (BoxOTools.BasicRandom((base.Owner.HasGun(7) ? 0.7f : 0.9f) * chungo) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				projectile.AdjustPlayerProjectileTint(Color.blue, 5);
				projectile.OnDestruction += Wave;
			}
		}

        private void Wave(Projectile obj)
        {
			int wackyRandomiser = UnityEngine.Random.Range(0, 90);
			for (int i = 0; i < 4; i++)
            {
				obj.sprite.HandleProjectileAimedFromDifferentPosition(Owner, 20, 10, 7, wackyRandomiser += i * 90);
            }
        }

        private void PostProcessBeamChanceTick(BeamController beamController)
		{
			if (BoxOTools.BasicRandom((base.Owner.HasGun(7) ? 0.7f : 0.9f)) && !CoolAsIce)
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
			Owner.PostProcessProjectile -= PostProcessProjectile;
			Owner.PostProcessBeamChanceTick -= PostProcessBeamChanceTick;
			base.OnDestroy();
		}

		static bool CoolAsIce = false;
	}
}
