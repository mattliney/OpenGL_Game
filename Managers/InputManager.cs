using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;

namespace OpenGL_Game.Managers
{
    class InputManager : GameWindow
    {
        public InputManager() { }

        public void ConfigureInputs(string pFileName)
        {
            StreamReader sr = new StreamReader(pFileName);

            string line;
            while(true)
            {
                line = sr.ReadLine();
                if (line == null) { return; } 
            }
        }
    }
}
