﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using MultiplayerBasicExample;
using System.Collections;

namespace katmod
{

	class ClassicCatSnack : PassiveItem
	{
		public static void Init()
		{
			string name = "Cat Snack (Excluded)";
			string resourcePath = "katmod/Resources/fishsnack.png";
			GameObject gameObject = new GameObject(name);
			ClassicCatSnack item = gameObject.AddComponent<ClassicCatSnack>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Bad Design";
			string longDesc = "Has a chance to shoot a fish along with every shot.\n\nI do not like this item. At it's best, its just a worse Sapling Bullets, but in a higher tier because of the shark easter egg, and at it's worst, you get 10 sharks a second. Game design notes with blazeykat: Low chance for lots of damage is a really boring item concept.";
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Accuracy, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.EXCLUDED;
			item.RemovePickupFromLootTables();
		}

		public void PostProcessProjectile(Projectile projectile, float chungo)
		{
			PlayerController player = base.Owner;
			float Chance = 0.8f;
			if (player.HasGun(7))
			{
				Chance -= 0.05f;
			}
			if (BoxOTools.BasicRandom(Chance) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (BoxOTools.BasicRandom(0.98f) && !SharkMFs)
				{
					SharkMFs = true;
					StartCoroutine(SharkCooldown());
					player.HandleProjectile(25f, (player.HasGun(359)) ? 110 : 90f, 359, true, Vector2.zero, true);
				} else
				{
					player.HandleProjectile(20f, (player.HasGun(7)) ? 15 : 10, 7, false, Vector2.zero, true);
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
			if (BoxOTools.BasicRandom(Chance) && !CoolAsIce)
			{
				CoolAsIce = true;
				StartCoroutine(StartCooldown());
				if (BoxOTools.BasicRandom(0.97f) && !SharkMFs)
				{
					SharkMFs = true;
					StartCoroutine(SharkCooldown());
					player.HandleProjectile(25f, (player.HasGun(359)) ? 110 : 95f, 359, true, Vector2.zero, true);
				}
				else
				{
					player.HandleProjectile(20f, (player.HasGun(7)) ? 20 : 10, 7, false, Vector2.zero, true);
				}
			}
		}

		private static IEnumerator StartCooldown()
        {
			yield return new WaitForSeconds(0.5f);
			ClassicCatSnack.CoolAsIce = false;
			yield break;
        }

		private static IEnumerator SharkCooldown()
		{
			yield return new WaitForSeconds(5);
			ClassicCatSnack.SharkMFs = false;
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

        private static bool CoolAsIce = false;

		private static bool SharkMFs = false;
	}
}
