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
    class SystemPlayer : ISystem
    {
        // Updates the position of the player based on the camera.
        // Also makes the skybox move with the player (essentially making it appear to not move)

        Camera mCamera;
        static string mPlayerName = "player";
        const ComponentTypes MASK = ComponentTypes.COMPONENT_POSITION;

        public SystemPlayer(Camera pCamera)
        {
            mCamera = pCamera;
        }

        public string Name
        {
            get { return "SystemPlayer"; }
        }

        public void OnAction(List<Entity> entities)
        {
            foreach(Entity entity in entities)
            {
                if((entity.Mask & MASK) == MASK)
                {
                    if(entity.Name == mPlayerName)
                    {
                        IComponent positionComponent = entity.Components.Find(delegate (IComponent component)
                        {
                            return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                        });
                        ComponentPosition position = (ComponentPosition)positionComponent;

                        MoveCamera(position);
                    }
                }
            }
        }

        public void MoveCamera(ComponentPosition pPosition)
        {
            pPosition.Position = mCamera.cameraPosition;
        }
    }
}
