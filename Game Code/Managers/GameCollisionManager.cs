using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;
using System.Collections.Generic;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using System.Diagnostics;

namespace OpenGL_Game.Managers
{
    public class GameCollisionManager : CollisionManager
    {
        Stopwatch mPlayerHurtTimer;
        Camera mPlayerCamera;
        List<Entity> mNodes;
        public GameCollisionManager(EntityManager pEntityManager, Camera pCamera) : base(pEntityManager)
        {
            mPlayerHurtTimer = new Stopwatch();
            mPlayerHurtTimer.Start();
            mPlayerCamera = pCamera;

            mNodes = new List<Entity>();
            foreach (Entity e in pEntityManager.Entities())
            {
                if(e.Name.Contains("node"))
                {
                    mNodes.Add(e);
                }
            }
        }

        public override void ProcessCollision()
        {
            foreach(Collision col in mCollisionManifold)
            {
                if(col.collisionType == COLLISION_TYPE.SPHERE_SPHERE)
                {
                    SphereSphere(col.entity1, col.entity2);
                }
                else if(col.collisionType == COLLISION_TYPE.SPHERE_SQUARE)
                {
                    SphereSquare(col.entity1, col.entity2);
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
            else if (pEntity2.Name.Contains("player") && pEntity1.Name.Contains("enemy"))
            {
                DealDamage(pEntity2, pEntity1);
            }
            else if(pEntity2.Name.Contains("node") && pEntity1.Name.Contains("enemy"))
            {
                MoveNode(pEntity2, pEntity1);
            }
        }

        private void SphereSquare(Entity pEntity1, Entity pEntity2)
        {
            if (pEntity1.Name.Contains("Bullet"))
            {
                mEntityManager.RemoveEntity(pEntity1);
                return;
            }
            else if (mPlayerCamera.mDebugMode)
            {
                return;
            }
            ComponentPosition entity1Position;
            ComponentCollisionSphere entity1Sphere;
            RetrieveComponents(pEntity1.Components, out entity1Position, out entity1Sphere);

            ComponentPosition entity2Position;
            ComponentCollisionSquare entity2Square;
            RetrieveComponents(pEntity2.Components, out entity2Position, out entity2Square);

            float xDistance = Math.Abs(entity1Position.Position.X - entity2Position.Position.X);
            float zDistance = Math.Abs(entity1Position.Position.Z - entity2Position.Position.Z);

            if (xDistance < (entity2Square.Width))
            {
                if (entity1Position.Position.Z > entity2Position.Position.Z) // right
                {
                    mPlayerCamera.cameraPosition.Z = entity2Position.Position.Z + entity2Square.Depth + entity1Sphere.Radius;
                }
                else if (entity1Position.Position.Z < entity2Position.Position.Z) // left
                {
                    mPlayerCamera.cameraPosition.Z = entity2Position.Position.Z - entity2Square.Depth - entity1Sphere.Radius;
                }
            }

            if (zDistance < (entity2Square.Depth))
            {

                if (entity1Position.Position.X > entity2Position.Position.X) // front
                {
                    mPlayerCamera.cameraPosition.X = entity2Position.Position.X + entity2Square.Width + entity1Sphere.Radius;
                }
                else if (entity1Position.Position.X < entity2Position.Position.X) // back
                {
                    mPlayerCamera.cameraPosition.X = entity2Position.Position.X - entity2Square.Width - entity1Sphere.Radius;
                }
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

                IComponent soundComponent = pDamageReceiver.Components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                });
                ComponentAudio audio = (ComponentAudio)soundComponent;

                if (mPlayerHurtTimer.ElapsedMilliseconds >= 1000)
                {
                    health.Health -= 1;
                    audio.PlayAudio();
                    mPlayerHurtTimer.Restart();
                }
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

        public void RetrieveComponents(List<IComponent> pComponents, out ComponentPosition position, out ComponentCollisionSphere sphere)
        {
            IComponent positionComponent = pComponents.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            position = (ComponentPosition)positionComponent;

            IComponent collisionComponent = pComponents.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE;
            });
            sphere = (ComponentCollisionSphere)collisionComponent;
        }

        public void RetrieveComponents(List<IComponent> pComponents, out ComponentPosition position, out ComponentCollisionSquare square)
        {
            IComponent positionComponent = pComponents.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            position = (ComponentPosition)positionComponent;

            IComponent collisionComponent = pComponents.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SQUARE;
            });
            square = (ComponentCollisionSquare)collisionComponent;
        }

        private IComponent GetComponent(ComponentTypes pComponentType, Entity pEntity)
        {
            IComponent entityComponent = pEntity.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == pComponentType;
            });
            IComponent returnComponent = (IComponent)entityComponent;

            return returnComponent;
        }

        private void MoveNode(Entity pNode, Entity pEnemy)
        {
            int currentNodeIndex = int.Parse(pNode.Name.Split(' ')[1]);
            int nextNodeIndex = 0;

            if(currentNodeIndex == 0)
            {
                nextNodeIndex = mNodes.Count - 1;
            }
            else
            {
                nextNodeIndex = currentNodeIndex - 1;
            }

            ComponentPosition currentNodePos = (ComponentPosition)GetComponent(ComponentTypes.COMPONENT_POSITION, pNode);
            ComponentPosition otherNodePos = (ComponentPosition)GetComponent(ComponentTypes.COMPONENT_POSITION, mNodes[nextNodeIndex]);

            ComponentVelocity enemyVel = (ComponentVelocity)GetComponent(ComponentTypes.COMPONENT_VELOCITY, pEnemy);

            if(enemyVel == null)
            {
                return;
            }

            Vector3 dist = otherNodePos.Position - currentNodePos.Position;
            dist.Normalize();

            enemyVel.Velocity = dist;
        }
    }
}
