using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using katmod;
using static PickupObject;
using System.IO;
using Dungeonator;

namespace ItemAPI
{
    public static class BoxOTools
    {
        public static readonly string EncounterGUID = "trolled";

        public static void PlaceItemInAmmonomiconAfterItemById(this PickupObject item, int id) //from SpAPI's items
        {
            item.ForcedPositionInAmmonomicon = PickupObjectDatabase.GetById(id).ForcedPositionInAmmonomicon;
        }

        public static void SetupUnlockOnFlag(this PickupObject self, GungeonFlags flag, bool requiredFlagValue) //thanks SpAPI
        {
            EncounterTrackable encounterTrackable = self.encounterTrackable;
            bool flag2 = encounterTrackable.prerequisites == null;
            if (flag2)
            {
                encounterTrackable.prerequisites = new DungeonPrerequisite[1];
                encounterTrackable.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = requiredFlagValue,
                    saveFlagToCheck = flag
                };
            }
            else
            {
                encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[]
                {
                    new DungeonPrerequisite
                    {
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                        requireFlag = requiredFlagValue,
                        saveFlagToCheck = flag
                    }
                }).ToArray<DungeonPrerequisite>();
            }
            EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
            bool flag3 = !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
            if (flag3)
            {
                entry.ProxyEncounterGuid = "";
            }
            bool flag4 = entry.prerequisites == null;
            if (flag4)
            {
                entry.prerequisites = new DungeonPrerequisite[1];
                entry.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                    requireFlag = requiredFlagValue,
                    saveFlagToCheck = flag
                };
            }
            else
            {
                entry.prerequisites = entry.prerequisites.Concat(new DungeonPrerequisite[]
                {
                    new DungeonPrerequisite
                    {
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG,
                        requireFlag = requiredFlagValue,
                        saveFlagToCheck = flag
                    }
                }).ToArray<DungeonPrerequisite>();
            }
        }

        public static void SetupUnlockOnStat(this PickupObject self, TrackedStats stat, DungeonPrerequisite.PrerequisiteOperation operation, int comparisonValue)
        {
            EncounterTrackable encounterTrackable = self.encounterTrackable;
            bool flag = encounterTrackable.prerequisites == null;
            if (flag)
            {
                encounterTrackable.prerequisites = new DungeonPrerequisite[1];
                encounterTrackable.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                    prerequisiteOperation = operation,
                    statToCheck = stat,
                    comparisonValue = (float)comparisonValue
                };
            }
            else
            {
                encounterTrackable.prerequisites = encounterTrackable.prerequisites.Concat(new DungeonPrerequisite[]
                {
                    new DungeonPrerequisite
                    {
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                        prerequisiteOperation = operation,
                        statToCheck = stat,
                        comparisonValue = (float)comparisonValue
                    }
                }).ToArray<DungeonPrerequisite>();
            }
            EncounterDatabaseEntry entry = EncounterDatabase.GetEntry(encounterTrackable.EncounterGuid);
            bool flag2 = !string.IsNullOrEmpty(entry.ProxyEncounterGuid);
            if (flag2)
            {
                entry.ProxyEncounterGuid = "";
            }
            bool flag3 = entry.prerequisites == null;
            if (flag3)
            {
                entry.prerequisites = new DungeonPrerequisite[1];
                entry.prerequisites[0] = new DungeonPrerequisite
                {
                    prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                    prerequisiteOperation = operation,
                    statToCheck = stat,
                    comparisonValue = (float)comparisonValue
                };
            }
            else
            {
                entry.prerequisites = entry.prerequisites.Concat(new DungeonPrerequisite[]
                {
                    new DungeonPrerequisite
                    {
                        prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON,
                        prerequisiteOperation = operation,
                        statToCheck = stat,
                        comparisonValue = (float)comparisonValue
                    }
                }).ToArray<DungeonPrerequisite>();
            }
        }

        public static Projectile HandleProjectile(this PlayerController player, float speed, float damage, int ProjectileID, bool charged, Vector2 overridePosition, bool processes = false, float overrideAim = 0, float range = 0)
        {
            Projectile projectile2;
            if (!charged)
            {
                projectile2 = ((Gun)ETGMod.Databases.Items[ProjectileID]).DefaultModule.projectiles[0];
            } else
            {
                projectile2 = ((Gun)ETGMod.Databases.Items[ProjectileID]).DefaultModule.chargeProjectiles[0].Projectile;
            }
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, overridePosition != Vector2.zero ? overridePosition : (player.CurrentGun == null) ? player.specRigidbody.UnitCenter : (Vector2)player.CurrentGun.barrelOffset.position, Quaternion.Euler(0f, 0f, (player.CurrentGun == null || overrideAim != 0) ? overrideAim : player.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool componentless = component != null;
            if (componentless)
            {
                component.Owner = player;
                component.Shooter = player.specRigidbody;
                component.baseData.speed = speed;
                component.baseData.damage = damage;
                if (range != 0)
                {
                    component.baseData.range = range;
                }
                if (processes)
                {
                    player.DoPostProcessProjectile(component);
                }
            }
            return component;
        }

        /*public static bool BasicRandom(PlayerController player, float value, float divider)
        {
			if (UnityEngine.Random.value > value - (player.stats.GetStatValue(PlayerStats.StatType.Coolness) / divider))
            {
				return true;
            } else
            {
				return false;
            }
        }*/

        /// <summary>
        /// The basic random value
        /// </summary>
        public static bool BasicRandom(float value)
        {
            if (UnityEngine.Random.value > value || Prismatism.DebugChance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static AIActor SummonAtRandomPosition(string guid, PlayerController owner)
        {
            AIActor aiactor = AIActor.Spawn(EnemyDatabase.GetOrLoadByGuid(guid), new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(new IntVector2?(owner.CurrentRoom.GetRandomVisibleClearSpot(1, 1)).Value), true, AIActor.AwakenAnimationType.Awaken, true);
            return aiactor;
        }

        /// <summary>
        /// Ask spapi when this goes wrong
        /// </summary>
        public static void ConstructOffsetsFromAnchor(this tk2dSpriteDefinition def, tk2dBaseSprite.Anchor anchor, Vector2? scale = null, bool fixesScale = false, bool changesCollider = true)
        {
            if (!scale.HasValue)
            {
                scale = new Vector2?(def.position3);
            }
            if (fixesScale)
            {
                Vector2 fixedScale = scale.Value - def.position0.XY();
                scale = new Vector2?(fixedScale);
            }
            float xOffset = 0;
            if (anchor == tk2dBaseSprite.Anchor.LowerCenter || anchor == tk2dBaseSprite.Anchor.MiddleCenter || anchor == tk2dBaseSprite.Anchor.UpperCenter)
            {
                xOffset = -(scale.Value.x / 2f);
            }
            else if (anchor == tk2dBaseSprite.Anchor.LowerRight || anchor == tk2dBaseSprite.Anchor.MiddleRight || anchor == tk2dBaseSprite.Anchor.UpperRight)
            {
                xOffset = -scale.Value.x;
            }
            float yOffset = 0;
            if (anchor == tk2dBaseSprite.Anchor.MiddleLeft || anchor == tk2dBaseSprite.Anchor.MiddleCenter || anchor == tk2dBaseSprite.Anchor.MiddleLeft)
            {
                yOffset = -(scale.Value.y / 2f);
            }
            else if (anchor == tk2dBaseSprite.Anchor.UpperLeft || anchor == tk2dBaseSprite.Anchor.UpperCenter || anchor == tk2dBaseSprite.Anchor.UpperRight)
            {
                yOffset = -scale.Value.y;
            }
            def.MakeOffset(new Vector2(xOffset, yOffset), changesCollider);
        }


        public static void MakeOffset(this tk2dSpriteDefinition def, Vector2 offset, bool changesCollider = false)
        {
            float xOffset = offset.x;
            float yOffset = offset.y;
            def.position0 += new Vector3(xOffset, yOffset, 0);
            def.position1 += new Vector3(xOffset, yOffset, 0);
            def.position2 += new Vector3(xOffset, yOffset, 0);
            def.position3 += new Vector3(xOffset, yOffset, 0);
            def.boundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.boundsDataExtents += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataCenter += new Vector3(xOffset, yOffset, 0);
            def.untrimmedBoundsDataExtents += new Vector3(xOffset, yOffset, 0);
            if (def.colliderVertices != null && def.colliderVertices.Length > 0 && changesCollider)
            {
                def.colliderVertices[0] += new Vector3(xOffset, yOffset, 0);
            }
        }

        public static void SetProjectileSpriteRight(this Projectile proj, string name, int pixelWidth, int pixelHeight, bool lightened = true, tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.LowerLeft, bool anchorChangesCollider = true,
                bool fixesScale = false, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null, int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null)
        {
            try
            {
                proj.GetAnySprite().spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
                tk2dSpriteDefinition def = SetupDefinitionForProjectileSprite(name, proj.GetAnySprite().spriteId, pixelWidth, pixelHeight, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX,
                    overrideColliderOffsetY, overrideProjectileToCopyFrom);
                def.ConstructOffsetsFromAnchor(anchor, def.position3, fixesScale, anchorChangesCollider);
                proj.GetAnySprite().scale = new Vector3(1f, 1f, 1f);
                proj.transform.localScale = new Vector3(1f, 1f, 1f);
                proj.GetAnySprite().transform.localScale = new Vector3(1f, 1f, 1f);
                proj.AdditionalScaleMultiplier = 1f;
            }
            catch (Exception ex)
            {
                ETGModConsole.Log("Ooops! Seems like something got very, Very, VERY wrong. Here's the exception:");
                ETGModConsole.Log(ex.ToString());
            }
        }

        public static tk2dSpriteDefinition SetupDefinitionForProjectileSprite(string name, int id, int pixelWidth, int pixelHeight, bool lightened = true, int? overrideColliderPixelWidth = null, int? overrideColliderPixelHeight = null,
            int? overrideColliderOffsetX = null, int? overrideColliderOffsetY = null, Projectile overrideProjectileToCopyFrom = null)
        {
            if (overrideColliderPixelWidth == null)
            {
                overrideColliderPixelWidth = pixelWidth;
            }
            if (overrideColliderPixelHeight == null)
            {
                overrideColliderPixelHeight = pixelHeight;
            }
            if (overrideColliderOffsetX == null)
            {
                overrideColliderOffsetX = 0;
            }
            if (overrideColliderOffsetY == null)
            {
                overrideColliderOffsetY = 0;
            }
            float thing = 14;
            float thing2 = 16;
            float trueWidth = (float)pixelWidth / thing;
            float trueHeight = (float)pixelHeight / thing;
            float colliderWidth = (float)overrideColliderPixelWidth.Value / thing2;
            float colliderHeight = (float)overrideColliderPixelHeight.Value / thing2;
            float colliderOffsetX = (float)overrideColliderOffsetX.Value / thing2;
            float colliderOffsetY = (float)overrideColliderOffsetY.Value / thing2;
            tk2dSpriteDefinition def = ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[(overrideProjectileToCopyFrom ??
                    (PickupObjectDatabase.GetById(lightened ? 47 : 12) as Gun).DefaultModule.projectiles[0]).GetAnySprite().spriteId].CopyDefinitionFrom();
            def.boundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.boundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.untrimmedBoundsDataCenter = new Vector3(trueWidth / 2f, trueHeight / 2f, 0f);
            def.untrimmedBoundsDataExtents = new Vector3(trueWidth, trueHeight, 0f);
            def.texelSize = new Vector2(1 / 16f, 1 / 16f);
            def.position0 = new Vector3(0f, 0f, 0f);
            def.position1 = new Vector3(0f + trueWidth, 0f, 0f);
            def.position2 = new Vector3(0f, 0f + trueHeight, 0f);
            def.position3 = new Vector3(0f + trueWidth, 0f + trueHeight, 0f);
            def.colliderVertices[0].x = colliderOffsetX;
            def.colliderVertices[0].y = colliderOffsetY;
            def.colliderVertices[1].x = colliderWidth;
            def.colliderVertices[1].y = colliderHeight;
            def.name = name;
            ETGMod.Databases.Items.ProjectileCollection.inst.spriteDefinitions[id] = def;
            return def;
        }

        public static void AnimateProjectile(this Projectile proj, List<string> names, int fps, bool loops, List<IntVector2> pixelSizes, List<bool> lighteneds, List<tk2dBaseSprite.Anchor> anchors, List<bool> anchorsChangeColliders,
            List<bool> fixesScales, List<Vector3?> manualOffsets, List<IntVector2?> overrideColliderPixelSizes, List<IntVector2?> overrideColliderOffsets, List<Projectile> overrideProjectilesToCopyFrom)
        {
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip
            {
                name = "idle",
                fps = fps
            };
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                IntVector2 pixelSize = pixelSizes[i];
                IntVector2? overrideColliderPixelSize = overrideColliderPixelSizes[i];
                IntVector2? overrideColliderOffset = overrideColliderOffsets[i];
                Vector3? manualOffset = manualOffsets[i];
                bool anchorChangesCollider = anchorsChangeColliders[i];
                bool fixesScale = fixesScales[i];
                if (!manualOffset.HasValue)
                {
                    manualOffset = new Vector2?(Vector2.zero);
                }
                tk2dBaseSprite.Anchor anchor = anchors[i];
                bool lightened = lighteneds[i];
                Projectile overrideProjectileToCopyFrom = overrideProjectilesToCopyFrom[i];
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame
                {
                    spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name),
                    spriteCollection = ETGMod.Databases.Items.ProjectileCollection
                };
                frames.Add(frame);
                int? overrideColliderPixelWidth = null;
                int? overrideColliderPixelHeight = null;
                if (overrideColliderPixelSize.HasValue)
                {
                    overrideColliderPixelWidth = overrideColliderPixelSize.Value.x;
                    overrideColliderPixelHeight = overrideColliderPixelSize.Value.y;
                }
                int? overrideColliderOffsetX = null;
                int? overrideColliderOffsetY = null;
                if (overrideColliderOffset.HasValue)
                {
                    overrideColliderOffsetX = overrideColliderOffset.Value.x;
                    overrideColliderOffsetY = overrideColliderOffset.Value.y;
                }
                tk2dSpriteDefinition def = SetupDefinitionForProjectileSprite(name, frame.spriteId, pixelSize.x, pixelSize.y, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX, overrideColliderOffsetY,
                    overrideProjectileToCopyFrom);
                def.ConstructOffsetsFromAnchor(anchor, def.position3, fixesScale, anchorChangesCollider);
                def.position0 += manualOffset.Value;
                def.position1 += manualOffset.Value;
                def.position2 += manualOffset.Value;
                def.position3 += manualOffset.Value;
                if (i == 0)
                {
                    proj.GetAnySprite().SetSprite(frame.spriteCollection, frame.spriteId);
                }
            }
            clip.wrapMode = loops ? tk2dSpriteAnimationClip.WrapMode.Loop : tk2dSpriteAnimationClip.WrapMode.Once;
            clip.frames = frames.ToArray();
            if (proj.sprite.spriteAnimator == null)
            {
                proj.sprite.spriteAnimator = proj.sprite.gameObject.AddComponent<tk2dSpriteAnimator>();
            }
            proj.sprite.spriteAnimator.playAutomatically = true;
            bool flag = proj.sprite.spriteAnimator.Library == null;
            if (flag)
            {
                proj.sprite.spriteAnimator.Library = proj.sprite.spriteAnimator.gameObject.AddComponent<tk2dSpriteAnimation>();
                proj.sprite.spriteAnimator.Library.clips = new tk2dSpriteAnimationClip[0];
                proj.sprite.spriteAnimator.Library.enabled = true;
            }
            proj.sprite.spriteAnimator.Library.clips = proj.sprite.spriteAnimator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            proj.sprite.spriteAnimator.DefaultClipId = proj.sprite.spriteAnimator.Library.GetClipIdByName("idle");
            proj.sprite.spriteAnimator.deferNextStartClip = false;
        }

        public static tk2dSpriteDefinition CopyDefinitionFrom(this tk2dSpriteDefinition other)
        {
            tk2dSpriteDefinition result = new tk2dSpriteDefinition
            {
                boundsDataCenter = new Vector3
                {
                    x = other.boundsDataCenter.x,
                    y = other.boundsDataCenter.y,
                    z = other.boundsDataCenter.z
                },
                boundsDataExtents = new Vector3
                {
                    x = other.boundsDataExtents.x,
                    y = other.boundsDataExtents.y,
                    z = other.boundsDataExtents.z
                },
                colliderConvex = other.colliderConvex,
                colliderSmoothSphereCollisions = other.colliderSmoothSphereCollisions,
                colliderType = other.colliderType,
                colliderVertices = other.colliderVertices,
                collisionLayer = other.collisionLayer,
                complexGeometry = other.complexGeometry,
                extractRegion = other.extractRegion,
                flipped = other.flipped,
                indices = other.indices,
                material = new Material(other.material),
                materialId = other.materialId,
                materialInst = new Material(other.materialInst),
                metadata = other.metadata,
                name = other.name,
                normals = other.normals,
                physicsEngine = other.physicsEngine,
                position0 = new Vector3
                {
                    x = other.position0.x,
                    y = other.position0.y,
                    z = other.position0.z
                },
                position1 = new Vector3
                {
                    x = other.position1.x,
                    y = other.position1.y,
                    z = other.position1.z
                },
                position2 = new Vector3
                {
                    x = other.position2.x,
                    y = other.position2.y,
                    z = other.position2.z
                },
                position3 = new Vector3
                {
                    x = other.position3.x,
                    y = other.position3.y,
                    z = other.position3.z
                },
                regionH = other.regionH,
                regionW = other.regionW,
                regionX = other.regionX,
                regionY = other.regionY,
                tangents = other.tangents,
                texelSize = new Vector2
                {
                    x = other.texelSize.x,
                    y = other.texelSize.y
                },
                untrimmedBoundsDataCenter = new Vector3
                {
                    x = other.untrimmedBoundsDataCenter.x,
                    y = other.untrimmedBoundsDataCenter.y,
                    z = other.untrimmedBoundsDataCenter.z
                },
                untrimmedBoundsDataExtents = new Vector3
                {
                    x = other.untrimmedBoundsDataExtents.x,
                    y = other.untrimmedBoundsDataExtents.y,
                    z = other.untrimmedBoundsDataExtents.z
                }
            };
            List<Vector2> uvs = new List<Vector2>();
            foreach (Vector2 vector in other.uvs)
            {
                uvs.Add(new Vector2
                {
                    x = vector.x,
                    y = vector.y
                });
            }
            result.uvs = uvs.ToArray();
            List<Vector3> colliderVertices = new List<Vector3>();
            foreach (Vector3 vector in other.colliderVertices)
            {
                colliderVertices.Add(new Vector3
                {
                    x = vector.x,
                    y = vector.y,
                    z = vector.z
                });
            }
            result.colliderVertices = colliderVertices.ToArray();
            return result;
        }
        /*/// <summary>
		/// Increases damage, makes it white, does the projectile stuff
		/// </summary>
		public static void MakeCrit(this Projectile projec, PlayerController player)
        {
			projec.baseData.damage *= 1.6f;
			projec.DefaultTintColor = Color.white;
			projec.HasDefaultTint = true;
			DoProjectile(player);
		}

		/// <summary>
		/// Handles the projectiles for all critting stuff
		/// </summary>
		public static void DoProjectile(PlayerController player)
		{
			if (player.HasPassiveItem(SaplingBullets.ID))
			{
				Projectile component = player.HandleChargeProjectile(20f, 7f, 620);
				component.AddHoming();
			}
			if (player.HasMTGConsoleID("psm:cat_snack"))
			{
				if (BoxOTools.BasicRandom(0.97f))
				{
					player.HandleChargeProjectile(25f, 95f, 359);
				}
				else
				{
					player.HandleProjectile(20f, (player.HasGun(7)) ? 20 : 10, 7);
				}
			}
		}*/

        public static PickupObject.ItemQuality GetQualityFromChances(float dChance, float cChance, float bChance, float aChance, float sChance)
        {
            float num = UnityEngine.Random.value;
            if (num < dChance)
            {
                return PickupObject.ItemQuality.D;
            }
            if (num < dChance + cChance)
            {
                return PickupObject.ItemQuality.C;
            }
            if (num < dChance + cChance + bChance)
            {
                return PickupObject.ItemQuality.B;
            }
            if (num < dChance + cChance + bChance + aChance)
            {
                return PickupObject.ItemQuality.A;
            }
            return PickupObject.ItemQuality.S;
        }

        public static void Log(this string stringy)
        {
            ETGModConsole.Log(stringy);
        }

        public static void Notify(string header, string text, string spriteID, UINotificationController.NotificationColor color = UINotificationController.NotificationColor.SILVER)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName(spriteID);
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, encounterIconCollection, spriteIdByName, color);
        }

        public static PickupObject GetTotallyRandomItem(ItemQuality quality = ItemQuality.B, bool anyQuality = true)
        {
            PickupObject pickup = LootEngine.GetItemOfTypeAndQuality<PickupObject>(quality, (UnityEngine.Random.value >= 0.5f) ? GameManager.Instance.RewardManager.ItemsLootTable : GameManager.Instance.RewardManager.GunsLootTable, anyQuality);
            return pickup;
        }

        public static bool PlayerHasActiveSynergy(this PlayerController player, string synergyNameToCheck)
        {
            foreach (int index in player.ActiveExtraSynergies)
            {
                AdvancedSynergyEntry synergy = GameManager.Instance.SynergyManager.synergies[index];
                if (synergy.NameKey == synergyNameToCheck)
                {
                    return true;
                }
            }
            return false;
        }


        public static void AddPermanentCharm(this AIActor actor, GameActorCharmEffect charm = null)
        {
            if (charm == null)
            {
                actor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
            }
            else
            {
                actor.ApplyEffect(charm, 1f, null);
            }
            actor.gameObject.AddComponent<KillOnRoomClear>();
            actor.IsHarmlessEnemy = true;
            actor.IgnoreForRoomClear = true;
            if (actor.gameObject.GetComponent<SpawnEnemyOnDeath>())
            {
                UnityEngine.Object.Destroy(actor.gameObject.GetComponent<SpawnEnemyOnDeath>());
            }
        }
        public static void RemovePickupFromLootTables(this PickupObject po)
        {
            WeightedGameObject go1 = GameManager.Instance.RewardManager.GunsLootTable.defaultItemDrops.FindWeightedGameObjectInCollection(po);
            if (go1 != null)
            {
                GameManager.Instance.RewardManager.GunsLootTable.defaultItemDrops.elements.Remove(go1);
            }
            WeightedGameObject go2 = GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.FindWeightedGameObjectInCollection(po);
            if (go2 != null)
            {
                GameManager.Instance.RewardManager.ItemsLootTable.defaultItemDrops.elements.Remove(go2);
            }
        }

        public static WeightedGameObject FindWeightedGameObjectInCollection(this WeightedGameObjectCollection collection, PickupObject po)
        {
            WeightedGameObject go = collection.FindWeightedGameObjectInCollection(po.PickupObjectId);
            if (go == null)
            {
                go = collection.FindWeightedGameObjectInCollection(po.gameObject);
            }
            return go;
        }

        public static WeightedGameObject FindWeightedGameObjectInCollection(this WeightedGameObjectCollection collection, int id)
        {
            foreach (WeightedGameObject go in collection.elements)
            {
                if (go.pickupId == id)
                {
                    return go;
                }
            }
            return null;
        }

        public static WeightedGameObject FindWeightedGameObjectInCollection(this WeightedGameObjectCollection collection, GameObject obj)
        {
            foreach (WeightedGameObject go in collection.elements)
            {
                if (go.gameObject == obj)
                {
                    return go;
                }
            }
            return null;
        }
    }

    class LabelablePlayerItem : PlayerItem
    {
        public void SetLabel(string label)
        {
            currentLabel = label;
        }
        public string currentLabel;
    }

    public static class UniqueStatModifiers
    {
        public static void AddStat(this PassiveItem item, PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            StatModifier statModifier = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };
            foreach (StatModifier statModifier2 in item.passiveStatModifiers)
            {
                bool flag = statModifier2.statToBoost == statType;
                if (flag)
                {
                    return;
                }
            }
            bool flag2 = item.passiveStatModifiers == null;
            if (flag2)
            {
                item.passiveStatModifiers = new StatModifier[]
                {
                    statModifier
                };
                return;
            }
            item.passiveStatModifiers = item.passiveStatModifiers.Concat(new StatModifier[]
            {
                statModifier
            }).ToArray<StatModifier>();
        }

        public static void RemoveStat(this PassiveItem item, PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            for (int i = 0; i < item.passiveStatModifiers.Length; i++)
            {
                bool flag = item.passiveStatModifiers[i].statToBoost != statType;
                if (flag)
                {
                    list.Add(item.passiveStatModifiers[i]);
                }
            }
            item.passiveStatModifiers = list.ToArray();
        }

        public static void BetterDoToEnemiesInRadius(this RoomHandler room, Vector2 position, float radius, Action<AIActor> action)
        {
            foreach (AIActor actor in room.GetActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                if (actor && actor.healthHaver && actor.specRigidbody && actor.IsNormalEnemy)
                {
                    PixelCollider hitboxPixelCollider = actor.specRigidbody.HitboxPixelCollider;
                    if (hitboxPixelCollider != null)
                    {
                        Vector2 vector = BraveMathCollege.ClosestPointOnRectangle(position, hitboxPixelCollider.UnitBottomLeft, hitboxPixelCollider.UnitDimensions);
                        float num = Vector2.Distance(actor.specRigidbody.UnitCenter, position);
                        float num2 = Vector2.Distance(vector, actor.specRigidbody.UnitCenter);
                        if (num < num2 + radius)
                        {
                            action(actor);
                        }
                    }
                }
            }
        }
    }

    public static class FileBullshit
    {
        public static void ExportTexture(Texture texture, string folder = "", string name = "")
        {
            string path = Path.Combine(ETGMod.ResourcesDirectory, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (string.IsNullOrEmpty(name))
            {
                File.WriteAllBytes(Path.Combine(path, texture.name + ".png"), ((Texture2D)texture).EncodeToPNG());
            }
            else
            {
                File.WriteAllBytes(Path.Combine(path, name + ".png"), ((Texture2D)texture).EncodeToPNG());
            }

        }
    }

    public static class HitboxMonitor
    {

        static float pixelsPerUnit = 16;

        public static void DisplayHitbox(SpeculativeRigidbody speculativeRigidbody)
        {
            PixelCollider collider = speculativeRigidbody.HitboxPixelCollider;
            Tools.Log("Collider Found...");
            if (!speculativeRigidbody.gameObject.GetComponent<HitBoxDisplay>())
                speculativeRigidbody.gameObject.AddComponent<HitBoxDisplay>();
            Tools.Log("Displaying...");
            LogHitboxInfo(collider);
        }

        public static void DeleteHitboxDisplays()
        {
            foreach (var comp in GameObject.FindObjectsOfType<HitBoxDisplay>())
            {
                GameObject.Destroy(comp);
            }
        }

        public class HitBoxDisplay : BraveBehaviour
        {
            GameObject hitboxDisplay = null;
            PixelCollider collider;
            void Start()
            {
                CreateDisplay();
            }

            public void CreateDisplay()
            {
                collider = base.specRigidbody.HitboxPixelCollider;
                string displayerName = "HitboxDisplay";

                if (hitboxDisplay == null)
                    hitboxDisplay = GameObject.CreatePrimitive(PrimitiveType.Cube);

                hitboxDisplay.GetComponent<Renderer>().material.color = new Color(1, 0, 1, .75f);
                hitboxDisplay.name = displayerName;
                hitboxDisplay.transform.SetParent(specRigidbody.transform);
            }

            void FixedUpdate()
            {
                hitboxDisplay.transform.localScale = new Vector3(
                    collider.Dimensions.x / pixelsPerUnit,
                    collider.Dimensions.y / pixelsPerUnit,
                    1f
                );
                Vector3 offset = new Vector3(
                    collider.Offset.x + collider.Dimensions.x * 0.5f,
                    collider.Offset.y + collider.Dimensions.y * 0.5f,
                    -pixelsPerUnit
                );
                offset /= pixelsPerUnit;
                hitboxDisplay.transform.localPosition = offset;
            }

            protected override void OnDestroy()
            {
                if (hitboxDisplay)
                    GameObject.DestroyImmediate(hitboxDisplay);
            }
        }

        private static void LogHitboxInfo(PixelCollider collider)
        {
            Tools.Print($"Dimensions: ({collider.Dimensions.x},{collider.Dimensions.y})");
            Tools.Print($"Offset: ({collider.Offset.x},{collider.Offset.y})");
        }
    }
}
