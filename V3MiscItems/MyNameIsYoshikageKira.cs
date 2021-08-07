using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;

namespace katmod
{
    class MyNameIsYoshikageKira : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Deadly Charm";
            string resourceName = "katmod/Resources/V3MiscItems/bitez";
            GameObject obj = new GameObject();
            MyNameIsYoshikageKira item = obj.AddComponent<MyNameIsYoshikageKira>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Kablewy";
            string longDesc = "Blows up 3 enemies upon taking damage\n\nNot much is known about the person who owned this charm. However, what is known...\n\n...is that he's 33 years old, he lives in the Northeast section of a town in japan, where all the villas are, and he's not married. He works as an employee for a department store, and he get's home every day by 8PM atleast. He doesn't smoke, but he occasionally drinks. He gets in bed by 11PM, and he makes sure he gets eight hours of sleep, no matter what. After having a warm glass of milk, and doing about twenty minutes of stretches before going to bed, he usually haves no problems sleeping until morning. Just like a baby, he wakes without any fatigue or stress in the morning. He was told there was no issues at his last check up."
                + "\n\nWhat I'm trying to explain, is that he is a person who wishes to live a very quiet life. I take care not to trouble myself with enemies, like winning and losing, that would cause me to lose sleep at night. That is how he deals with society, and he knows that is what brings him happiness.\n\nHowever, if he were to fight, he wouldn't lose to anyone.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.B;
            item.PlaceItemInAmmonomiconAfterItemById(817);
        }

        public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += OnDamaged;
            base.Pickup(player);
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= OnDamaged;
            return base.Drop(player);
        }

        void OnDamaged(PlayerController player)
        {
            List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null && player.CurrentRoom != null && player.healthHaver && activeEnemies.Count != 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    int RandomEnemy = UnityEngine.Random.Range(0, activeEnemies.Count);
                    if (activeEnemies[RandomEnemy] != null && activeEnemies[RandomEnemy].specRigidbody)
                    {
                        Exploder.DoDefaultExplosion(activeEnemies[RandomEnemy].specRigidbody.UnitCenter, default);
                    }
                }
            }
        }
    }
}
