using OpenTK;
using OpenGL_Game.Managers;
using System;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.OBJLoader;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    public class ComponentCollisionSquare : IComponent
    {
        float mWidth;
        float mHeight;
        float mDepth;

        public ComponentCollisionSquare(float pXLength, float pYLength, float pZLength)
        {
            mWidth = pXLength;
            mHeight = pYLength;
            mDepth = pZLength;
        }

        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }

        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }

        public float Depth
        {
            get { return mDepth; }
            set { mDepth = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_SQUARE; }
        }

        public void Close()
        {

        }
    }
}
