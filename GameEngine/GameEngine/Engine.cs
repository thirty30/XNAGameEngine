using Microsoft.Xna.Framework;
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

        public Engine()
        {
            this.mGraphics = new GraphicsDeviceManager(this);
            this.mGraphics.PreferredBackBufferWidth = 1280;
            this.mGraphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RenderSystem.GetSingleton().Initialize();
            GameObjectManager.GetSingleton().Initialize();
            this.MainCamera = new Camera();
            this.MainCamera.Position = new Vector3(0, 100, 100);
            this.MainCamera.Ratio = this.mGraphics.GraphicsDevice.Viewport.AspectRatio;
            this.MainCamera.Far = 1000.0f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            GameObject go = new GameObject();
            ModelRenderObject mr = go.AddComponent<ModelRenderObject>();
            mr.Init(this.Content.Load<Model>("FlatPlane"), this.Content.Load<Effect>("DiffuseShader"));
            go.AddComponent<TestLogic>();
            go.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(-90.0f));
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //DepthStencilState state = new DepthStencilState();
            //state.DepthBufferEnable = true;
            //state.DepthBufferWriteEnable = true;
            //GraphicsDevice.DepthStencilState = state;

            RenderSystem.GetSingleton().Update();


            base.Draw(gameTime);
        }
    }
}
