using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;

namespace katmod
{
    class PositiveEffects
    {
        public static void BasicPositiveEffectShit(string name, string lowerText, PlayerController player)
        {
            BoxOTools.Notify(name, lowerText, "katmod/Resources/Candies/happypilldude");
            AkSoundEngine.PostEvent("Play_OBJ_metronome_jingle_01", player.gameObject);
        }

        public static void FullHealth(PlayerController player)
        {
            BasicPositiveEffectShit("Full Health", "Healed to Full", player);
            if (player && player.healthHaver)
            {
                player.healthHaver.FullHeal();
                player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero);
            }
        }

        public static void ArmouredUp(PlayerController player)
        {
            BasicPositiveEffectShit("Rock Candy", "Armoured Up", player);
            if (player && player.healthHaver)
            {
                player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_item_pickup") as GameObject, Vector3.zero);
                player.healthHaver.Armor += 5;
            }
        }
    }
}
