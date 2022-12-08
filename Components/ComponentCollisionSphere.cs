using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentCollisionSphere : IComponent
    {
        float mRadius;

        public ComponentCollisionSphere(float pRadius)
        {
            mRadius = pRadius;
        }

        public float Radius
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
