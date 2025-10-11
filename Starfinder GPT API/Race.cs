using GameData;
using System.Buffers.Text;
using System.Collections.Generic;

public class Race
{
    public string Name { get; set; }
    public string Description { get; set; }
    public GameData.Race Xeno { get; set; }

    public int BaseHP { get; set; }
    public RaceFeat[] Feats { get; set; }

    public AbilityContainer RaceAbilitys { get; set; }


    public Race(string name, string description, AbilityContainer raceAbilitys, RaceFeat[] feats, int baseHP)
    {
        Name = name;
        Description = description;
        BaseHP = baseHP;
        RaceAbilitys = raceAbilitys;
    }

    public static class Races
    {
        public static readonly Race Android = new Race("Android", "Complex technological creations crafted to resemble humans,\r\nandroids were originally a servitor race, but they have since\r\nbroken free to form their own society. Unlike ordinary robots\r\nor ship AIs, androids do not simply respond according to their\r\nprogramming; rather, they have independent consciousnesses\r\nand are animated by souls—a distinction crucial to\r\ntheir generally accepted status as people rather\r\nthan property.",
        new AbilityContainer(dexterity: 2, intelligence: 2, charisma: -2),new RaceFeat[] {RaceFeat.Constructed,RaceFeat.FlatAffect,RaceFeat.UpgradeSlot }, 4);

        public static readonly Race Human = new Race("Human", "Ambitious, creative, and endlessly curious, humans have\r\nshown more drive to explore their system and the universe\r\nbeyond than any of their neighbor races—for better and for\r\nworse. They’ve helped usher in a new era of system-wide\r\ncommunication and organization and are admired for their\r\npassion and tenacity, but their tendency to shoot\r\nfirst and think about the consequences later can\r\nmake them a liability for those races\r\notherwise inclined to work with them.",
        new AbilityContainer(), new RaceFeat[] { RaceFeat.FreeFeat, RaceFeat.Skilled}, 4);

        public static readonly Race Kasathas = new Race("Kasathas", "Originally from a planet orbiting\r\na dying star far beyond the Pact\r\nWorlds, the four-armed kasathas\r\nmaintain a reputation as a noble\r\nand mysterious people. They are\r\nfamous for their anachronistic\r\nwarriors, ancient wisdom, and\r\nstrange traditions.",
        new AbilityContainer(strength: 2, wisdom: 2, intelligence: -2), new RaceFeat[] { RaceFeat.DesertStride, RaceFeat.FourArmed, RaceFeat.Historian, RaceFeat.NaturalGrace}, 4);

        public static readonly Race Korasha = new Race("Lashunta", "Idealized by many other humanoid\r\nraces and gifted with innate\r\npsychic abilities, lashuntas are at\r\nonce consummate scholars and\r\nenlightened warriors, naturally\r\ndivided into two specialized\r\nsubraces with different abilities\r\nand societal roles.",
        new AbilityContainer(strength: 2, wisdom: -2), new RaceFeat[] { RaceFeat.DimorphicKorasha, RaceFeat.LashuntaMAgic, RaceFeat.Student, RaceFeat.LimitedTelepathyLashunta }, 4);

        public static readonly Race Damaya = new Race("Lashunta", "Idealized by many other humanoid\r\nraces and gifted with innate\r\npsychic abilities, lashuntas are at\r\nonce consummate scholars and\r\nenlightened warriors, naturally\r\ndivided into two specialized\r\nsubraces with different abilities\r\nand societal roles.",
        new AbilityContainer(constitution: -2, intelligence: 2), new RaceFeat[] { RaceFeat.DimorphicDamaya, RaceFeat.LashuntaMAgic, RaceFeat.Student, RaceFeat.LimitedTelepathyLashunta }, 4);

        public static readonly Race Shirren = new Race("Shirren", "Once part of a ravenous hive of\r\nlocust-like predators, the insectile\r\nshirrens only recently broke with\r\ntheir hive mind to become a race\r\nof telepaths physically addicted\r\nto their own individualism,\r\nyet dedicated to the idea of\r\ncommunity and harmony with\r\nother races.",
        new AbilityContainer(constitution: 2, wisdom: 2, charisma: -2), new RaceFeat[] { RaceFeat.Blindsense, RaceFeat.Communalism, RaceFeat.CulturalFascination, RaceFeat.LimitedTelepathyShirren }, 6);

        public static readonly Race Vesk = new Race("Vesk", "Heavily muscled and covered with thick scales and short,\r\nsharp horns, the reptilian vesk are exactly as predatory\r\nand warlike as they appear. Originally hailing from a star\r\nsystem near the Pact Worlds, they sought to conquer and\r\nsubdue their stellar neighbors, as they had all the other\r\nintelligent races in their own system, until\r\nan overwhelming threat forced them\r\ninto a grudging alliance with the Pact\r\nWorlds—for now.",
        new AbilityContainer(strength: 2, constitution: 2, intelligence: -2), new RaceFeat[] { RaceFeat.ArmorSavant, RaceFeat.Fearless, RaceFeat.LowLightVision, RaceFeat.NaturalWeapons }, 6);

        public static readonly Race Ysoki = new Race("Ysoki", "Heavily muscled and covered with thick scales and short,\r\nsharp horns, the reptilian vesk are exactly as predatory\r\nand warlike as they appear. Originally hailing from a star\r\nsystem near the Pact Worlds, they sought to conquer and\r\nsubdue their stellar neighbors, as they had all the other\r\nintelligent races in their own system, until\r\nan overwhelming threat forced them\r\ninto a grudging alliance with the Pact\r\nWorlds—for now.",
        new AbilityContainer(dexterity: 2, intelligence: 2, strength: -2), new RaceFeat[] { RaceFeat.CheekPouches, RaceFeat.Darkvision, RaceFeat.Moxie, RaceFeat.Scrounger }, 2);
    }
}
