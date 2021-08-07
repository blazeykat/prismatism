using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    class GameActorUnweaveEffect : GameActorEffect
    {

        public int maxAmount;

        public float increments;

        public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1)
        {
            try
            {
            actor.healthHaver.AllDamageMultiplier += initalAmount;
            counter.HowMuchToRemove += initalAmount;
            counter.TimesApplied++;
                counter = actor.gameObject.AddComponent<CountingComponent>();
            } catch (Exception error)
            {
                error.ToString().Log();
            }
        }

        public void IncreaseIfAmountNotHit(GameActor actor)
        {
            try {
            if (counter.TimesApplied <= maxAmount)
            {
                actor.healthHaver.AllDamageMultiplier += increments;
                counter.HowMuchToRemove += increments;
                counter.TimesApplied++;
                }
            }
            catch (Exception error)
            {
                error.ToString().Log();
            }
        }

        public float initalAmount;


        public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
        {
            try
            {
                actor.healthHaver.AllDamageMultiplier -= counter.HowMuchToRemove;
            } catch (Exception error)
            {
                error.ToString().Log();
            }
        }

        public CountingComponent counter = new CountingComponent();
    }


    public class CountingComponent : MonoBehaviour
    {
        public float HowMuchToRemove;

        public int TimesApplied;// { get; private set; }
    }
}
