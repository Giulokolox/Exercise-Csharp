using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;
using System.Xml;

namespace Heads_2021
{
    class PlayScene : Scene
    {
        //Refs
        protected List<Player> players;
        protected List<Controller> controllers;
        protected List<Enemy> enemies;
        protected GameObject bg;
        //public Enemy Enemy;
        public Map map;
        // Vars
        protected int alivePlayers;
        // Props
        public List<Player> Players { get { return players; } }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            // Managers Initialization
            //FontMngr.Init();
            BulletMngr.Init();
            PowerUpsMngr.Init();

            //// Font(s)
            //Font stdFont = FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 5, 5);
            //Font comic = FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 10, 10);

            // Controllers
            LoadControllers();
            // Map Initialization
            LoadMap();
            // GameObjects Initialization
            LoadObjects();

            base.Start();
        }

        private void LoadControllers()
        {
            Controller controller1 = Game.GetController(0);
            Controller controller2 = Game.GetController(1);

            if (controller2 is KeyboardController)
            {
                List<KeyCode> keys = new List<KeyCode>();
                keys.Add(KeyCode.Up);
                keys.Add(KeyCode.Down);
                keys.Add(KeyCode.Right);
                keys.Add(KeyCode.Left);
                keys.Add(KeyCode.CtrlRight);

                KeysList keyList = new KeysList(keys);
                controller2 = new KeyboardController(1, keyList);
            }

            controllers = new List<Controller>();
            controllers.Add(controller1);
            controllers.Add(controller2);
        }

        private static void LoadAssets()
        {
            GfxMngr.AddTexture("bg", "Assets/hex_grid_green.png");
            GfxMngr.AddTexture("player_1", "Assets/player_1.png");
            GfxMngr.AddTexture("enemy_0", "Assets/enemy_0.png");
            GfxMngr.AddTexture("enemy_1", "Assets/enemy_1.png");
            GfxMngr.AddTexture("bullet", "Assets/fireball.png");
            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");
            GfxMngr.AddTexture("powerUp", "Assets/heart.png");
            GfxMngr.AddTexture("crate", "Assets/Levels/crate.png");
        }

        private void LoadObjects()
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(@".\Assets\Config\GameConfig.xml");
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            XmlNode rootNode = xmlDoc.SelectSingleNode("Objects");
            XmlNode actorsNode = rootNode.SelectSingleNode("Actors");
            
            // Players
            XmlNode playersNode = actorsNode.SelectSingleNode("Players");
            XmlNodeList playerNodes = playersNode.SelectNodes("Player");

            players = new List<Player>();
            
            for (int i = 0; i < playerNodes.Count; i++)
            {
                int x = int.Parse(playerNodes[i].SelectSingleNode("Position").Attributes.GetNamedItem("x").Value);
                int y = int.Parse(playerNodes[i].SelectSingleNode("Position").Attributes.GetNamedItem("y").Value);
                bool isActive = bool.Parse(playerNodes[i].SelectSingleNode("IsActive").Attributes.GetNamedItem("value").Value);

                Player player = new Player(controllers[i], i);
                player.Position = new Vector2(x, y);
                player.IsActive = isActive;

                players.Add(player);
            }

            alivePlayers = players.Count;

            // Enemy
            XmlNode enemiesNode = actorsNode.SelectSingleNode("Enemies");
            XmlNodeList enemyNodes = enemiesNode.SelectNodes("Enemy");

            enemies = new List<Enemy>();

            for (int i = 0; i < enemyNodes.Count; i++)
            {
                int enemyX = int.Parse(enemyNodes[i].SelectSingleNode("Position").Attributes.GetNamedItem("x").Value);
                int enemyY = int.Parse(enemyNodes[i].SelectSingleNode("Position").Attributes.GetNamedItem("y").Value);
                bool enemyIsActive = bool.Parse(enemyNodes[i].SelectSingleNode("IsActive").Attributes.GetNamedItem("value").Value);

                Enemy enemy = new Enemy();
                enemy.Position = new Vector2(enemyX, enemyY);
                enemy.IsActive = enemyIsActive;

                enemies.Add(enemy);
            }
            
            //// Background
            XmlNode bgNode = rootNode.SelectSingleNode("Background");

            int bgX = int.Parse(bgNode.SelectSingleNode("Position").Attributes.GetNamedItem("x").Value);
            int bgY = int.Parse(bgNode.SelectSingleNode("Position").Attributes.GetNamedItem("y").Value);
            bool bgIsActive = bool.Parse(bgNode.SelectSingleNode("IsActive").Attributes.GetNamedItem("value").Value);

            bg = new GameObject("bg", DrawLayer.Background);
            bg.Position = Game.ScreenCenter;
            bg.IsActive = bgIsActive;
        }

        private void LoadMap()
        {
            int[] cells = new int[]
            {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1,
                1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1,
                1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1

            };

            map = new Map(21, 11, cells);
        }

        private void LoadTiledMap()
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(@".\Assets\Config\HeadsMap.tmx");
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            XmlNode mapNode = xmlDoc.SelectSingleNode("map");
            XmlNode layerNode = mapNode.SelectSingleNode("layer");
            XmlNode dataNode = layerNode.SelectSingleNode("data");

            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");

            string[] Ids = csvData.Split(',');
            int[] cells = new int[Ids.Length];

            for (int i = 0; i < Ids.Length; i++)
            {
                if(Ids[i] == "1")
                {
                    cells[i] = int.MaxValue;
                    //cells[i] = 1;
                }
                else
                {
                    cells[i] = 1;
                    //Enemy e = new Enemy();
                    //e.Position = new Vector2(i % 21, i / 21);
                    //e.IsActive = true;
                    //enemies.Add(e);
                }
            }

            map = new Map(21, 11, cells);
        }

        public int GetIntAttribute(XmlNode node, string attrName)
        {
            return int.Parse(GetStringAttribute(node, attrName));
        }

        public bool GetBoolAttribute(XmlNode node, string attrName)
        {
            return bool.Parse(GetStringAttribute(node, attrName));
        }

        public string GetStringAttribute(XmlNode node, string attrName)
        {
            return node.Attributes.GetNamedItem(attrName).Value;
        }

        public override void Input()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].IsAlive)
                {
                    players[i].Input();
                }
            }
        }

        public override void Update()
        {
            PhysicsMngr.Update();
            UpdateMngr.Update();
            PowerUpsMngr.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            //FontMngr.ClearAll();

            DebugMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            DrawMngr.Draw();
            //map.Draw();

            //DebugMngr.Draw();
        }

        public virtual void OnPlayerDies()
        {
            alivePlayers--;
            if(alivePlayers <= 0)
            {
                //IsPlaying = false;
            }
        }
    }
}
