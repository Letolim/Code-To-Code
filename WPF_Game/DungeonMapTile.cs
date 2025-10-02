using System;
using static WPFGame.Map;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace WPFGame
{
    public class DungeonMapTile
    {
        public string Name = "TileName";
        public int ID = 0;
        public Stat MoveCost = new Stat("MoveSpeed", 1, true);
        public string Image = "EmptyTile.png";
        public Event[] EventMove = new Event[] { new Event("None", "None", new string[] { "None", "None" }, new Stat[] { new Stat("None", 0, true) }, true), new Event("None", "None", new string[] { "None", "None" }, new Stat[] { new Stat("None", 0, true) }, true) };
        public Event[] EventOpen = new Event[] { new Event("None", "None", new string[] { "EmptyTileEnterCharacterA", "EmptyTileEnterCharacterA" }, new Stat[] { new Stat("MoveSpeed", 1, true) }, true), new Event("None", "None", new string[] { "EmptyTileEnterCharacterA", "EmptyTileEnterCharacterA" }, new Stat[] { new Stat("MoveSpeed", 1, true) }, true) };
        public string Description = "A Empty Tile";
        public string ToolTip = "Empty Tile";
        public string[] Images = new string[] { "ImageAEmptyTile.png", "ImageBEmptyTile.png" };
        public string[] SpawnConditionsEvent = new string[] { "EmptyTileEnterCharacterA", "EmptyTileEnterCharacterA" };
        public Stat[] SpawnConditionsCharacterStat = new Stat[] { new Stat("None", 0, true) };

        private bool opened = false;

        public int SpawnProppability = 50;
        public int FixedSpawnCount = 50;

        public DungeonMapTile()
        {

        }

        public event MouseButtonEventHandler MouseEnter;


        public bool Opened()
        {
            return opened;
        }

        public void Open()
        {
            opened = true;
        }

        public DungeonMapTile(string name, int iD, Stat moveCost, string image, Event[] eventMove, Event[] eventOpen, string toolTip, string description, string[] images, string[] spawnConditionsEvent, Stat[] spawnConditionsCharacterStat, int spawnProppability, int fixedSpawnCount)
        {
            Name = name;
            ID = iD;
            MoveCost = moveCost;
            Image = image;
            EventMove = eventMove;
            EventOpen = eventOpen;
            Description = description;
            ToolTip = toolTip;
            Images = images;
            SpawnConditionsEvent = spawnConditionsEvent;
            SpawnConditionsCharacterStat = spawnConditionsCharacterStat;
            SpawnProppability = spawnProppability;
            FixedSpawnCount = fixedSpawnCount;
        }
    }
}