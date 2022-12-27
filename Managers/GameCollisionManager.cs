using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;
using System.Collections.Generic;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        public GameCollisionManager(EntityManager pEntityManager) : base(pEntityManager)
        {

        }

        public override void ProcessCollision()
        {
            foreach(Collision col in mCollisionManifold)
            {
                if(col.collisionType == COLLISION_TYPE.SPHERE_SPHERE)
                {
                    SphereSphere(col.entity1, col.entity2);
                }
            }

            ClearManifold();
        }

        public void SphereSphere(Entity pEntity1, Entity pEntity2)
        {
            if(pEntity2.Name == "HealthPowerUp" && pEntity1.Name == "player")
            {
                HealthPowerUpCollect(pEntity1, pEntity2);
            }
            else if (pEntity2.Name == "SpeedPowerUp" && pEntity1.Name == "player")
            {
                SpeedPowerUpCollect(pEntity1, pEntity2);
            }
            else if (pEntity2.Name == "DamagePowerUp" && pEntity1.Name == "player")
            {
                DamagePowerCollect(pEntity2);
            }
            else if (pEntity2.Name.Contains("Bullet") && pEntity1.Name.Contains("enemy"))
            {
                DealDamage(pEntity1, pEntity2);
            }
        }

        private void HealthPowerUpCollect(Entity pEntityPlayer, Entity pEntityPowerUp)
        {
            IComponent healthComponent = pEntityPlayer.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
            });
            ComponentHealth health = (ComponentHealth)healthComponent;

            IComponent audioComponent = pEntityPowerUp.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
            });
            ComponentAudio audio = (ComponentAudio)audioComponent;

            audio.PlayAudio();

            health.Health += 1;

            mEntityManager.RemoveEntity(pEntityPowerUp);
        }

        private void SpeedPowerUpCollect(Entity pEntityPlayer, Entity pEntityPowerUp)
        {
            IComponent speedComponent = pEntityPlayer.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_SPEED;
            });
            ComponentSpeed speed = (ComponentSpeed)speedComponent;

            IComponent audioComponent = pEntityPowerUp.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
            });
            ComponentAudio audio = (ComponentAudio)audioComponent;

            audio.PlayAudio();

            speed.Speed += 0.05f;

            mEntityManager.RemoveEntity(pEntityPowerUp);
        }

        private void DamagePowerCollect(Entity pEntityPowerUp)
        {
            if (mEntityManager != null)
            {
                foreach (Entity e in mEntityManager.Entities())
                {
                    if(e.Name == "bullet")
                    {
                        IComponent damageComponent = e.Components.Find(delegate (IComponent component)
                        {
                            return component.ComponentType == ComponentTypes.COMPONENT_DAMAGE;
                        });
                        ComponentDamage damage = (ComponentDamage)damageComponent;

                        IComponent audioComponent = pEntityPowerUp.Components.Find(delegate (IComponent component)
                        {
                            return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                        });
                        ComponentAudio audio = (ComponentAudio)audioComponent;

                        audio.PlayAudio();
                        damage.Damage += 2;
                    }
                }
            }

            mEntityManager.RemoveEntity(pEntityPowerUp);
        }

        private void DealDamage(Entity pDamageReceiver, Entity pDamageGiver)
        {
            ComponentHealth health = null;
            if (pDamageReceiver.Name.Contains("enemy") && pDamageGiver.Name.Contains("Bullet"))
            {
                IComponent healthComponent = pDamageReceiver.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                });
                health = (ComponentHealth)healthComponent;

                IComponent damageComponent = pDamageGiver.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_DAMAGE;
                });
                ComponentDamage damage = (ComponentDamage)damageComponent;

                health.Health -= damage.Damage;

                mEntityManager.RemoveEntity(pDamageGiver);
            }
            else if(pDamageReceiver.Name.Contains("player") && pDamageGiver.Name.Contains("enemy"))
            {
                    IComponent healthComponent = pDamageReceiver.Components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                    });
                    health = (ComponentHealth)healthComponent;

                    IComponent damageComponent = pDamageGiver.Components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_DAMAGE;
                    });
                    ComponentDamage damage = (ComponentDamage)damageComponent;

                    health.Health -= damage.Damage;

                    mEntityManager.RemoveEntity(pDamageGiver);
            }
            else
            {
                return;
            }

            if (health.Health <= 0)
            {
                IComponent soundComponent = pDamageReceiver.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio = (ComponentAudio)soundComponent;
                audio.PlayAudio();
                mEntityManager.RemoveEntity(pDamageReceiver);
            }
        }
    }
}
