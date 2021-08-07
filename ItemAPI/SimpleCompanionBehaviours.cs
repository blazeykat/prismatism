using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ItemAPI
{
    static class SimpleCompanionBehaviours
    {
        public class SimpleCompanionApproach : MovementBehaviorBase
        {
            public SimpleCompanionApproach(int desiredDistance)
            {
                DesiredDistance = desiredDistance;
            }

            public override void Init(GameObject gameObject, AIActor aiActor, AIShooter aiShooter) { base.Init(gameObject, aiActor, aiShooter); }
            public override void Upkeep()
            {
                base.Upkeep();
                base.DecrementTimer(ref this.repathTimer, false);
            }
            public override BehaviorResult Update()
            {
                if (Owner == null)
                {
                    if (this.m_aiActor && this.m_aiActor.CompanionOwner)
                    {
                        Owner = this.m_aiActor.CompanionOwner;
                    }
                    else
                    {
                        Owner = GameManager.Instance.BestActivePlayer;
                    }
                }
                SpeculativeRigidbody overrideTarget = this.m_aiActor.OverrideTarget;
                BehaviorResult result;
                if (this.repathTimer > 0f)
                {
                    result = ((overrideTarget == null) ? BehaviorResult.Continue : BehaviorResult.SkipRemainingClassBehaviors);
                }
                else
                {
                    if (overrideTarget == null)
                    {
                        this.PickNewTarget();
                        result = BehaviorResult.Continue;
                    }
                    else
                    {
                        this.isInRange = (Vector2.Distance(this.m_aiActor.specRigidbody.UnitCenter, overrideTarget.UnitCenter) <= this.DesiredDistance);
                        if (overrideTarget != null && !this.isInRange)
                        {
                            this.m_aiActor.PathfindToPosition(overrideTarget.UnitCenter, null, true, null, null, null, false);
                            this.repathTimer = this.PathInterval;
                            result = BehaviorResult.SkipRemainingClassBehaviors;
                        }
                        else
                        {
                            if (overrideTarget != null && this.repathTimer >= 0f)
                            {
                                this.m_aiActor.ClearPath();
                                this.repathTimer = -1f;
                            }
                            result = BehaviorResult.Continue;
                        }
                    }
                }
                return result;
            }
            private void PickNewTarget()
            {
                if (this.m_aiActor != null)
                {
                    if (this.Owner == null)
                    {
                        if (this.m_aiActor && this.m_aiActor.CompanionOwner)
                        {
                            Owner = this.m_aiActor.CompanionOwner;
                        }
                        else
                        {
                            Owner = GameManager.Instance.BestActivePlayer;
                        }
                    }
                    this.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear, ref this.roomEnemies);
                    for (int i = 0; i < this.roomEnemies.Count; i++)
                    {
                        AIActor aiactor = this.roomEnemies[i];
                        if (aiactor.IsHarmlessEnemy || !aiactor.IsNormalEnemy || aiactor.healthHaver.IsDead || aiactor == this.m_aiActor || aiactor.EnemyGuid == "ba928393c8ed47819c2c5f593100a5bc")
                        { this.roomEnemies.Remove(aiactor); }
                    }
                    if (this.roomEnemies.Count == 0) { this.m_aiActor.OverrideTarget = null; }
                    else
                    {
                        AIActor aiActor = this.m_aiActor;
                        AIActor aiactor2 = this.roomEnemies[UnityEngine.Random.Range(0, this.roomEnemies.Count)];
                        aiActor.OverrideTarget = ((aiactor2 != null) ? aiactor2.specRigidbody : null);
                    }
                }
            }
            public float PathInterval = 0.25f;
            public float DesiredDistance;
            private float repathTimer;
            private List<AIActor> roomEnemies = new List<AIActor>();
            private bool isInRange;
            private PlayerController Owner;
        }
    }
}
