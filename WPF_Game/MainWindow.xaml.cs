using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Threading;
using static WPFGame.Map;
using System.Xml.Serialization;
using System.Windows.Media.Media3D;
using static System.Net.Mime.MediaTypeNames;








namespace WPFGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        long delta = 0;
        long lastUpdateTime = 0;
        float deltaTime = 0;

        private ImageBrush ButtonLitBrush, ButtonUnlitBrush, ButtonClickedBrush, ButtonGeyedOut;

        BitmapImage bitmapImage;

        public MainWindow()
        {
            InitializeComponent();



            Uri ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\ButtonUnlit.png");

            ButtonUnlitBrush = new ImageBrush(new BitmapImage(ImageLocation));

            ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\ButtonLit.png");

            ButtonLitBrush = new ImageBrush(new BitmapImage(ImageLocation));

            ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\ButtonClicked.png");

            ButtonClickedBrush = new ImageBrush(new BitmapImage(ImageLocation));

            ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\ButtonGeyedOut.png");
            bitmapImage = new BitmapImage(ImageLocation);

            ButtonGeyedOut = new ImageBrush(new BitmapImage(ImageLocation));

            gameScreens.Add(new WorldScreen("worldScreen", 0));
            gameScreens.Add(new CampScreen("campScreen", 1));

            InitButtons();

            ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\EmptyTile.png");




            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(1);
            timer.Tick += (sender, e) =>
            {
                Update();
            };
            timer.Start();





            MapManager mapManager = new MapManager();

            this.MapButton.Text = mapManager.Init();

            Map map = new Map(this);

            map.GenerateImageGrid(new BitmapImage(ImageLocation));

        }


        public void AddEventListner(object sender, MouseEventArgs e)
        {
            WorldScreenButton.Text = "WorldButton";
            MouseEnterMenuRectangle(sender, e);
        }



        bool gameLoopThreadIsRunning = false;
        bool gameLoopThreadStopped = false;

        public void GameThread()
        {
            gameLoopThreadIsRunning = true;

            while (gameLoopThreadIsRunning)
            {
                delta = System.DateTime.Now.Ticks - lastUpdateTime;
                lastUpdateTime = System.DateTime.Now.Ticks;

                deltaTime = (float)delta / 10000f;
            }
        }

     

        public void InitButtons()
        {
            Uri buttonUnlitImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\ButtonUnlit.png");
            BitmapImage image = new BitmapImage(buttonUnlitImageLocation);
            ImageBrush brush = new ImageBrush(image);
        }





        private void RecInit(object sender, RoutedEventArgs e)
        {
        }


        bool MouseOverButton = true;
        bool MousePressingButton = false;

        private void MouseEnterMenuRectangle(object sender, MouseEventArgs e)
        {
            MouseOverButton = true;
            if (MousePressingButton)
                return;

            if (sender.GetType() == typeof(TextBlock))
            {
                TextBlock textBlock = (TextBlock)sender;
                textBlock.Background = ButtonLitBrush;
            }

            if (sender.GetType() == typeof(System.Windows.Controls.Image))
            {
                System.Windows.Controls.Image image = (System.Windows.Controls.Image)sender;
                image.Source = bitmapImage;
                
                //WorldScreenButton.Text = "?";
            }

            if (sender.GetType() == typeof(DungeonMapTile))
            {
                DungeonMapTile dungenTile = (DungeonMapTile)sender;

                //if(playerMove > moveCost
                //Move


                if (!dungenTile.Opened())
                {
                    dungenTile.Open();
                   // for (int i = 0; i < dungenTile.EventOpen.Length)
                       // dungenTile.EventOpen[i];
                }

                WorldScreenButton.Text = "done!";
            }
        }

        private void MouseDownMenuRectangle(object sender, MouseButtonEventArgs e)
        {
            MousePressingButton = true;
            TextBlock textBlock = (TextBlock)sender;
            textBlock.Background = ButtonClickedBrush;
            if (textBlock.Name == "WorldScreenButton")
                return;
        }

        private void MouseUpMenuRectangle(object sender, MouseButtonEventArgs e)
        {
            if (!MousePressingButton)
                return;

            TextBlock textBlock = (TextBlock)sender;

            if (MousePressingButton)
                textBlock.Background = ButtonLitBrush;
            else
                textBlock.Background = ButtonUnlitBrush;

            playerInput = textBlock.Name;
            MousePressingButton = false;
        }

        private void MouseLeaveMenuRectangle(object sender, MouseEventArgs e)
        {
            MouseOverButton = false;
            MousePressingButton = false;

            TextBlock textBlock = (TextBlock)sender;
            textBlock.Background = ButtonUnlitBrush;
        }

        List<GameState> gameScreens = new List<GameState>();
        


        private void Init(object sender, EventArgs e)
        {

   
            //GameState gameEvent = (GameState)gameScreens[currentGameScreen];


        }

        float rf, gf, bf;

        //rf += deltaTime * 10f;
        //gf += deltaTime * 10f;
        //bf += deltaTime * 10f;

        //if (rf > 255)
        //    rf = 255;
        //if (gf > 255)
        //    gf = 255;
        //if (bf > 255)
        //    bf = 255;

        //this.Explore2.Background = new SolidColorBrush(Color.FromRgb((byte)rf, (byte)gf, (byte)bf));

        private int currentScreenIndex = -1;
        private string playerInput = "";
        private int currentGameScreen = -1;

        public void Update()
        {
            if (currentGameScreen == -1)
                    currentGameScreen = 1;

                for (int i = 0; i < gameScreens.Count; i++)
                {
                    if (gameScreens[i].ID == currentGameScreen)
                    {

                        currentScreenIndex = i;
                        break;

                    }
                }

            int updateValue = -1;

            updateValue = gameScreens[currentScreenIndex].Update(playerInput, this);

            if (updateValue != -1)
                currentGameScreen = updateValue;


            playerInput = "";
        }

        private abstract class GameState
        {
            public string? Name { get;  set; }
            public int ID { get; set; }

            public GameState(){}

            public abstract void Init();
        
            public abstract int Update(string action, MainWindow main);
            
        }
        private class WorldScreen : GameState
        {
            public WorldScreen(string name, int ID)
            {
                this.Name = name;
                this.ID = ID;
            }

            public override void Init()
            {
                throw new NotImplementedException();
            }

            public override int Update(string action, MainWindow main)
            {
                if (action == "MapButton")
                    return 1;


                return ID;
            }
        }

        private class CampScreen : GameState
        {

            public CampScreen(string name, int ID)
            {
                this.Name = name;
                this.ID = ID;
            }

  
            public override void Init()
            {
                throw new NotImplementedException();
            }

            public override int Update(string action, MainWindow main)
            {
                if (action == "WorldScreenButton")
                    return 0;

                return ID;
            }
        }

        private class CombatScreen : GameState
        {
            public CombatScreen(string name)
            {
                this.Name = name;
            }

            public override void Init()
            {
                throw new NotImplementedException();
            }

            public override int Update(string action, MainWindow main)
            {
                if (action == "MapButton")
                    return 0;


                return -1;
            }
        }


    }
}