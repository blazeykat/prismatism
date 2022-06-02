/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using UnityEngine;
using System.Reflection;

namespace katmod
{
    class PrismaticLexicon : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Prismatic Lexicon";
            string resourceName = "katmod/Resources/V3MiscItems/lemonpig";
            GameObject obj = new GameObject();
            PrismaticLexicon item = obj.AddComponent<PrismaticLexicon>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "dont look at my lore you ghouls";
            string longDesc = "bngh;
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psm");
            item.quality = ItemQuality.S;
            item.PlaceItemInAmmonomiconAfterItemById(451);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnEnteredCombat += enter;
        }

        public void enter()
        {
            int RandomItem = UnityEngine.Random.Range(0, PrismaticItem.items.Count);
            MethodInfo method = PrismaticItem.items[RandomItem].GetMethod("OnPickupModifiers", BindingFlags.Public | BindingFlags.Instance);
            object[] args = new object[]
            {
                Owner
            };
            this.gameObject.AddComponent(PrismaticItem.items[RandomItem]);
            method.Invoke(this.gameObject.GetComponent(PrismaticItem.items[RandomItem]), args);
        }
    }
}*/
