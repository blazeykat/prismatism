using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace katmod
{
    public abstract class HitscanHelper : MonoBehaviour
    {
        private Projectile projectile;
        public GameObject linkVFXPrefab;
        public float wait;

        void Start()
        {
            projectile = gameObject.GetComponent<Projectile>();
        }

        void Update()
        {
            if (projectile)
            {
                Vector2 position = projectile.specRigidbody.UnitCenter;
                Vector2 direction = projectile.LastVelocity.normalized;
                int thePointOfTheMaskIs = CollisionMask.LayerToMask(CollisionLayer.HighObstacle, CollisionLayer.BulletBlocker, CollisionLayer.EnemyHitBox, CollisionLayer.BulletBreakable);
                PhysicsEngine.Instance.Raycast(position, direction, 100f, out RaycastResult result, true, true, thePointOfTheMaskIs);
                if (result != null && result.Contact != null)
                {
                    tk2dTiledSprite component = SpawnManager.SpawnVFX(linkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
                    PostBeamRender(component);

                    Vector2 unitCenter = position;
                    Vector2 unitCenter2 = result.Contact;
                    component.transform.position = unitCenter;
                    Vector2 vector = unitCenter2 - unitCenter;
                    float num = BraveMathCollege.Atan2Degrees(vector.normalized);
                    int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
                    component.dimensions = new Vector2((float)num2, component.dimensions.y);
                    component.transform.rotation = Quaternion.Euler(0f, 0f, num);
                    component.UpdateZDepth();

                    ETGMod.StartGlobalCoroutine(doTimerMagic(component.gameObject, wait));
                    OnBeamHit(result.Contact, projectile);
                }
                projectile.DieInAir();
            }
        }
        public virtual void PostBeamRender(tk2dTiledSprite beam)
        {

        }

        public abstract void OnBeamHit(Vector2 contact, Projectile projectile);

        private static IEnumerator doTimerMagic(GameObject beam, float wait)
        {
            for (int i = 0; i < wait; i++)
            {
                yield return null;
            }
            SpawnManager.Despawn(beam);
            yield break;
        }
    }

}
