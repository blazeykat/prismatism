using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using Dungeonator;
using System.Collections;

namespace ItemAPI
{

    static class PlayerEventAdder
    {

        public static void Init()
        {
            Hook hook = new Hook(typeof(PlayerController).GetMethod("orig_Start", BindingFlags.Public | BindingFlags.Instance), typeof(PlayerEventAdder).GetMethod("AddComponent"));
            Hook hook2 = new Hook(typeof(RoomHandler).GetMethod("PlayerEnter", BindingFlags.Public | BindingFlags.Instance), typeof(PrismaticEvents).GetMethod("OnRoomEnter"));
            Hook hook3 = new Hook(typeof(PlayerConsumables).GetProperty("Currency", BindingFlags.Public | BindingFlags.Instance).GetSetMethod(), typeof(PrismaticEvents).GetMethod("OnCasingsChangedHook"));
            Hook updateLabelHook = new Hook(typeof(GameUIItemController).GetMethod("UpdateItem", BindingFlags.Instance | BindingFlags.Public), typeof(PlayerEventAdder).GetMethod("UpdateCustomLabel"));
        }

        public static void AddComponent(Action<PlayerController> action, PlayerController player)
        {
            action(player);
            player.gameObject.AddComponent<PrismaticEvents>();
        }

        public static void UpdateCustomLabel(Action<GameUIItemController, PlayerItem, List<PlayerItem>> orig, GameUIItemController self, PlayerItem current, List<PlayerItem> items)
        {
            orig(self, current, items);
            if (current && current is LabelablePlayerItem)
            {
                LabelablePlayerItem labelable = current as LabelablePlayerItem;
                if (!string.IsNullOrEmpty(labelable.currentLabel))
                {
                    self.ItemCountLabel.IsVisible = true;
                    self.ItemCountLabel.Text = labelable.currentLabel;
                }
            }
        }
    }




    public class PrismaticEvents : MonoBehaviour
    {
        public event Action<PlayerController, RoomHandler> OnEnterAnyRoom;

        public event Action<int> OnCasingsChanged;

        public static void OnRoomEnter(Action<RoomHandler, PlayerController> action, RoomHandler room, PlayerController player)
        {
            action(room, player);
            player.GetComponent<PrismaticEvents>().OnEnterAnyRoom?.Invoke(player, room);
            if (!player.GetComponent<PrismaticEvents>().EnteredRooms.Contains(room))
                player.GetComponent<PrismaticEvents>().EnteredRooms.Add(room);
        }

        public static void OnCasingsChangedHook(Action<PlayerConsumables, int> orig, PlayerConsumables self, int Money)
        {
            orig(self, Money);
            PlayerController player = GetPlayerFromConsumables(self);
            DaveAPIManager.UpdateMaximum(CustomTrackedMaximums.MOST_MONEY, Money);
            if (player && player.GetComponent<PrismaticEvents>())
            {
                player.GetComponent<PrismaticEvents>().OnCasingsChanged?.Invoke(Money);
            }
        }

        public static PlayerController GetPlayerFromConsumables(PlayerConsumables consumables)
        {
            if (GameManager.HasInstance && GameManager.Instance.AllPlayers != null)
            {
                return Array.Find(GameManager.Instance.AllPlayers, (PlayerController player) => player.carriedConsumables == consumables);
            }
            return null;
        }

        public List<RoomHandler> EnteredRooms = new List<RoomHandler>();
    }
}
