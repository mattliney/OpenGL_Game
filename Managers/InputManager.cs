using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;
using System.Collections.Generic;

namespace OpenGL_Game.Managers
{
    class InputManager
    {
        Dictionary<Key, string> keyboardBinds = new Dictionary<Key, string>();
        Dictionary<MouseButton, string> mouseBinds = new Dictionary<MouseButton, string>();

        public InputManager() {  }

        public void ProcessInputs(SceneManager pSceneManager, Camera pCamera)
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetState();

            foreach(KeyValuePair<Key, string> kvp in keyboardBinds)
            {
                if(ks.IsKeyDown(kvp.Key))
                {
                    HandleInput(kvp.Value, pCamera, pSceneManager);
                }
            }
            foreach (KeyValuePair<MouseButton, string> kvp in mouseBinds)
            {
                if (ms.IsButtonDown(kvp.Key))
                {
                    HandleInput(kvp.Value, pCamera, pSceneManager);
                }
            }

        }

        private void HandleInput(string pValue, Camera pCamera, SceneManager pSceneManager)
        {
            string commandType = pValue.Split('_')[0];
            string commandInstruction = pValue.Split('_')[1];

            if(commandType == "CAMERA")
            {
                if (commandInstruction == "FORWARD") { pCamera.MoveForward(0.1f); }

                if (commandInstruction == "RIGHT") { pCamera.RotateY(0.025f); ; }

                if (commandInstruction == "BACK") { pCamera.MoveForward(-0.025f); }

                if (commandInstruction == "LEFT") { pCamera.RotateY(-0.1f); }
            }
            else if(commandType == "SCENE")
            {
                if (commandInstruction == "MAIN") { pSceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU); }

                if (commandInstruction == "OVER") { pSceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER); }

                if (commandInstruction == "GAME") { pSceneManager.ChangeScene(SceneTypes.SCENE_GAME); }
            }
        }

        public void DefaultBinds()
        {
            keyboardBinds.Add(Key.Up, "CAMERA_FORWARD");
            keyboardBinds.Add(Key.Right, "CAMERA_RIGHT");
            keyboardBinds.Add(Key.Down, "CAMERA_BACK");
            keyboardBinds.Add(Key.Left, "CAMERA_LEFT");
            keyboardBinds.Add(Key.M, "SCENE_MAIN");
            keyboardBinds.Add(Key.L, "SCENE_OVER");
        }

        public void ClearBinds()
        {
            keyboardBinds = new Dictionary<Key, string>();
            mouseBinds = new Dictionary<MouseButton, string>();
        }

        public void ReadFromFile(string pFileName)
        {
            StreamReader sr = new StreamReader(pFileName);
            string line = sr.ReadLine();
            string[] lineSplit = new string[2];

            if(line == "KEYBOARD")
            {
                line = sr.ReadLine();
                while (line != "MOUSE" && line != null)
                {
                    lineSplit = line.Split('=');

                    Key key;
                    Enum.TryParse(lineSplit[0], out key);

                    keyboardBinds.Add(key, lineSplit[1]);
                    line = sr.ReadLine();
                }
            }
            if (line == "MOUSE")
            {
                line = sr.ReadLine();
                while (line != null && line != "KEYBOARD")
                {
                    lineSplit = line.Split('=');

                    MouseButton mb;
                    Enum.TryParse(lineSplit[0], out mb);

                    mouseBinds.Add(mb, lineSplit[1]);
                    line = sr.ReadLine();
                }
            }

        }
    }
}
