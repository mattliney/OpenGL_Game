using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    class MainMenuScene : Scene
    {

        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Main Menu";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.inputManager.ReadFromFile("Controls/MainMenuControls.txt");
        }

        public override void Update(FrameEventArgs e)
        {
            sceneManager.inputManager.ProcessInputs(sceneManager, null, null);
        }

        public override void Render(FrameEventArgs e)
        {
            Image menuImage = GUI.CreateImage("Images/menu.png");
            GUI.ImageDraw(menuImage, 1250, 800, -120, -341, 0);
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.CornflowerBlue;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;

            GUI.Render();
        }

        public override void Close()
        {
        }
    }
}