﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        EntityManager entityManager;
        SystemManager systemManager;
        GameScriptManager gameScriptManager;
        int mDroneCount;
        int mPlayerHealth;
        public Camera camera;

        bool[] keyPressed = new bool[255];

        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            gameScriptManager = new GameScriptManager();
            sceneManager.collisionManager = new GameCollisionManager(entityManager);
            sceneManager.inputManager.ReadFromFile("Controls/GameControls.txt");

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            //GL.Enable(EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(6, 0.5f, -8f), new Vector3(6, 0.5f, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);

            CreateEntities();
            CreateSystems();

            // TODO: Add your initialization logic here

        }

        private void CreateEntities()
        {
            GameScriptManager gsm = new GameScriptManager();
            gsm.LoadEntityList("Scripts/GameSceneEntities.txt", entityManager);

            gameScriptManager.LoadEntityList("Scripts/GameSceneEntities", entityManager);
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemAudio();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemCollisionSphereSphere(sceneManager.collisionManager);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPlayer(camera);
            systemManager.AddSystem(newSystem);
            newSystem = new SystemCollisionSphereSquare(sceneManager.collisionManager);
            systemManager.AddSystem(newSystem);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            //System.Console.WriteLine("fps=" + (int)(1.0/dt));

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            GetSceneEntityInfo(out mPlayerHealth, out mDroneCount);
            if(mPlayerHealth <= 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }
            else if(mDroneCount <= 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_WIN);
            }

            sceneManager.inputManager.ProcessInputs(sceneManager, camera, entityManager);

            sceneManager.collisionManager.ProcessCollision();

            // TODO: Add your update logic here

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;

            GUI.Label(new Rectangle(25, 25, (int)width, (int)(fontSize * 2f)), "Health: ", 18, StringAlignment.Near, Color.White);;
            for (int i = 0; i < mPlayerHealth; i++) { GUI.CreateImage("Images/heart.png", 100, 100, 100 + (i * 65), 0); }

            GUI.Label(new Rectangle(0, 90, (int)width, (int)(fontSize * 2f)), "Enemies Left: ", 18, StringAlignment.Near, Color.White); ;
            for (int i = 0; i < mDroneCount; i++) { GUI.CreateImage("Images/skull.png", 100, 100, 130 + (i * 65), 60); }

            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            foreach(Entity e in entityManager.Entities())
            {
                e.Close();
            }
            ResourceManager.RemoveAllAssets();
        }

        public void GetSceneEntityInfo(out int playerHealth, out int droneCount)
        {
            playerHealth = 0;
            droneCount = 0;
            foreach(Entity e in entityManager.Entities())
            {
                if(e.Name == "player")
                {
                    IComponent healthComponent = e.Components.Find(delegate (IComponent component)
                    {
                        return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                    });
                    ComponentHealth health = (ComponentHealth)healthComponent;

                    playerHealth = health.Health;
                }
                else if (e.Name.Contains("enemy"))
                {
                    droneCount++;
                }
            }
        }
    }
}
