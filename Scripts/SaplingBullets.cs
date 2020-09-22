using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;

namespace katmod
{

	class SaplingBullets : PassiveItem
	{
		public static int ID;

		public static void Init()
		{
			string name = "Sapling Bullets";
			string resourcePath = "katmod/Resources/seedbullet";
			GameObject gameObject = new GameObject(name);
			SaplingBullets item = gameObject.AddComponent<SaplingBullets>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Sap-Fling";
			string longDesc = "Shoots a leaf along with each shot.\n\nThese seeds are planted inside of the bullets, and they feed upon gunpowder to grow. The speeds of the bullets cause them to soak up more gunpowder and grow quicker.\n\n\"Oh, please. Did you really think you'd be that lucky?\"";
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.B;
			item.PlaceItemInAmmonomiconAfterItemById(640);
			ID = item.PickupObjectId;
		}

		private void PostProcessProjectile(Projectile projectile, float Chance)
		{
			PlayerController player = base.Owner;
			if (Utilities.BasicRandom(player, 0.7f, 50) && CoolAsIce)
			{
				CoolAsIce = false;
				StartCoroutine(StartCooldown());
				Projectile component = player.HandleChargeProjectile(20f, player.HasGun(339) ? 9f : 7f, 620);
				component.AddHoming();
			}
		}


		private void PostProcessBeamChanceTick(BeamController beamController)
		{
			PlayerController player = base.Owner;
			if (Utilities.BasicRandom(player, 0.8f, 50) && CoolAsIce)
			{
				CoolAsIce = false;
				StartCoroutine(StartCooldown());
				Projectile component = player.HandleChargeProjectile(20f, player.HasGun(339) ? 9f : 7f, 620);
				component.AddHoming();
			}
		}


		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(1f);
			this.CoolAsIce = true;
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
			base.Owner.PostProcessProjectile -= this.PostProcessProjectile;
			base.Owner.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			base.OnDestroy();
        }

        private bool CoolAsIce = true;
	}
}
