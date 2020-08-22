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
    class BlackSkull : IounStoneOrbitalItem
    {
        public static void Init()
        {
			string name = "Black Skull";
			string resourcePath = "katmod/Resources/DecaySet/blackskull.png";
			GameObject gameObject = new GameObject();
			BlackSkull item = gameObject.AddComponent<BlackSkull>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Dark Omen";
			string longDesc = "Rotates around the player and blocks bullets.\n\nThese skulls are taken straight from the ominous ashen skeletons. They travel in hordes, laying waste to all they see. Where one goes, a hundred more follow.\n\nThe gungeon shall claim another victim.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			BlackSkull.BuildPrefab();
			item.OrbitalPrefab = BlackSkull.orbitalPrefab;
			item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
		}
		public static void BuildPrefab()
		{
			bool flag = BlackSkull.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("katmod/Resources/DecaySet/skullguon.png", null);
				gameObject.name = "Black Skull Orbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				BlackSkull.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				BlackSkull.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
				BlackSkull.orbitalPrefab.shouldRotate = false;
				BlackSkull.orbitalPrefab.orbitRadius = 2.5f;
				BlackSkull.orbitalPrefab.SetOrbitalTier(0);
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
			BlackSkull.speedUp = false;
			return base.Drop(player);
		}

		public static bool speedUp = false;

		public static PlayerOrbital orbitalPrefab;
	}
}
