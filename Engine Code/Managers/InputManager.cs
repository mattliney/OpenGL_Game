using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;
using System.Collections.Generic;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using System.Diagnostics;
using OpenGL_Game.Managers;
using OpenGL_Game.Game_Code.Managers;

namespace OpenGL_Game.Game_Code.Managers
{
    abstract class InputManager
    {
        protected Dictionary<Key, string> keyboardBinds = new Dictionary<Key, string>();
        protected Dictionary<MouseButton, string> mouseBinds = new Dictionary<MouseButton, string>();

        public abstract void ProcessInputs(SceneManager pSceneManager, Camera pCamera, EntityManager pEntityManager);

        protected abstract void HandleInput(string pValue, Camera pCamera, SceneManager pSceneManager);
    }
}
