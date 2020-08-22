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
	class PrismaticGuonStone : IounStoneOrbitalItem
	{
		public static int ID;
		public static void Init()
		{
			string name = "Prismatic Guon Stone";
			string resourcePath = "katmod/Resources/PrismaticSet/prismaticguonstone.png";
			GameObject gameObject = new GameObject();
			PrismaticGuonStone item = gameObject.AddComponent<PrismaticGuonStone>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "bright!";
			string longDesc = "Rotates around the player and blocks bullets, dissapears after a while.\n\nMade from pure light whatever magic.";
			item.SetupItem(shortDesc, longDesc, "psm");
			item.quality = PickupObject.ItemQuality.EXCLUDED;
			PrismaticGuonStone.BuildPrefab();
			PrismaticGuonStone.ID = item.PickupObjectId;
			item.OrbitalPrefab = PrismaticGuonStone.orbitalPrefab;
			item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			item.CanBeDropped = false;
		}
		public static void BuildPrefab()
		{
			bool flag = PrismaticGuonStone.orbitalPrefab != null;
			if (!flag)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("katmod/Resources/PrismaticSet/prismaticguonorbital.png", null);
				gameObject.name = "Prismatic Guon Orbital";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(7, 13));
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
				orbitalPrefab.shouldRotate = false;
				orbitalPrefab.orbitRadius = 2.5f;
				orbitalPrefab.SetOrbitalTier(0);
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
			base.Invoke("breakThis", 4);
			base.Pickup(player);
		}
		public override DebrisObject Drop(PlayerController player)
		{
			speedUp = false;
			return base.Drop(player);
		}
		private void breakThis()
		{
			base.Owner.RemovePassiveItem(PickupObjectId);
		}

		protected override void OnDestroy()
		{
			speedUp = false;
			base.OnDestroy();
		}

		public static bool speedUp = false;


		public static PlayerOrbital orbitalPrefab;
	}
}
