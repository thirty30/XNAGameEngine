﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class Engine : Game
    {
        private GraphicsDeviceManager mGraphics;
        private SpriteBatch mSpriteBatch;
        private static Engine _Instance;
        public static Engine GetSingleton() { return _Instance; }

        public Camera MainCamera;
        public Skybox SkyBox;

        public Engine()
        {
            this.mGraphics = new GraphicsDeviceManager(this);
            this.mGraphics.PreferredBackBufferWidth = 1280;
            this.mGraphics.PreferredBackBufferHeight = 720;
            this.mGraphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RenderSystem.GetSingleton().Initialize();
            GameObjectManager.GetSingleton().Initialize();
            LightSystem.GetSingleton().Initialize();
            //LightSystem.GetSingleton().AmbientColor = Vector3.Zero;

            this.MainCamera = new Camera();
            this.MainCamera.Position = new Vector3(0, 50, 80);
            this.MainCamera.Ratio = this.mGraphics.GraphicsDevice.Viewport.AspectRatio;
            this.MainCamera.Far = 1000.0f;

            //this.SkyBox = new Skybox();
            //this.SkyBox.Init(
            //    this.Content.Load<Model>("Skybox/Cube"),
            //    this.Content.Load<TextureCube>("Skybox/Skybox"),
            //    this.Content.Load<Effect>("Skybox/SkyboxShader")
            //    );

            //GameObject go1 = new GameObject();
            //go1.AddComponent<Light>().Type = LightType.DIRECTION;
            //go1.AddComponent<Light>().LightColor = new Vector3(0.1f, 0.1f, 0.1f);
            //go1.LocalRotation = new Vector3(-45, -45, 0);

            GameObject go2 = new GameObject();
            go2.Position = new Vector3(0, 20, 30);
            Light PointLight1 = go2.AddComponent<Light>();
            PointLight1.Type = LightType.POINT;
            PointLight1.Attenuation = 5000;
            PointLight1.CutOffDistance = 50;
            PointLight1.LightColor = new Vector3(0, 0.5f, 0);
            go2.AddComponent<TestLogic>();

            GameObject go3 = new GameObject();
            go3.Position = new Vector3(0, 50, 0);
            go3.LocalRotation = new Vector3(-90, 0, 0);
            Light SpotLight1 = go3.AddComponent<Light>();
            SpotLight1.Type = LightType.SPOT;
            SpotLight1.Attenuation = 5000;
            SpotLight1.CutOffDistance = 100;
            SpotLight1.ConeAngle = 15;
            SpotLight1.LightColor = new Vector3(1, 0, 0);
            go3.AddComponent<TestLogic>();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            GameObject go = new GameObject();
            ModelRenderObject mr = go.AddComponent<ModelRenderObject>();
            MultiLightMaterial material = new MultiLightMaterial();
            material.Tiling = new Vector2(10, 10);
            material.Shader = this.Content.Load<Effect>("Shaders/MultiLightShader");
            
            //PlaneMaterial material = new PlaneMaterial();
            //material.Shader = this.Content.Load<Effect>("Shaders/SpotLightShader");
            
            material.Tex1 = Engine.GetSingleton().Content.Load<Texture2D>("Checkerboard");
            mr.Init(this.Content.Load<Model>("FlatPlane"), material);
            go.LocalRotation = new Vector3(-90, 0, 0);

            for (int i = 0; i < 1; i++)
            {
                GameObject teapot = new GameObject();
                ModelRenderObject mr2 = teapot.AddComponent<ModelRenderObject>();
                //TeapotMaterial material2 = new TeapotMaterial();
                //material2.Shader = this.Content.Load<Effect>("Shaders/SpotLightShader");
                MultiLightMaterial material2 = new MultiLightMaterial();
                material2.Shader = this.Content.Load<Effect>("Shaders/MultiLightShader");
            
                material2.Tex1 = Engine.GetSingleton().Content.Load<Texture2D>("Metal");
                mr2.Init(this.Content.Load<Model>("Teapot"), material2);
                //teapot.LocalScale = Vector3.One * 0.3f;
            
                if (i == 0)
                    teapot.Position = new Vector3(0, 0, 0);
                else if (i == 1)
                    teapot.Position = new Vector3(-30, -20, 0);
                else
                    teapot.Position = new Vector3(0, -20, 20);
            
                teapot.LocalScale *= 50;
                teapot.LocalRotation = new Vector3(-90, 0, 0);
            }

            //GameObject go = new GameObject();
            //ModelRenderObject mr = go.AddComponent<ModelRenderObject>();
            //FighterMaterial material = new FighterMaterial();
            //material.Shader = this.Content.Load<Effect>("Fighter/FighterShader");
            //material.TexDiffuse = Engine.GetSingleton().Content.Load<Texture2D>("Fighter/FighterDiffuse");
            //material.TexNormal = Engine.GetSingleton().Content.Load<Texture2D>("Fighter/FighterNormal");
            //
            ////FighterBasicMaterial material = new FighterBasicMaterial();
            //
            //mr.Init(this.Content.Load<Model>("Fighter/fighter"), material);
            ////go.LocalRotation = new Vector3(-100, -120, 0);
            //go.LocalScale = Vector3.One * 0.03f;
            //go.AddComponent<TestLogic>();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            this.MainCamera.Update();
            GameObjectManager.GetSingleton().Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            RasterizerState rasterizerstate = new RasterizerState();
            rasterizerstate.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerstate;

            if (this.SkyBox != null)
            {
                GraphicsDevice.DepthStencilState = DepthStencilState.None;
                this.SkyBox.Draw();
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            RenderSystem.GetSingleton().Update();

            base.Draw(gameTime);
        }
    }
}
