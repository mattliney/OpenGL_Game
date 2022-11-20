using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;
using System.Collections.Generic;
using OpenGL_Game.Objects;

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

        }
    }
}
