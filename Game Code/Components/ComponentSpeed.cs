using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentSpeed : IComponent
    {
        float mSpeed;

        public ComponentSpeed(float pSpeed)
        {
            mSpeed = pSpeed;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SPEED; }
        }

        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }

        public void Close()
        {
        }
    }
}
