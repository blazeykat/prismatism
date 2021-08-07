using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
	class DeadRinger : DamageReductionItem
	{
		public static void Init()
		{
			string name = "Dead Ringer";
			string resourcePath = "katmod/Resources/V3MiscItems/deadringer";
			GameObject gameObject = new GameObject(name);
			DeadRinger item = gameObject.AddComponent<DeadRinger>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Gentlemen";
			string longDesc = "Take less damage, fake your death and cloak yourself upon taking damage.\n\nThey definitely won't think you're still alive the 12th time.";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
			item.quality = ItemQuality.C;
			item.PlaceItemInAmmonomiconAfterItemById(462);
		}

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnReceivedDamage += Damaged;
        }

        private void Damaged(PlayerController player)
        {

			this.StealthEffect();
		}

		private void StealthEffect()
		{
			PlayerController lastOwner = Owner;
			lastOwner.OnItemStolen += this.BreakStealthOnSteal;
			lastOwner.ChangeSpecialShaderFlag(1, 1f);
			lastOwner.healthHaver.OnDamaged += this.OnDamaged;
			lastOwner.SetIsStealthed(true, "deadringer");
			lastOwner.SetCapableOfStealing(true, "deadringer", null);
			GameManager.Instance.StartCoroutine(this.Unstealthy());
			GameManager.Instance.StartCoroutine(this.EatFrog());
		}

		private IEnumerator EatFrog()
		{
			yield return new WaitForSeconds(10);
			this.BreakStealth(this.Owner);
			yield break;
		}

		private IEnumerator Unstealthy()
		{
			yield return new WaitForSeconds(0.15f);
			this.Owner.OnDidUnstealthyAction += this.BreakStealth;
			yield break;
		}

		private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			this.BreakStealth(base.Owner);
		}

		private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
		{
			this.BreakStealth(arg1);
		}

		private void BreakStealth(PlayerController player)
		{
			player.ChangeSpecialShaderFlag(1, 0f);
			player.OnItemStolen -= this.BreakStealthOnSteal;
			player.SetIsStealthed(false, "deadringer");
			player.healthHaver.OnDamaged -= this.OnDamaged;
			player.SetCapableOfStealing(false, "deadringer", null);
			player.OnDidUnstealthyAction -= this.BreakStealth;
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
		}
	}
}
