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
    enum COLLISIONTYPE
    {
        SPHERE_SPHERE
    }
    struct Collision
    {
        Entity entity1;
        Entity entity2;
        COLLISIONTYPE collisionType;
    }

    class CollisionManager
    {
        List<Collision> mCollisionManifold = new List<Collision>();

        public CollisionManager() {  }
    }
}
