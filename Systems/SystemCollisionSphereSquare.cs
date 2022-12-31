using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.OBJLoader;
using OpenGL_Game.Objects;
using OpenGL_Game.Scenes;
using OpenTK.Audio.OpenAL;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Systems
{
    class SystemCollisionSphereSquare : ISystem
    {
        const ComponentTypes SPHEREMASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);
        const ComponentTypes SQUAREMASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SQUARE);
        CollisionManager mCollisionManager;

        public SystemCollisionSphereSquare(CollisionManager pCM)
        {
            this.mCollisionManager = pCM;
        }

        public string Name
        {
            get { return "SystemCollisionSphere"; }
        }

        public void OnAction(List<Entity> entities)
        {
            foreach(Entity entity1 in entities)
            {
                if ((entity1.Mask & SPHEREMASK) == SPHEREMASK && entity1.Name == "player" || entity1.Name.Contains("Bullet"))
                {
                    foreach(Entity entity2 in entities)
                    {
                        if(entity1 != entity2 && (entity2.Mask & SQUAREMASK) == SQUAREMASK)
                        {
                            Collision(entity1, entity2);
                        }
                    }
                }
            }
        }

        public void Collision(Entity pEntity1, Entity pEntity2)
        {
            ComponentPosition entity1Position;
            ComponentCollisionSphere entity1Sphere;
            RetrieveComponents(pEntity1.Components, out entity1Position, out entity1Sphere);

            ComponentPosition entity2Position;
            ComponentCollisionSquare entity2Square;
            RetrieveComponents(pEntity2.Components, out entity2Position, out entity2Square);

            float xDistance = Math.Abs(entity1Position.Position.X - entity2Position.Position.X);
            float zDistance = Math.Abs(entity1Position.Position.Z - entity2Position.Position.Z);

            if (xDistance >= (entity2Square.Width + entity1Sphere.Radius)) { return; } //no collision... sorry!
            if (zDistance >= (entity2Square.Depth + entity1Sphere.Radius)) { return; }

            mCollisionManager.RegisterCollision(pEntity1, pEntity2, COLLISION_TYPE.SPHERE_SQUARE);
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
    }
}
