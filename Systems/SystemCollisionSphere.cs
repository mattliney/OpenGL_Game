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

namespace OpenGL_Game.Systems
{
    class SystemCollisionSphere : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_COLLISION_SPHERE);

        public SystemCollisionSphere()
        {
        }

        public string Name
        {
            get { return "SystemCollision"; }
        }

        public void OnAction(List<Entity> entities)
        {
            foreach(Entity entity1 in entities)
            {
                if ((entity1.Mask & MASK) == MASK)
                {
                    List<IComponent> components1 = entity1.Components;
                    ComponentPosition entity1Position;
                    ComponentCollisionSphere entity1Sphere;
                    RetrieveComponents(components1, out entity1Position, out entity1Sphere);

                    foreach(Entity entity2 in entities)
                    {
                        if(entity1 != entity2 && (entity2.Mask & MASK) == MASK)
                        {
                            List<IComponent> components2 = entity2.Components;
                            ComponentPosition entity2Position;
                            ComponentCollisionSphere entity2Sphere;
                            RetrieveComponents(components2, out entity2Position, out entity2Sphere);

                            Collision(entity1Position, entity1Sphere, entity2Position, entity2Sphere);
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

            IComponent audioComponent = pComponents.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_COLLISION_SPHERE;
            });
            sphere = (ComponentCollisionSphere)audioComponent;
        }

        public void Collision(ComponentPosition pEntity1Position, ComponentCollisionSphere pEntity1Sphere, ComponentPosition pEntity2Position, ComponentCollisionSphere pEntity2Sphere)
        {
            // do something
            //tell collision manager (abstract). it will have a default response. (sphere sphere)
            //what will actually happen is overloaded version of response will be in game specific collision manager. (shpere sphere) whatever
        }
    }
}
