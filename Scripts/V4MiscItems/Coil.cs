using ItemAPI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace katmod
{
    class Coil : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Coil";
            string resourceName = "katmod/Resources/V3MiscItems/2ofhearts";
            GameObject obj = new GameObject();
            Coil item = obj.AddComponent<Coil>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Have a heart";
            string longDesc = "Increases the chance for hearts and armor to spawn from room clear, increases coolness.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc);
            item.quality = ItemQuality.B;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.GetComponent<PrismaticEvents>().OnEnterAnyRoom += Coil_OnEnterAnyRoom;
        }

        private void Coil_OnEnterAnyRoom(PlayerController arg1, Dungeonator.RoomHandler arg2)
        {
            if (arg1 && arg2 != null)
            {
                m_cable = arg1.gameObject.GetOrAddComponent<CoilHelper>();
                m_cable.Initialize(arg1.specRigidbody);
            }
        }

        private CoilHelper m_cable;

        private class CoilHelper : MonoBehaviour
        {
            public void Initialize(SpeculativeRigidbody rigbody)
            {
                m_cables = new List<ArbitraryCableDrawer>();
                GameObject thisIsCrazy = new GameObject("cableHelper");
                GameObject superCrazy = UnityEngine.Object.Instantiate<GameObject>(thisIsCrazy, rigbody.UnitCenter, Quaternion.identity);
                ArbitraryCableDrawer cable = superCrazy.gameObject.GetOrAddComponent<ArbitraryCableDrawer>();
                cable.Initialize(rigbody.transform, superCrazy.transform);
                m_cables.Add(cable);

                m_rigidbody = rigbody;

                cable.GetComponent<Mesh>().SetColors(new List<Color>() { Color.red });
            }

            protected SpeculativeRigidbody m_rigidbody;

            void Update()
            {
                try
                {
                    if (m_cables.Count > 0)
                    {
                        ArbitraryCableDrawer currentCable = m_cables[m_cables.Count - 1];

                        int thePointOfTheMaskIs = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.PlayerBlocker);
                        Vector2 position = currentCable.Attach2.position;
                        Vector2 position2 = currentCable.Attach1.position;

                        Vector2 direction = position2 - position;
                        float distance = Vector2.Distance(position, position2);

                        if (PhysicsEngine.Instance.Raycast(position, direction, distance, out RaycastResult result, true, true, thePointOfTheMaskIs) && result != null && result.Contact != null)
                        {
                            ETGModConsole.Log("is raycast hittin");
                            GameObject thisIsCrazy = new GameObject($"cableHelper");
                            GameObject superCrazy = UnityEngine.Object.Instantiate<GameObject>(thisIsCrazy, result.Contact, Quaternion.identity);

                            currentCable.Attach1 = superCrazy.transform;


                            ArbitraryCableDrawer cable = superCrazy.gameObject.AddComponent<ArbitraryCableDrawer>();
                            cable.Initialize(m_rigidbody.transform, superCrazy.transform);
                            m_cables.Add(cable);
                        }
                        else
                        {
                            ETGModConsole.Log("dumb baby");
                        }
                    }
                    ETGModConsole.Log(m_cables.Count.ToString());
                } catch (Exception e) { ETGModConsole.Log(e.ToString()); }
            }

            private List<ArbitraryCableDrawer> m_cables;
        }

    }
}
