using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.CompanionBuilder.AnimationType;
using Gungeon;
using FullInspector;

namespace katmod
{
	class BlueBubbler
	{

		public static GameObject prefab;
		public static readonly string guid = "bluebubbler";
		public static Dictionary<string, GameObject> enemyPrefabDictionary = new Dictionary<string, GameObject>();


		public static void Init()
		{

			BlueBubbler.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			if (prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid))
				return;

			//Create the prefab with a starting sprite and hitbox offset/size
			prefab = EnemyBuilder.BuildPrefab("BlueBubbler", guid, "katmod/Resources/Enemies/BlueBub/Idle/bubred_idle_001", new IntVector2(1, 0), new IntVector2(9, 9));

			//Add a companion component to the prefab (could be a custom class)
			AIActor companion = prefab.GetComponent<AIActor>();
			companion.aiActor.MovementSpeed = 9f;
			companion.aiActor.healthHaver.PreventAllDamage = false;
			companion.aiActor.CollisionDamage = 100f;
			companion.aiActor.HasShadow = false;
			companion.aiActor.CanTargetPlayers = true;
			companion.aiActor.specRigidbody.CollideWithOthers = true;
			companion.aiActor.specRigidbody.CollideWithTileMap = true;
			companion.aiActor.healthHaver.ForceSetCurrentHealth(35f);
			companion.aiActor.healthHaver.SetHealthMaximum(35f, null, false);
			companion.CollisionKnockbackStrength = 10;
			companion.aiActor.specRigidbody.PixelColliders.Clear();
			companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.EnemyCollider,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 20,
				ManualOffsetY = 6,
				ManualWidth = 1,
				ManualHeight = 1,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});
			companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
			{
				ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
				CollisionLayer = CollisionLayer.EnemyHitBox,
				IsTrigger = false,
				BagleUseFirstFrameOnly = false,
				SpecifyBagelFrame = string.Empty,
				BagelColliderNumber = 0,
				ManualOffsetX = 0,
				ManualOffsetY = 0,
				ManualWidth = 10,
				ManualHeight = 26,
				ManualDiameter = 0,
				ManualLeftX = 0,
				ManualLeftY = 0,
				ManualRightX = 0,
				ManualRightY = 0
			});

			//Add all of the needed animations (most of the animations need to have specific names to be recognized, like idle_right or attack_left)
			prefab.AddAnimation("idle", "katmod/Resources/Enemies/BlueBub/Idle", fps: 5, AnimationType.Idle, DirectionType.TwoWayHorizontal);


			//Add the behavior here, this too can be a custom class that extends AttackBehaviorBase or something like that
			var bs = prefab.GetComponent<BehaviorSpeculator>();
			bs.TargetBehaviors = new List<TargetBehaviorBase>() {
				new TargetPlayerBehavior() {
					Radius = 35,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f
				}
			};
			bs.MovementBehaviors = new List<MovementBehaviorBase>() {
				new SeekTargetBehavior() {
					StopWhenInRange = false,
					CustomRange = 6,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0,
					MaxActiveRange = 0
				}
			};

		}




	}
}
