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
    class SystemAudio : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_AUDIO);

        public SystemAudio()
        {
        }

        public string Name
        {
            get { return "SystemAudio"; }
        }

        public void OnAction(List<Entity> entities)
        {
            foreach(Entity entity in entities)
            {
                if ((entity.Mask & MASK) == MASK)
                {
                    List<ComponentAudio> audioComponents = GetAudioComponents(entity);

                    List<IComponent> components = entity.Components;

                    IComponent positionComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                    });
                    ComponentPosition position = (ComponentPosition)positionComponent;

                    IComponent audioComponent = components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_AUDIO;
                    });
                    ComponentAudio audio = (ComponentAudio)audioComponent;

                    if(audioComponents.Count > 1)
                    {
                        SetPosition(position, audioComponents[0]);
                        if(audioComponents[1].mIsLooping && !audioComponents[1].mIsPlaying)
                        {
                            audioComponents[1].PlayAudio();
                            audioComponents[1].mIsPlaying = true;
                        }
                    }
                    else
                    {
                        SetPosition(position, audio);
                    }
                }
            }
        }

        public void SetPosition(ComponentPosition pPosition, ComponentAudio pAudio)
        {
            pAudio.SetPosition(pPosition.Position);
        }

        private List<ComponentAudio> GetAudioComponents(Entity pEntity)
        {
            List<ComponentAudio> audios = new List<ComponentAudio>();

            foreach(IComponent c in pEntity.Components)
            {
                if(c.ComponentType == ComponentTypes.COMPONENT_AUDIO)
                {
                    audios.Add((ComponentAudio)c);
                }
            }

            return audios;
        }
    }
}
