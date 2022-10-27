using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using System.IO;

namespace OpenGL_Game.Managers
{
    class InputManager
    {
        bool[] mKeysPressed = new bool[255];

        public InputManager() { }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            mKeysPressed[(char)e.Key] = true;
        }

        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            mKeysPressed[(char)e.Key] = false;
        }

        public void ProcessInputs(SceneManager pSceneManager, Camera pCamera)
        {
            if (mKeysPressed[(char)Key.Up])
            {
                camera.MoveForward(0.1f);
            }
            if (mKeysPressed[(char)Key.Right])
            {
                camera.RotateY(0.01f);
            }
            if (mKeysPressed[(char)Key.Down])
            {
                camera.MoveForward(-0.1f);
            }
            if (mKeysPressed[(char)Key.Left])
            {
                camera.RotateY(-0.01f);
            }
            if (mKeysPressed[(char)Key.M])
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU);
            }
            if (mKeysPressed[(char)Key.L])
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }
        }
    }
}
