using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Threading;

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
        int mCurrentPlayerHealth;
        public Camera camera;
        int mPlayerX;
        int mPlayerZ;

        public static GameScene gameInstance;

        //Map Variables
        Image mMapImage;
        Image mPowerUpImage;
        Image mEnemyImage;
        Image mPlayerImage;
        Image mHealthImage;
        Image mEnemiesLeftImage;

        public GameScene(SceneManager sceneManager, int pPlayerHealth) : base(sceneManager)
        {
            mPlayerX = 0;
            mPlayerZ = 0;

            gameInstance = this;
            entityManager = new EntityManager();
            gameScriptManager = new GameScriptManager();
            CreateEntities();
            systemManager = new SystemManager();
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
            camera = new Camera(new Vector3(-3, 0.5f, 0f), new Vector3(-3, 0.5f, 1), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            sceneManager.collisionManager = new GameCollisionManager(entityManager, camera);

            CreateSystems();
            CreateImages();

            // Set player health
            mPlayerHealth = pPlayerHealth;
            mCurrentPlayerHealth = mPlayerHealth;
            InitPlayerHealth();
        }

        private void CreateEntities()
        {
            GameScriptManager gsm = new GameScriptManager();
            gsm.LoadEntityList("Scripts/GameSceneEntities.txt", entityManager);

            gameScriptManager.LoadEntityList("Scripts/GameSceneEntities", entityManager);
        }

        private void CreateImages()
        {
            mMapImage = GUI.CreateImage("Images/map.png");
            mEnemiesLeftImage = GUI.CreateImage("Images/skull.png");
            mHealthImage = GUI.CreateImage("Images/heart.png");
            mPowerUpImage = GUI.CreateImage("Images/pumpkin.png");
            mEnemyImage = GUI.CreateImage("Images/enemy.png");
            mPlayerImage = GUI.CreateImage("Images/playerImage.png");
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
            System.Console.WriteLine("fps=" + (int)(1.0/dt));

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            GetSceneEntityInfo(out mPlayerHealth, out mDroneCount);
            if(mPlayerHealth <= 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }
            else if(mPlayerHealth < mCurrentPlayerHealth)
            {
                Thread.Sleep(250);
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME);
            }
            else if(mDroneCount <= 0)
            {
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
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
            for (int i = 0; i < mPlayerHealth; i++) { GUI.ImageDraw(mHealthImage, 100, 100, 100 + (i * 65), -20, 0); }

            GUI.Label(new Rectangle(0, 90, (int)width, (int)(fontSize * 2f)), "Enemies Left: ", 18, StringAlignment.Near, Color.White); ;
            for (int i = 0; i < mDroneCount; i++) { GUI.ImageDraw(mEnemiesLeftImage, 100, 100, 130 + (i * 65), 35, 0); }

            GUI.Label(new Rectangle(0, 120, (int)width, (int)(fontSize * 2f)), "X: " + mPlayerX, 18, StringAlignment.Near, Color.White); ;
            
            GUI.Label(new Rectangle(0, 150, (int)width, (int)(fontSize * 2f)), "Z: " + mPlayerZ, 18, StringAlignment.Near, Color.White); ;

            DrawMap();

            GUI.Render();
        }

        private void DrawMap()
        {
            GUI.ImageDraw(mMapImage, 300, 300, 835, 440, 0);

            foreach(Entity e in entityManager.Entities())
            {
                ComponentPosition pos = (ComponentPosition)GetComponent(ComponentTypes.COMPONENT_POSITION, e);
                Vector3 position = -pos.Position;

                int xOffset;
                int zOffset;

                float xPos = position.X * 14;
                float zPos = position.Z * 14;

                if (e.Name == "player")
                {
                    xOffset = 990;
                    zOffset = 620;
                    Vector3 baseVector = new Vector3(0, 0, -1);
                    Vector3 direction = -camera.cameraDirection;
                    float angle = Vector3.CalculateAngle(baseVector, direction);
                    angle = MathHelper.RadiansToDegrees(angle);

                    if (direction.X < 0)
                    {
                        angle = -angle;
                    }

                    GUI.ImageDraw(mPlayerImage, 32, 32, (int)xPos + xOffset, (int)zPos + zOffset, angle);
                }
                else if (e.Name.Contains("PowerUp"))
                {
                    xOffset = 1000;
                    zOffset = 630;
                    GUI.ImageDraw(mPowerUpImage, 32, 32, (int)xPos + xOffset, (int)zPos + zOffset, 0);
                }
                else if (e.Name.Contains("enemy"))
                {
                    xOffset = 1000;
                    zOffset = 630;
                    GUI.ImageDraw(mEnemyImage, 32, 32, (int)xPos + xOffset, (int)zPos + zOffset, 0);
                }
            }
        }

        private IComponent GetComponent(ComponentTypes pComponentType, Entity pEntity)
        {
            IComponent entityComponent = pEntity.Components.Find(delegate (IComponent component)
            {
                return component.ComponentType == pComponentType;
            });
            IComponent returnComponent = (IComponent)entityComponent;

            return returnComponent;
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
                    ComponentHealth health = (ComponentHealth)GetComponent(ComponentTypes.COMPONENT_HEALTH, e);
                    ComponentPosition pos = (ComponentPosition)GetComponent(ComponentTypes.COMPONENT_POSITION, e);
                    mPlayerX = (int)pos.Position.X;
                    mPlayerZ = (int)pos.Position.Z;

                    playerHealth = health.Health;
                    sceneManager.mPlayerHealth = playerHealth;

                    if(playerHealth >= mCurrentPlayerHealth)
                    {
                        mCurrentPlayerHealth = playerHealth;
                    }
                }
                else if (e.Name.Contains("enemy"))
                {
                    droneCount++;
                }
            }


        }

        private void InitPlayerHealth()
        {
            foreach(Entity e in entityManager.Entities())
            {
                if(e.Name == "player")
                {
                    ComponentHealth temp = (ComponentHealth)GetComponent(ComponentTypes.COMPONENT_HEALTH, e);

                    temp.Health = mPlayerHealth;

                    return;
                }
            }
        }
    }
}
