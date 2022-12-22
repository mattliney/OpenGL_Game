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
        }

        private void HealthPowerUpCollect(Entity pEntityPlayer, Entity pEntityPowerUp)
        {
            IComponent healthComponent = pEntityPlayer.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
            });
            ComponentHealth health = (ComponentHealth)healthComponent;

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

            speed.Speed += 0.5f;

            mEntityManager.RemoveEntity(pEntityPowerUp);
        }
    }
}
