using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentDamage : IComponent
    {
        int mDamage;

        public ComponentDamage(int pDamage)
        {
            mDamage = pDamage;
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_DAMAGE; }
        }

        public int Damage
        {
            get { return mDamage; }
            set { mDamage = value; }
        }

        public void Close()
        {
        }
    }
}
