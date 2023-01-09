﻿using System;
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
    class SystemCollisionSphereSphere : ISystem
    {
        CollisionManager mCollisionManager;
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);

        public SystemCollisionSphereSphere(CollisionManager pCM)
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
                if ((entity1.Mask & MASK) == MASK)
                {

                    foreach(Entity entity2 in entities)
                    {
                        if(entity1 != entity2 && (entity2.Mask & MASK) == MASK)
                        {
                            Collision(entity1, entity2);
                        }
                    }
                }
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

        public void Collision(Entity pEntity1, Entity pEntity2)
        {
            ComponentPosition entity1Position;
            ComponentCollisionSphere entity1Sphere;
            RetrieveComponents(pEntity1.Components, out entity1Position, out entity1Sphere);

            ComponentPosition entity2Position;
            ComponentCollisionSphere entity2Sphere;
            RetrieveComponents(pEntity2.Components, out entity2Position, out entity2Sphere);

            //The calculation for sphere collision. The distance should be greater than the radius combined.
            if ((entity1Position.Position - entity2Position.Position).Length < entity1Sphere.Radius + entity2Sphere.Radius)
            {
                mCollisionManager.RegisterCollision(pEntity1, pEntity2, COLLISION_TYPE.SPHERE_SPHERE);
            }
        }

    }
}