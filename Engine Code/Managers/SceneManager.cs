using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Scenes;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;

namespace OpenGL_Game.Managers
{
    class SceneManager : GameWindow
    {
        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;

        public delegate void KeyboardDelegate(KeyboardKeyEventArgs e);
        public KeyboardDelegate keyboardDownDelegate;
        public KeyboardDelegate keyboardUpDelegate;

        public delegate void MouseDelegate(MouseButtonEventArgs e);
        public MouseDelegate mouseDelegate;

        public GameInputManager inputManager = new GameInputManager();
        public CollisionManager collisionManager;
        public AudioContext audioContext;

        Scene scene;
        SceneTypes sceneType = SceneTypes.SCENE_NONE;
        public static int width = 1200, height = 800;
        public static int windowXPos = 200, windowYPos = 80;

        public int mPlayerHealth; // It is necessary to store this variable here because the game scene restarts on hit
        public bool mAIToggle;

        public SceneManager() : base(width, height, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8, 8, 8, 8), 16))
        {
            mAIToggle = false;
            audioContext = new AudioContext();
            this.X = windowXPos;
            this.Y = windowYPos;
            mPlayerHealth = 3;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape) Exit();
            if (keyboardDownDelegate != null) keyboardDownDelegate.Invoke(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (keyboardUpDelegate != null) keyboardUpDelegate.Invoke(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if(mouseDelegate != null) mouseDelegate.Invoke(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            //Load the GUI
            GUI.SetUpGUI(width, height);

            StartMenu();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }

        public void StartNewGame()
        {
            if(scene != null) scene.Close();
            scene = new GameScene(this, mPlayerHealth);
        }

        public void StartMenu()
        {
            if (scene != null) scene.Close();
            scene = new MainMenuScene(this);
        }

        public void GameOver()
        {
            if (scene != null) scene.Close();
            scene = new GameOverScene(this);
        }

        public void GameWin()
        {
            if (scene != null) scene.Close();
            scene = new GameWinScene(this);
        }

        public static int WindowWidth
        {
            get { return width; }
        }

        public static int WindowHeight
        {
            get { return height; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            SceneManager.width = Width;
            SceneManager.height = Height;

            //Load the GUI
            GUI.SetUpGUI(Width, Height);
        }

        public void ChangeScene(SceneTypes pSceneType)
        {
            inputManager.ClearBinds();
            if (pSceneType == SceneTypes.SCENE_MAIN_MENU)
            {
                StartMenu();
            }
            else if(pSceneType == SceneTypes.SCENE_GAME)
            {
                StartNewGame();
            }
            else if (pSceneType == SceneTypes.SCENE_GAME_OVER)
            {
                GameOver();
            }
            else if (pSceneType == SceneTypes.SCENE_GAME_WIN)
            {
                GameWin();
            }
        }
    }

}

