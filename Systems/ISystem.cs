using OpenGL_Game.Objects;
using System.Collections.Generic;

namespace OpenGL_Game.Systems
{
    interface ISystem
    {
        void OnAction(List<Entity> entity);

        // Property signatures: 
        string Name
        {
            get;
        }
    }
}
