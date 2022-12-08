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
        public GameCollisionManager() {  }

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
            IComponent positionComponent = pEntity1.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });
            ComponentPosition e1Pos = (ComponentPosition)positionComponent;

            //e1Pos.Position += new Vector3(0.1f, 0, 0);

            return;
        }
    }
}
