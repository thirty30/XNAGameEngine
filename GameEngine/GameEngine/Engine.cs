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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
            RenderSystem.GetSingleton().Update();


            base.Draw(gameTime);
        }
    }
}
