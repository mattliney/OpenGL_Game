using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL_Game.Objects;
using System.IO;
using OpenGL_Game.Components;

namespace OpenGL_Game.Managers
{
    abstract class ScriptManager
    {
        public ScriptManager() { }

        abstract public void LoadEntityList(string pFileName, EntityManager pEntityManager);

        abstract protected void ParseLine(string pLine, Entity pEntity);
    }
}
