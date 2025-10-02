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
using static WPFGame.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPFGame
{
    public class MapManager
    {

        private List<DungeonMapTile[]> dungeonTiles = new List<DungeonMapTile[]>();

        public MapManager(){}

        public string Init()
        {
            List<Biome> biome = new List<Biome>();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Biome>));
            XmlSerializer xmlSerializerTile = new XmlSerializer(typeof(DungeonMapTile));

            using (TextReader textReader = new StreamReader(Directory.GetCurrentDirectory() + @"\Resources\XML\Biome.XML"))
            {
                biome = xmlSerializer.Deserialize(textReader) as List<Biome>;
            }

            for (int i = 0; i < biome.Count; i++)
            {
                dungeonTiles.Add(new DungeonMapTile[biome[i].TileSheet.Length]);

                for (int n = 0; n < biome[i].TileSheet.Length; n++)
                {
                    using (TextReader textReader = new StreamReader(Directory.GetCurrentDirectory()  + @"\Resources\DungeonMap\" + biome[i].TileSheet[n].TilePath + ".XML"))
                    {
                        dungeonTiles[i][n] = xmlSerializerTile.Deserialize(textReader) as DungeonMapTile;
                    }
                }
            }

            for (int n = 0; n < dungeonTiles.Count; n++)
            {
                for (int i = 1; i < dungeonTiles[n].Length; i++)
                {
                    if (dungeonTiles[n][i].SpawnProppability < dungeonTiles[n][i - 1].SpawnProppability)
                    {
                        int tmp = dungeonTiles[0][i].SpawnProppability;
                        dungeonTiles[n][i].SpawnProppability = dungeonTiles[n][i - 1].SpawnProppability;
                        dungeonTiles[n][i - 1].SpawnProppability = tmp;
                        i = 0;
                    }
                }
            }

            string txt = "";

            for (int i = 0; i < dungeonTiles[0].Length; i++)
                txt += " " + dungeonTiles[0][i].SpawnProppability;

            return "Button Text" + txt;
        }

        private void LoadTile()
        {
            DungeonMapTile mapTile = new DungeonMapTile();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DungeonMapTile));
            using (TextWriter textReader = new StreamWriter(Directory.GetCurrentDirectory() + @"\Resources\Images\dugeonTile.XML"))
            {
                xmlSerializer.Serialize(textReader, mapTile);
            }
        }

        private void LoadBiome()
        {
            List<Biome> biome = new List<Biome>();
            biome.Add(new Biome());
            biome[0].Init();


            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Biome>));
            using (TextWriter textReader = new StreamWriter(Directory.GetCurrentDirectory() + @"\Resources\Images\MapTileBBbbbbbbbbbb.XML"))
            {
                xmlSerializer.Serialize(textReader, biome);
            }
        }
    }
}