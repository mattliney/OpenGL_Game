using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentHealth : IComponent
    {
        int mHealth;

        public ComponentHealth(int pHealth)
        {
            mHealth = pHealth;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_HEALTH; }
        }

        public int Health
        {
            get { return mHealth; }
            set { mHealth = value; }
        }

        public void Close()
        {
        }
    }
}
