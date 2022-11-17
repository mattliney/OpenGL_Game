using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentCollisionSphere : IComponent
    {
        int mRadius;

        public ComponentCollisionSphere(int pRadius)
        {
            mRadius = pRadius;
        }

        public int Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_SPHERE; }
        }

        public void Close() { }
    }
}
