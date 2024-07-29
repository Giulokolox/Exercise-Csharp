using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Tankz_2021
{
    class PlayScene : Scene
    {
        //Refs
        protected List<Player> players;
        protected Background bg;
        // Vars
        protected int alivePlayers;
        protected int currentPlayerIndex;
        protected int turnDuration = 20;
        protected TextObject timerTxt;
        // Props
        //public List<Player> Players { get { return players; } }
        public Player CurrentPlayer { get { return players[currentPlayerIndex]; } }
        public float GroundY { get; set; }
        public float MinX { get; set; }
        public float MaxX { get; set; }

        public float PlayerTimer { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            GroundY = 9.5f;

            CameraLimits cameraLimits = new CameraLimits(Game.Window.OrthoWidth * 1.23f, Game.Window.OrthoWidth * 0.5f, Game.Window.OrthoHeight * 0.5f, Game.Window.OrthoHeight * 0.4f);

            MinX = 1;
            MaxX = cameraLimits.MaxX + 7;
            CameraMngr.Init(null,cameraLimits);

            CameraMngr.AddCamera("GUI", new Camera());
            CameraMngr.AddCamera("Bg_0", cameraSpeed: 0.9f);
            CameraMngr.AddCamera("Bg_1", cameraSpeed: 0.95f);

            // Font
            FontMngr.Init();
            Font stdFont = FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            Font comic = FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);
            // Background
            bg = new Background(2);

            //Timer
            timerTxt = new TextObject(new Vector2(Game.Window.OrthoWidth * 0.5f, 3), "", comic);
            //timerTxt.IsActive = true;
            // Players
            players = new List<Player>();

            Player player = new Player(Game.GetController(0), 0);
            player.Position = new Vector2(4, GroundY);

            // Controllers
            Controller controller = Game.GetController(1);

            CameraMngr.SetTarget(player);

            if (controller is KeyboardController)
            {
                List<KeyCode> keys = new List<KeyCode>();
                keys.Add(KeyCode.Up);
                keys.Add(KeyCode.Down);
                keys.Add(KeyCode.Right);
                keys.Add(KeyCode.Left);
                keys.Add(KeyCode.CtrlRight);

                KeysList keyList = new KeysList(keys);
                controller = new KeyboardController(1, keyList);
            }

            Player player2 = new Player(controller, 1);
            player2.Position = new Vector2(6, GroundY);

            players.Add(player);
            players.Add(player2);

            alivePlayers = players.Count;

            CurrentPlayer.Play();

            BulletMngr.Init();

            //PowerUpsMngr.Init();


            base.Start();
        }

        private static void LoadAssets()
        {
            GfxMngr.AddTexture("bg", "Assets/bg_0.png");

            GfxMngr.AddTexture("tracks", "Assets/tanks_tankTracks1.png");
            GfxMngr.AddTexture("body", "Assets/tanks_tankGreen_body1.png");
            GfxMngr.AddTexture("cannon", "Assets/tanks_turret2.png");

            GfxMngr.AddTexture("stdBullet", "Assets/tank_bullet1.png");
            //GfxMngr.AddTexture("rocketBullet", "Assets/tank_bullet3.png");

            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");
        }

        public override void Input()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsAlive)
                {
                    //players[i].Input();
                    players[i].UpdateStateMachine();
                }
            }
        }

        public override void Update()
        {
            if (timerTxt.IsActive)
            {
                PlayerTimer -= Game.DeltaTime;
                timerTxt.Text = ((int)PlayerTimer).ToString();
            }

            PhysicsMngr.Update();
            UpdateMngr.Update();
            //PowerUpsMngr.Update();

            PhysicsMngr.CheckCollisions();

            CameraMngr.Update();
        }

        public override Scene OnExit()
        {
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
        }

        public virtual void OnPlayerDies()
        {
            alivePlayers--;
            if (alivePlayers <= 0)
            {
                IsPlaying = false;
            }
        }

        public virtual void ResetTimer()
        {
            PlayerTimer = turnDuration;
            timerTxt.Text = turnDuration.ToString();
            timerTxt.IsActive = true;
        }

        public virtual void StopTimer()
        {
            timerTxt.IsActive = false;
        }

        public virtual void NextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;

            CameraMngr.SetTarget(CurrentPlayer,false);
            CameraMngr.MoveTo(CurrentPlayer.Position, 1);

            //send current player on Play state
            CurrentPlayer.Play();
        }
    }
}
