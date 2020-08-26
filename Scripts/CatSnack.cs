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
			string shortDesc = "Channel your inner fish";
			string longDesc = "Has a chance to shoot a fish along with every shot.\n\n...and I followed the sound of music...";
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.A;
			List<string> mandatoryConsoleIDs = new List<string>
			{
				"psm:cat_snack",
				"barrel"
			};
			CustomSynergies.Add("feesh", mandatoryConsoleIDs, null, true);
			item.SetupUnlockOnFlag(GungeonFlags.FRIFLE_REWARD_SHARK_GUN, true);
			item.PlaceItemInAmmonomiconAfterItemById(301);
		}

		private void PostProcessProjectile(Projectile projectile, float chungo)
		{
			PlayerController player = base.Owner;
			float Chance = 0.8f;
			if (player.HasGun(7))
			{
				Chance -= 0.05f;
			}
			if (Utilities.BasicRandom(player, Chance, 50) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (Utilities.UncoolRandom(0.99f) && !SharkMFs)
				{
					SharkMFs = true;
					StartCoroutine(SharkCooldown());
					player.HandleChargeProjectile(25f, (player.HasGun(359)) ? 110 : 90f, 359);
				} else
				{
					player.HandleProjectile(20f, (player.HasGun(7)) ? 15 : 10, 7);
				}
			}
		}


		private void PostProcessBeamChanceTick(BeamController beamController)
		{
			PlayerController player = base.Owner;
			float Chance = 0.9f;
			if (player.HasGun(7))
			{
				Chance -= 0.05f;
			}
			if (Utilities.BasicRandom(player, Chance, 100) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (Utilities.UncoolRandom(0.97f) && !SharkMFs)
				{
					SharkMFs = true;
					StartCoroutine(SharkCooldown());
					player.HandleChargeProjectile(25f, (player.HasGun(359)) ? 110 : 95f, 359);
				}
				else
				{
					player.HandleProjectile(20f, (player.HasGun(7)) ? 20 : 10, 7);
				}
			}
		}

		private IEnumerator StartCooldown()
        {
			yield return new WaitForSeconds(0.5f);
			CatSnack.CoolAsIce = false;
			yield break;
        }

		private IEnumerator SharkCooldown()
		{
			yield return new WaitForSeconds(5);
			CatSnack.SharkMFs = false;
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

		static bool SharkMFs = false;
	}
}
