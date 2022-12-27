using System.Collections.Generic;
using OpenGL_Game.Objects;
using System.Diagnostics;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    public class EntityManager
    {
        List<Entity> entityList;

        public EntityManager()
        {
            entityList = new List<Entity>();
        }

        public void AddEntity(Entity entity)
        {
            Entity result = FindEntity(entity.Name);
            Debug.Assert(result == null, "Entity '" + entity.Name + "' already exists");
            entityList.Add(entity);
        }

        private Entity FindEntity(string name)
        {
            return entityList.Find(delegate(Entity e)
            {
                return e.Name == name;
            }
            );
        }

        public void RemoveEntity(Entity pEntity)
        {
            foreach (IComponent c in pEntity.Components)
            {
                if (c.ComponentType == ComponentTypes.COMPONENT_AUDIO)
                {
                    ComponentAudio ca = (ComponentAudio)c;
                    if(ca.mIsLooping)
                    {
                        c.Close();
                    }
                }
            }
            entityList.Remove(pEntity);
        }

        public List<Entity> Entities()
        {
            return entityList;
        }
    }
}
