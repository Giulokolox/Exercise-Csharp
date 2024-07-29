using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bulletz_2022
{
    class PlayScene : Scene
    {
        protected Background bg;
        public Player Player { get; protected set; }

        public float GroundY { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            GroundY = 9;

            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 0.8f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, 0);
            CameraMngr.Init(null, cameraLimits);

            CameraMngr.AddCamera("GUI", new Aiv.Fast2D.Camera());
            CameraMngr.AddCamera("Sky", cameraSpeed: 0.02f);
            CameraMngr.AddCamera("Bg_0", cameraSpeed: 0.15f);
            CameraMngr.AddCamera("Bg_1", cameraSpeed: 0.2f);
            CameraMngr.AddCamera("Bg_2", cameraSpeed: 0.9f);

            FontMngr.Init();
            Font stdFont = FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            Font comic = FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);

            bg = new Background(4);
            

            Player = new Player(Game.GetController(0), 0);
            Player.Position = new Vector2(8,9);
            CameraMngr.Target = Player;

            
            BulletMngr.Init();
            //SpawnMngr.Init();
            //PowerUpsMngr.Init();

            base.Start();
        }

        private static void LoadAssets()
        {
            GfxMngr.AddTexture("player", "Assets/player.png");
            GfxMngr.AddTexture("playerBullet", "Assets/greenGlobe.png");

            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");

            GfxMngr.AddTexture("bg", "Assets/bg_sky.jpg");
        }

        public override void Input()
        {
            Player.Input();
        }

        public override void Update()
        {
            if (!Player.IsAlive)
                IsPlaying = false;

            PhysicsMngr.Update();
            UpdateMngr.Update();
            //SpawnMngr.Update();
            //PowerUpsMngr.Update();
            //Bg.Update();

            PhysicsMngr.CheckCollisions();
            CameraMngr.Update();
        }

        public override Scene OnExit()
        {
            Player = null;

            //BulletMngr.ClearAll();
            //SpawnMngr.ClearAll();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();

            DebugMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();

            //DebugMngr.Draw();
        }
    }
}
