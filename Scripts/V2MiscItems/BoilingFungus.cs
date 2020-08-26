using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace katmod
{
	internal class BoilingFungus : PassiveItem
	{
		public static void Init()
		{
			string name = "Boiling Fungus";
			string resourcePath = "katmod/Resources/V2MiscItems/fungus.png";
			GameObject gameObject = new GameObject(name);
			BoilingFungus item = gameObject.AddComponent<BoilingFungus>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Hot Water";
			string longDesc = "Creates a damaging aura when standing still\n\nA previously pacifistic fungus genetically modified to kill.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.B;
			item.sprite.IsPerpendicular = true;
			item.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			item.PlaceItemInAmmonomiconAfterItemById(258);
		}

		public override void Pickup(PlayerController player)
		{
			bool pickedUp = this.m_pickedUp;
			if (!pickedUp)
			{
				base.Pickup(player);
			}
		}

		protected override void Update()
		{
			base.Update();
			if (this.m_pickedUp && !GameManager.Instance.IsLoadingLevel && this.m_owner != null && this.Owner.Velocity.magnitude == 0 && !this.m_owner.IsFalling && this.m_owner.healthHaver && !this.m_owner.healthHaver.IsDead)
			{
				if (WalkFlag)
				{
					if (!Active) 
					{
						this.m_radialIndicatorActive = true;
						this.ShockRing();
						this.Active = true;
					}
					List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
					Vector2 centerPosition = base.Owner.CenterPosition;
					foreach (AIActor aiactor in activeEnemies)
					{
						bool flag = Vector2.Distance(aiactor.CenterPosition, centerPosition) < 4f && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && base.Owner != null;
						if (flag)
						{
							aiactor.healthHaver.ApplyDamage(0.25f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
						}
					}
				} else
                {
					StartCoroutine(StartCooldown());
                }
			}
			else if (Active)
			{
				StopCoroutine(StartCooldown());
				WalkFlag = false;
				this.Active = false;
				this.m_radialIndicator.EndEffect();
				this.m_radialIndicatorActive = false;
			}
		}

		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(1f);
			WalkFlag = true;
			yield break;
		}
		private void ShockRing()
		{
			this.m_radialIndicatorActive = true;
			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.Owner.CenterPosition, Quaternion.identity, base.transform)).GetComponent<HeatIndicatorController>();
			this.m_radialIndicator.CurrentColor = Color.red;
			this.m_radialIndicator.IsFire = false;
			this.m_radialIndicator.CurrentRadius = 4f;
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			debrisObject.GetComponent<BoilingFungus>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		private bool m_radialIndicatorActive;

		private HeatIndicatorController m_radialIndicator;

		private bool Active = false;

		private bool WalkFlag;
	}
}
