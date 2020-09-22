using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using MonoMod.RuntimeDetour.HookGen;
using UnityEngine;
using System.Collections;

namespace katmod
{
	class PurpleGuonStone : IounStoneOrbitalItem
	{
		public static void Init()
		{
			string name = "Blurple Guon Stone";
			string resourcePath = "katmod/Resources/V2MiscItems/purpleguonstone.png";
			GameObject gameObject = new GameObject();
			PurpleGuonStone item = gameObject.AddComponent<PurpleGuonStone>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Less Blue";
			string longDesc = "Rotates around the player and blocks bullets, gives extended invulnerability on damage.\n\nNot actually blurple, SpAPI had just already taken \"Purple Guon Stone\".";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.C;
			PurpleGuonStone.BuildPrefab();
			item.OrbitalPrefab = PurpleGuonStone.orbitalPrefab;
			item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			item.PlaceItemInAmmonomiconAfterItemById(260);
		}
		public static void BuildPrefab()
		{
			bool flag = PurpleGuonStone.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("katmod/Resources/V2MiscItems/purpleguonstoneorbital.png", null);
				gameObject.name = "Blurple Guon Orbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				PurpleGuonStone.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				PurpleGuonStone.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
				PurpleGuonStone.orbitalPrefab.shouldRotate = false;
				PurpleGuonStone.orbitalPrefab.orbitRadius = 2.5f;
				PurpleGuonStone.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}
		public override void Pickup(PlayerController player)
		{
			foreach (IPlayerOrbital playerOrbital in player.orbitals)
			{
				PlayerOrbital playerOrbital2 = (PlayerOrbital)playerOrbital;
				playerOrbital2.orbitDegreesPerSecond = 90f;
			}
			player.OnReceivedDamage += this.DoLiquidEffect;
			base.Pickup(player);
		}
		private IEnumerator HandleShield(PlayerController user)
		{
			float processedDuration = (user.HasPassiveItem(286) || user.HasPassiveItem(158) || user.HasPassiveItem(159)) ? 12 : duration;
			this.m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
			user.sprite.usesOverrideMaterial = true;
			user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
			SpeculativeRigidbody specRigidbody = user.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision += this.OnPreCollision;
			user.healthHaver.IsVulnerable = false;
			float elapsed = 0f;
			while (elapsed < processedDuration)
			{
				elapsed += BraveTime.DeltaTime;
				user.healthHaver.IsVulnerable = false;
				yield return null;
			}
			bool flag = user;
			if (flag)
			{
				user.healthHaver.IsVulnerable = true;
				user.ClearOverrideShader();
				user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
				SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
				specRigidbody2.OnPreRigidbodyCollision -= this.OnPreCollision;
				specRigidbody2 = null;
			}
			bool flag2 = this;
			if (flag2)
			{
				AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
			}
			yield break;
		}
		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			Projectile component = otherRigidbody.GetComponent<Projectile>();
			bool flag = component != null && !(component.Owner is PlayerController);
			if (flag)
			{
				PassiveReflectItem.ReflectBullet(component, true, base.Owner.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
				PhysicsEngine.SkipCollision = true;
			}
		}
		private void DoLiquidEffect(PlayerController player)
		{
			StartCoroutine(HandleShield(player));
		}
		public override DebrisObject Drop(PlayerController player)
		{
			speedUp = false;
			return base.Drop(player);
		}
		protected override void OnDestroy()
		{
			speedUp = false;
			base.OnDestroy();
		}

		public static bool speedUp = false;

		public static PlayerOrbital orbitalPrefab;

		private readonly float duration = 5f;

		private bool m_usedOverrideMaterial;
	}
}
