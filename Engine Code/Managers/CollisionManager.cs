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
    public enum COLLISION_TYPE
    {
        SPHERE_SPHERE,
        SPHERE_SQUARE
    }
    public struct Collision
    {
        public Entity entity1;
        public Entity entity2;
        public COLLISION_TYPE collisionType;
    }

    public abstract class CollisionManager
    {
        protected List<Collision> mCollisionManifold = new List<Collision>();
        protected EntityManager mEntityManager;

        public CollisionManager(EntityManager pEntityManager)
        {
            mEntityManager = pEntityManager;
        }

        public void ClearManifold()
        {
            mCollisionManifold.Clear();
        }

        //Adds collisions to manifold. Responds in GameCollisionManager
        public void RegisterCollision(Entity pEntity1, Entity pEntity2, COLLISION_TYPE pCollisionType)
        {
            foreach(Collision c in mCollisionManifold)
            {
                //Checks to see if the collision is already in the manifold.
                //It is necessary to do it in reverse or the collisions will register twice (not ideal)
                if(c.entity1 == pEntity1 && c.entity2 == pEntity2)
                {
                    return;
                }
                else if(c.entity2 == pEntity1 && c.entity1 == pEntity2) 
                {
                    return;
                }
            }

            Collision col;
            col.entity1 = pEntity1;
            col.entity2 = pEntity2;
            col.collisionType = pCollisionType;

            mCollisionManifold.Add(col);
        }

        public abstract void ProcessCollision();
    }
}
