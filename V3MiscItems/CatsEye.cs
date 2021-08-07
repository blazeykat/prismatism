using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;
using System.Reflection;

namespace katmod
{
    class CatsEye : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Cats Eye";
            string resourceName = "katmod/Resources/V3MiscItems/catseye";
            GameObject obj = new GameObject();
            CatsEye item = obj.AddComponent<CatsEye>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "So Many Nights";
            string longDesc = "Entering a secret room heals you for 1 heart.\n\nA pin fashioned with the logo of a cat's eye.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.D;
            item.PlaceItemInAmmonomiconAfterItemById(817);
            item.SetupUnlockOnStat(TrackedStats.SECRET_ROOMS_FOUND, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN, 5);
            Prismatism.Unlocks.Add(item.PickupObjectId, "Find 5 secret rooms");
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.GetComponent<PrismaticEvents>().OnEnterAnyRoom += NewCoolAction;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.GetComponent<PrismaticEvents>().OnEnterAnyRoom -= NewCoolAction;
            return base.Drop(player);
        }

        public void NewCoolAction(PlayerController player, RoomHandler room)
        {
            if (room.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET && !player.GetComponent<PrismaticEvents>().EnteredRooms.Contains(room))
            {
                if (player.characterIdentity != PlayableCharacters.Robot) { player.healthHaver.ApplyHealing(1f); } else { player.healthHaver.Armor += 1; }
                AkSoundEngine.PostEvent("Play_OBJ_heart_heal_01", gameObject);
                player.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/vfx_healing_sparkles_001") as GameObject, Vector3.zero);
                if (player.PlayerHasActiveSynergy("Rumble of the Drums"))
                {
                    LootEngine.SpawnItem(PickupObjectDatabase.GetById(GlobalItemIds.AmmoPickup).gameObject, player.specRigidbody.UnitCenter, Vector2.zero, 1f, true, true, true);
                }
            }
        }
    }
}
