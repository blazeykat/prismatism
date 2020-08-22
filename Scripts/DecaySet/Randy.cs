using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using MonoMod.RuntimeDetour.HookGen;
using UnityEngine;

namespace katmod
{
	class Randy : IounStoneOrbitalItem
	{
		public static void Init()
		{
			string name = "Randy";
			string resourcePath = "katmod/Resources/DecaySet/randy.png";
			GameObject gameObject = new GameObject();
			Randy item = gameObject.AddComponent<Randy>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "https://modworkshop.net/mod/27616";
			string longDesc = "he is in the decay folder";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.D;
			Randy.BuildPrefab();
			item.OrbitalPrefab = Randy.orbitalPrefab;
			item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			item.CanBeDropped = false;
			item.SetupUnlockOnFlag(GungeonFlags.TUTORIAL_COMPLETED, true);
		}
		public static void BuildPrefab()
		{
			bool flag = Randy.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("katmod/Resources/DecaySet/randy.png", null);
				gameObject.name = "randorbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Randy.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				Randy.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
				Randy.orbitalPrefab.shouldRotate = false;
				Randy.orbitalPrefab.orbitRadius = 4.5f;
				Randy.orbitalPrefab.SetOrbitalTier(0);
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
			base.Pickup(player);
		}
		public override DebrisObject Drop(PlayerController player)
		{
			Randy.speedUp = false;
			return base.Drop(player);
		}

		public static bool speedUp = false;

		public static PlayerOrbital orbitalPrefab;
	}
}
