using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class SwarmSneak : SwarmPickup
    {
        public static void Init()
        {
            string name = "beesneak";
            string resourcePath = "katmod/Resources/Swarm/sneakpower.png";
            GameObject gameObject = new GameObject(name);
            SwarmSneak item = gameObject.AddComponent<SwarmSneak>();
            SpeculativeRigidbody specRig = gameObject.AddComponent<SpeculativeRigidbody>();
            PixelCollider collide = new PixelCollider
            {
                IsTrigger = true,
                ManualWidth = 19,
                ManualHeight = 22,
                ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
                CollisionLayer = CollisionLayer.PlayerBlocker,
                ManualOffsetX = 0,
                ManualOffsetY = 0
            };
            specRig.PixelColliders = new List<PixelCollider>
            {
                collide
            };
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "mayo";
            string longDesc = "naise";
            item.SetupItem(shortDesc, longDesc);
            item.quality = PickupObject.ItemQuality.EXCLUDED;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.StealthEffect();
        }

		private void StealthEffect()
		{
			PlayerController lastOwner = StoredPlayer;
			lastOwner.OnItemStolen += this.BreakStealthOnSteal;
			lastOwner.ChangeSpecialShaderFlag(1, 1f);
			lastOwner.healthHaver.OnDamaged += this.OnDamaged;
			lastOwner.SetIsStealthed(true, "bee");
			lastOwner.SetCapableOfStealing(true, "bee", null);
			GameManager.Instance.StartCoroutine(this.Unstealthy());
			GameManager.Instance.StartCoroutine(this.EatFrog());
		}

		private IEnumerator EatFrog()
        {
			yield return new WaitForSeconds(10);
			this.BreakStealth(this.StoredPlayer);
			yield break;
		}

		private IEnumerator Unstealthy()
		{
			yield return new WaitForSeconds(0.15f);
			this.StoredPlayer.OnDidUnstealthyAction += this.BreakStealth;
			yield break;
		}

		private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
		{
			this.BreakStealth(this.StoredPlayer);
		}

		private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
		{
			this.BreakStealth(arg1);
		}

		private void BreakStealth(PlayerController player)
		{
			player.ChangeSpecialShaderFlag(1, 0f);
			player.OnItemStolen -= this.BreakStealthOnSteal;
			player.SetIsStealthed(false, "bee");
			player.healthHaver.OnDamaged -= this.OnDamaged;
			player.SetCapableOfStealing(false, "bee", null);
			player.OnDidUnstealthyAction -= this.BreakStealth;
			AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
		}
	}
}
