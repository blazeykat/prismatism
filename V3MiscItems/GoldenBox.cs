using Dungeonator;
using ItemAPI;
using MonoMod.RuntimeDetour;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace katmod
{
    class GoldenBox : PassiveItem
    {
        public static void Init()
        {
            string name = "Golden Box";
            string resourcePath = "katmod/Resources/V3MiscItems/goldchest.png";
            GameObject gameObject = new GameObject(name);
            GoldenBox item = gameObject.AddComponent<GoldenBox>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Think Outside";
            string longDesc = "Doubles all chest items, you can only pick one.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(525);
            GameManager.Instance.RainbowRunForceExcludedIDs.Add(item.PickupObjectId);
            Hook hook = new Hook(typeof(Chest).GetMethod("SpewContentsOntoGround", BindingFlags.NonPublic | BindingFlags.Instance), typeof(GoldenBox).GetMethod("OnPostOpenBullshit"));
        }

        public static void OnPostOpenBullshit(Action<Chest, List<Transform>> orig, Chest self, List<Transform> transrights)
        {
            try
            {
                List<DebrisObject> list = new List<DebrisObject>();
                bool isRainbowRun = GameStatsManager.Instance.IsRainbowRun;
                Type chesttype = typeof(Chest);
                FieldInfo _forceDropOkayForRainbowRun = chesttype.GetField("m_forceDropOkayForRainbowRun", BindingFlags.NonPublic | BindingFlags.Instance);
                if (isRainbowRun && !self.IsRainbowChest && !(bool)_forceDropOkayForRainbowRun.GetValue(self))
                {
                    Vector2 a;
                    if (self.spawnTransform != null)
                    {
                        a = self.spawnTransform.position;
                    }
                    else
                    {
                        Bounds bounds = self.sprite.GetBounds();
                        a = self.transform.position + bounds.extents;
                    }
                    FieldInfo _room = chesttype.GetField("m_room", BindingFlags.NonPublic | BindingFlags.Instance);
                    LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteChest, a + new Vector2(-0.5f, -2.25f), (RoomHandler)_room.GetValue(self), true);
                }
                else
                {
                    for (int i = 0; i < self.contents.Count; i++)
                    {
                        List<DebrisObject> list2 = LootEngine.SpewLoot(new List<GameObject>
                {
                    self.contents[i].gameObject
                }, transrights[i].position);
                        list.AddRange(list2);
                        for (int j = 0; j < list2.Count; j++)
                        {
                            if (list2[j])
                            {
                                list2[j].PreventFallingInPits = true;
                            }
                            if (!(list2[j].GetComponent<Gun>() != null))
                            {
                                if (!(list2[j].GetComponent<CurrencyPickup>() != null))
                                {
                                    if (list2[j].specRigidbody != null)
                                    {
                                        list2[j].specRigidbody.CollideWithOthers = false;
                                        DebrisObject debrisObject = list2[j];
                                        MethodInfo _BecomeViableItem = chesttype.GetMethod("BecomeViableItem", BindingFlags.NonPublic | BindingFlags.Instance);
                                        debrisObject.OnTouchedGround += (Action<DebrisObject>)_BecomeViableItem.Invoke(self, new object[] { debrisObject });
                                    }
                                }
                            }
                        }
                    }
                }
                if (self.IsRainbowChest && isRainbowRun && self.transform.position.GetAbsoluteRoom() == GameManager.Instance.Dungeon.data.Entrance)
                {
                    MethodInfo _HandleRainbowRunLootProcessing = chesttype.GetMethod("HandleRainbowRunLootProcessing", BindingFlags.NonPublic | BindingFlags.Instance);
                    GameManager.Instance.Dungeon.StartCoroutine((IEnumerator)_HandleRainbowRunLootProcessing.Invoke(self, new object[] { list }));
                }
                if (god.ContainsKey(self))
                {
                    List<Tuple<int, int>> choices = god[self];
                    foreach (Tuple<int, int> choice in choices)
                    {
                        DebrisObject firstItem = GetItemFromListByID(list, choice.First);
                        DebrisObject secondItem = GetItemFromListByID(list, choice.Second);
                        if (firstItem && secondItem)
                        {
                            GameManager.Instance.Dungeon.StartCoroutine(ItemChoiceCoroutine(firstItem, secondItem));
                        }
                    }
                } 

            }
            catch (Exception error)
            {
                ETGModConsole.Log(error.ToString());
            }
        }

        public static IEnumerator ItemChoiceCoroutine(DebrisObject one, DebrisObject two)
        {
            for (; ; )
            {
                if (!one)
                {
                    LootEngine.DoDefaultItemPoof(two.transform.position);
                    Destroy(two.gameObject);
                    yield break;
                }
                if (!two)
                {
                    LootEngine.DoDefaultItemPoof(one.transform.position);
                    Destroy(one.gameObject);
                    yield break;
                }
                yield return null;
            }
        }

        public void MoreItems(Chest chest)
        {
            if (!GameStatsManager.Instance.IsRainbowRun && chest && !chest.IsMimic)
            {
                chest.PredictContents(Owner);
                List<PickupObject> items = new List<PickupObject>();
                List<Tuple<int, int>> choices = new List<Tuple<int, int>>();
                foreach (PickupObject pickup in chest.contents)
                {
                    PickupObject awesome = BoxOTools.GetTotallyRandomItem(pickup.quality, false);
                    choices.Add(new Tuple<int, int>(pickup.PickupObjectId, awesome.PickupObjectId));
                    items.Add(awesome);
                }
                god.Add(chest, choices);
                chest.contents.AddRange(items);
            }
        }

        private static DebrisObject GetItemFromListByID(List<DebrisObject> list, int id)
        {
            foreach (DebrisObject item in list)
            {
                if (item)
                {
                    ETGModConsole.Log(item.gameObject.name);
                    if (item.GetComponent<PassiveItem>() && item.GetComponent<PassiveItem>().PickupObjectId == id)
                    {
                        return item;
                    }
                    if (item.GetComponent<PlayerItem>() && item.GetComponent<PlayerItem>().PickupObjectId == id)
                    {
                        return item;
                    }
                    if (item.GetComponentInChildren<Gun>() && item.GetComponentInChildren<Gun>().PickupObjectId == id)
                    {
                        return item;
                    }
                    if (item.GetComponent<PickupObject>() && item.GetComponent<PickupObject>().PickupObjectId == id)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        protected override void Update()
        {
            base.Update();
            if (Owner)
            {
                List<Chest> chests = StaticReferenceManager.AllChests;
                if (chests != lastChests)
                {
                    lastChests = chests;
                    foreach (Chest chest in lastChests)
                    {
                        if (chest && chest.gameObject && chest.gameObject.GetComponent<ChestCheck>() == null)
                        {
                            chest.gameObject.AddComponent<ChestCheck>();
                            MoreItems(chest);
                        }
                    }
                }
            }
        }

        private List<Chest> lastChests = new List<Chest>();

        private class ChestCheck : MonoBehaviour
        {

        }

        private static Dictionary<Chest, List<Tuple<int, int>>> god = new Dictionary<Chest, List<Tuple<int, int>>>();
    }
}
