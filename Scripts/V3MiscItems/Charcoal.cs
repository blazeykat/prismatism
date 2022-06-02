using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace katmod
{
    class Charcoal : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Shoddy Lighter";
            string resourceName = "katmod/Resources/V3MiscItems/lighter";
            GameObject obj = new GameObject();
            Charcoal item = obj.AddComponent<Charcoal>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Backup Light";
            string longDesc = "Ignites nearby enemies upon killing an enemy";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.C;
            item.PlaceItemInAmmonomiconAfterItemById(442);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnKilledEnemyContext += KilledEnemy;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemyContext -= KilledEnemy;
            return base.Drop(player);
        }

        public void KilledEnemy(PlayerController player, HealthHaver killed)
        {
            List<AIActor> actor = player.CurrentRoom.GetActiveEnemies(Dungeonator.RoomHandler.ActiveEnemyType.All);
            for(int i = 0; i < actor.Count; i++)
            {
                if (actor[i] && killed && actor[i].healthHaver && killed.specRigidbody && actor[i].specRigidbody)
                {
                    if (Vector2.Distance(actor[i].CenterPosition, killed.specRigidbody.UnitCenter) <= 7)
                    {
                        actor[i].ApplyEffect(lame);
                    }
                }
            }
        }

        private GameActorFireEffect lame = Gungeon.Game.Items["hot_lead"].GetComponent<BulletStatusEffectItem>().FireModifierEffect;
    }
}
