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
using WPFGame;
using System.Xml.Serialization;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml;

namespace WPFGame
{
    public class Map
    {
        MainWindow mainWindow;
        int screenWidth = 1920;
        int screenHeight = 1080;

        //public string[] triggeredEvents;

        public Map(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.bitmapImages = new List<BitmapImage>();

            // xmlWriter.

            XmlSerializer serializer = new XmlSerializer(typeof(BiomeContent[]));

            //XmlWriter xmlWriter = XmlWriter.Create(Directory.GetCurrentDirectory() + @"\Resources\Images\");

            FileStream stream = new FileStream(Directory.GetCurrentDirectory() + @"\Resources\Images\MapTileBEBEV.XML", FileMode.Create, FileAccess.ReadWrite);



            List<DungeonMapTile> tile = new List<DungeonMapTile> { new DungeonMapTile(), new DungeonMapTile() };
            List<Biome> biome = new List<Biome> { new Biome(), new Biome() };


            serializer.Serialize(stream, new BiomeContent[] { new BiomeContent("", 0, 0), new BiomeContent("", 0, 0) });


            //serializer.Serialize(stream, biome);

            stream.Dispose();
            // xmlWriter.WriteStartDocument();
        }



        private List<BitmapImage> bitmapImages = new();

        public void LoadImages()
        {

            Uri ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\Images\ButtonUnlit.png");

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Resources\Images\TileMapImages");

        }


        public class Biome
        {
            public string Name = "!";
            public int ID = 0;
            public string Image = "!";
            public Event EventLoad = new Event("None", "None", new string[] { "None", "None" }, new Stat[] { new Stat("None", 0, true) }, true);
            public string Description = "!";
            public string ToolTip = "!";
            public int BaseTileID = 0;
            public BiomeContent[]? TileSheet;

            public Biome()
            {
                //TileSheet = new BiomeContent[] { new BiomeContent("", 0, 0)new BiomeContent[] { new BiomeContent("", 0, 0) };
            }

            public Biome(string name, int iD, string image, Event eventLoad, string toolTip, string description, int baseTileID, BiomeContent[] tileSheet)
            {
                Name = name;
                ID = iD;
                Image = image;
                EventLoad = eventLoad;
                BaseTileID = baseTileID;
                TileSheet = tileSheet;//ID prop count
                Description = description;
                ToolTip = toolTip;
            }


            public void Init()
            {
                Name = "name";
                ID = 10;
                Image = "image";
                EventLoad = new Event("None", "None", new string[] { "None", "None" }, new Stat[] { new Stat("None", 0, true) }, true);
                BaseTileID = 13;
                TileSheet = new BiomeContent[] { new BiomeContent("ForestTile", 30, 40),  new BiomeContent("Glade", 10, 20) };
                Description = "description";
                ToolTip = "toolTip";
            }
        }

        public class BiomeContent
        {
            public string TilePath = "";
            public int SpawnProppability = 50;
            public int FixedSpawnCount = 50;

            public BiomeContent() { }

            public BiomeContent(string tilePath, int spawnProppability, int fixedSpawnCount)
            {
                TilePath = tilePath;
                SpawnProppability = spawnProppability;
                FixedSpawnCount = fixedSpawnCount;
            }
        }

        public class Event
        {
            public string Name = "!";
            public string Action = "!";
            public string[] SpawnConditionsEvent = new string[] { "EmptyTileEnterCharacterA", "EmptyTileEnterCharacterA" };
            public Stat[] SpawnConditionsCharacterStat = new Stat[] { new Stat("MoveSpeed", 1, true) };
            public bool TriggerOnce = false;

            public Event() { }

            public Event(string name, string action, string[] spawnConditionsEvent, Stat[] spawnConditionsCharacterStat, bool triggerOnce = false)
            {
                Name = name;
                Action = action;
                SpawnConditionsEvent = spawnConditionsEvent;
                SpawnConditionsCharacterStat = spawnConditionsCharacterStat;
                TriggerOnce = triggerOnce;
            }
        }

        public class Stat
        {
            public string Name = "!";
            public int Value = 0;
            public bool CharacterStat = false;

            public Stat() { }

            public Stat(string name, int value, bool characterStat)
            {
                Name = name;
                Value = value;
                CharacterStat = characterStat;
            }
        }

      
        public void GenerateImageGrid(DungeonMapTile[] dungeonTiles)
        {
            int width = 10;
            int height = 10;
            int imageWidht = 32;
            int imageHeight = 32;

            int halfScreenWidth = screenWidth / 2;
            int halfScreenHeight = screenHeight / 2;

            int maxTileCount = width * height;

            BitmapImage[] images = new BitmapImage[dungeonTiles.Length];


            Uri ImageLocation;

            for (int i = 0; i < images.Length; i++)
            {

                ImageLocation = new Uri(Directory.GetCurrentDirectory() + @"\Resources\DungeonMap\" + dungeonTiles[i].Image);


                images[i] = new BitmapImage(ImageLocation);
            }



            List<int> tilesToSpawn = new List<int>();


            for (int i = 0; i < dungeonTiles.Length; i++)
            {
                for(int n  = 0; n < dungeonTiles[i].FixedSpawnCount; n ++)
                    tilesToSpawn.Add(i);
            }

            int fixedTileCount = tilesToSpawn.Count;



            System.Random randomGenerator = new System.Random();

            for (int i = 0; i < width * height - fixedTileCount; i++)
            {
                int rnd = randomGenerator.Next(0,100);

                for (int n = 0; n < dungeonTiles.Length; n++)
                    if (rnd > dungeonTiles[i].SpawnProppability)
                    { 
                        tilesToSpawn.Add(n);
                        break;
                    }    
            }

            //if(tilesToSpawn  > maxTileCount....



            int count = 0;


            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {


                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = images[count];
                    image.Width = images[count].Width;
                    image.Height = images[count].Width;
                    image.Name = dungeonTiles[tilesToSpawn[count]].Name;

                    image.MouseEnter += (sender, e) =>
                    {
                        mainWindow.AddEventListner(sender, e);
                    };

                    dungeonTiles[tilesToSpawn[count]].MouseEnter += (sender, e) =>
                    {
                        mainWindow.AddEventListner(sender, e);
                    };

                    TranslateTransform transform = new TranslateTransform();
                    transform.X = x * imageWidht + halfScreenWidth - (imageWidht * width) / 2;
                    transform.Y = y * imageHeight + halfScreenHeight - (imageHeight * height) / 2;

                    mainWindow.Canvas.Children.Add(image);

                    mainWindow.Canvas.Children[mainWindow.Canvas.Children.Count - 1].RenderTransform = transform;

                    mainWindow.Canvas.Children[mainWindow.Canvas.Children.Count - 1].RenderTransform.Value.Translate(x * halfScreenWidth + imageWidht / 2 - halfScreenWidth / 2, y * halfScreenHeight + imageWidht / 2 - halfScreenHeight / 2);

                    count++;

                }
        }

        public void GenerateImageGrid(BitmapImage imageSource, int[,] layout)
        {
            int width = 10;
            int height = 10;
            int imageWidht = 32;
            int imageHeight = 32;

            int halfScreenWidth = screenWidth / 2;
            int halfScreenHeight = screenHeight / 2;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Width = imageWidht;
                    image.Height = imageHeight;
                    image.Source = imageSource;

                    image.MouseEnter += (sender, e) =>
                    {
                        mainWindow.AddEventListner(sender, e);
                    };

                    TranslateTransform transform = new TranslateTransform();
                    transform.X = x * imageWidht + halfScreenWidth - (imageWidht * width) / 2;
                    transform.Y = y * imageHeight + halfScreenHeight - (imageHeight * height) / 2;

                    mainWindow.Canvas.Children.Add(image);

                    mainWindow.Canvas.Children[mainWindow.Canvas.Children.Count - 1].RenderTransform = transform;

                    mainWindow.Canvas.Children[mainWindow.Canvas.Children.Count - 1].RenderTransform.Value.Translate(x * halfScreenWidth + imageWidht / 2 - halfScreenWidth / 2, y * halfScreenHeight + imageWidht / 2 - halfScreenHeight / 2);
                }
        }

        public void GenerateImageGrid(BitmapImage imageSource)
        {
            int width = 10;
            int height = 10;
            int imageWidht = 32;
            int imageHeight = 32;

            int halfScreenWidth = screenWidth / 2;
            int halfScreenHeight = screenHeight / 2;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {


                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Width = imageWidht;
                    image.Height = imageHeight;
                    image.Source = imageSource;

                    image.MouseEnter += (sender, e) =>
                    {
                        mainWindow.AddEventListner(sender, e);
                    };

                    TranslateTransform transform = new TranslateTransform();
                    transform.X = x * imageWidht + halfScreenWidth - (imageWidht * width) / 2;
                    transform.Y = y * imageHeight + halfScreenHeight - (imageHeight * height) / 2;

                    mainWindow.Canvas.Children.Add(image);

                    mainWindow.Canvas.Children[mainWindow.Canvas.Children.Count - 1].RenderTransform = transform;

                    mainWindow.Canvas.Children[mainWindow.Canvas.Children.Count - 1].RenderTransform.Value.Translate(x * halfScreenWidth + imageWidht / 2 - halfScreenWidth / 2, y * halfScreenHeight + imageWidht / 2 - halfScreenHeight / 2);
                }
        }
    }
}