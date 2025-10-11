using static UnityEngine.Rendering.HighDefinition.ScalableSettingLevelParameter;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.IO;
using System.Collections;

namespace GameData
{


    public class Attribute
    {
        public string Name {  get; set; }
        public int Value { get; set; }

        public Attribute(string name = "", int value = 0)
        {
            Name = name;
            Value = value;
        }
    }

    public class AbilityContainer
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public AbilityContainer(int strength = 0  , int dexterity = 0, int constitution = 0, int intelligence = 0, int wisdom = 0, int charisma = 0)
        {
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
        }

        public AbilityContainer()
        {
            Strength = 0;
            Dexterity = 0;
            Constitution = 0;
            Intelligence = 0;
            Wisdom = 0;
            Charisma = 0;
        }
    }

    public class ClassAbilitiesContainer
    {
        public int[] BaseAttackBonus { get; set; }
        public int[] Fortitude { get; set; }
        public int[] Reflex { get; set; }
        public int[] Will { get; set; }

        public ClassAbilitiesContainer(int[] baseAttackBonus = null, int[] fortitude = null, int[] reflex = null, int[] will = null)
        {
            BaseAttackBonus = baseAttackBonus;
            Fortitude = fortitude;
            Reflex = reflex;
            Will = will;
        }
    }

    public class SkillContainer
    {
        public List<Attribute> attributes;

        public SkillContainer()
        {
            attributes = new List<Attribute>() {
                new Attribute("DoubleManeuver", 0),
                new Attribute("Spacecraft", 0),
                new Attribute("Pilots", 0),
                new Attribute("SpaceTravel", 0),
                new Attribute("Vehicles", 0),
                new Attribute("AlienSpecies", 0),
                new Attribute("Races", 0),
                new Attribute("Arts", 0),
                new Attribute("Entertainment", 0),
                new Attribute("Etiquette", 0),
                new Attribute("Customs", 0),
                new Attribute("History", 0),
                new Attribute("PoliticalSituations", 0),
                new Attribute("Languages", 0),
                new Attribute("Dialects", 0),
                new Attribute("Religion", 0),
                new Attribute("Philosophy", 0),
                new Attribute("Geography", 0),
                new Attribute("EnvironmentalConditions", 0),
                new Attribute("NotableLocations", 0),
                new Attribute("CulturalLandmarks", 0),
                new Attribute("Acrobatics", 0),
                new Attribute("Athletics", 0),
                new Attribute("Bluff", 0),
                new Attribute("Computers", 0),
                new Attribute("Diplomacy", 0),
                new Attribute("Engineering", 0),
                new Attribute("Intimidate", 0),
                new Attribute("LifeScience", 0),
                new Attribute("Medicine", 0),
                new Attribute("Mysticism", 0),
                new Attribute("PhysicalScience", 0),
                new Attribute("Piloting", 0),
                new Attribute("SenseMotive", 0),
                new Attribute("Perception", 0),
                new Attribute("SleightOfHand", 0),
                new Attribute("Stealth", 0),
                new Attribute("Survival", 0),
                new Attribute("Disguise", 0),
                new Attribute("CustomStrength", 0),
                new Attribute("CustomConstitution", 0),
                new Attribute("CustomIntelligence", 0),
                new Attribute("CustomWisdom", 0),
                new Attribute("CustomCharisma", 0),
                new Attribute("Culture", 0)

            };
        }

        public void ModSkillPoint(string skill, int value)
        {
            Attribute foundAttribute = attributes.Find(a => a.Name == skill);

            if (foundAttribute != null)
                foundAttribute.Value += value;
            else
                Console.WriteLine("Attribute not found");
        }

        public void ModSkillPoint(Skill skill, int value)
        {
            Attribute foundAttribute = attributes.Find(a => a.Name == skill.ToString());

            if (foundAttribute != null)
                foundAttribute.Value += value;
            else
                Console.WriteLine("Attribute not found");
        }

        public void SetSkillPoint(string skill, int value)
        {
            Attribute foundAttribute = attributes.Find(a => a.Name == skill);

            if (foundAttribute != null)
                foundAttribute.Value = value;
            else
                Console.WriteLine("Attribute not found");
        }

        public void SetSkillPoint(Skill skill, int value)
        {
            Attribute foundAttribute = attributes.Find(a => a.Name == skill.ToString());

            if (foundAttribute != null)
                foundAttribute.Value = value;
            else
                Console.WriteLine("Attribute not found");
        }

    }

    public enum Alignment
    {
        LawfulGood,
        NeutralGood,
        ChaoticGood,
        LawfulNeutral,
        TrueNeutral,
        ChaoticNeutral,
        LawfulEvil,
        NeutralEvil,
        ChaoticEvil
    }

    public enum Race
    {
        Human,
        Android,
        Kasatha,
        LashuntaKorasha,
        LashuntaDamaya,
        Shirren,
        Vesk,
        Ysoki,
        Drones,
        Dragons
    }

    public enum CharacterClass
    {
        Envoy,
        Mechanic,
        Mystic,
        Operative,
        Solarian,
        Soldier,
        Technomancer
    }

    public enum Specialization // profession
    {
        None,
        AcePilot,
        BountyHunter,
        Detective,
        Explorer,
        FreeTrader,
        Icon,
        Outlaw,
        Saboteur,
        Spacefarer,
        Xenoseeker,
        Exocortex,
        Drone,
        Akashic,
        Empath,
        Healer,
        Mindbreaker,
        StarShaman,
        Xenodruid,
        Daredevil,
        Ghost,
        Hacker,
        Spy,
        Thief,
        Sharpshooter,
        SolarWeapon,
        SolarArmor,
        Blitz,
        Guard,
        HitAndRun,
        Sharpshoot,
        Bombard,
        ArcaneAssailant,
        Arcanist,
        Spellslinger
    }

    public enum Profession
    {
        Themeless,
        AcePilot,
        BountyHunter,
        Icon,
        Mercenary,
        Outlaw,
        Priest,
        Scholar,
        Spacefarer
    }

    public enum ClassFeat
    {
        ExtensiveStudies,
        SteelyDetermination,
        Grunt,
        SquadLeader,
        Commander,
        Celebrity,
        MegaCelebrity,
    }

    public enum Proficiencys
    {
        
    }

    

    public enum ClassFeatEnvoy
    {
        ExpertiseT1,
        ExpertiseT2,
        ExpertiseT3,
        ExpertiseT4,
        ExpertiseT5,
        ExpertiseT6,
        ExpertiseT7,
        //Improvisations
            CleverFeint,
            DispiritingTaunt,
            DontQuit,
            ExpandedAttunement,
            GetEm,
            InspiringBoost,
            LookAlive,
            NotInTheFace,
            UniversalExpression,
            WatchYourStep,
            //Level4
            CleverAttack,
            DuckUnder,
            Focus,
            Hurry,
            QuickDispiritingTaunt,
            QuickInspiringBoost,
            LongRangeImprovisation,
            WatchOut,
            //Level6
            CleverImprovisations,
            DrawFire,
            HeadsUp,
            ImprovedGetEm,
            //Level8
            DesperateDefense,
            ExpertAttack,
            HiddenAgenda,
            ImprovedHurry,
            SituationalAwareness,
            SustainedDetermination,
        //Expertise Talents
        AdditionalSkillExpertise,
        AlteredBearing,
        Analyst,
        CautiousExpertise,
        ConvincingLiar,
        CulturalSavant,
        CunningDisguise,
        EngineeringAdept,
        ExpertForger,
        FastHack,
        InspiredMedic,
        KeenObserver,
        MenacingGaze,
        RattlingPresence,
        SkilledLinguist,
        SlickCustomer,
        StudentOfTechnology,
        Surgeon,
        WellInformed,
    }


    public enum RaceFeat
    {
        //Android
        Constructed,
        ExceptionalVision,
        FlatAffect,
        UpgradeSlot,
        //Human,
        FreeFeat,
        Skilled,
        //Kasathas
        DesertStride,
        FourArmed,
        Historian,
        NaturalGrace,
        //Lashuntas
        DimorphicDamaya,
        DimorphicKorasha,
        LashuntaMAgic,
        Student,
        LimitedTelepathyLashunta,
        //Shirrens
        Blindsense,
        Communalism,
        CulturalFascination,
        LimitedTelepathyShirren,
        //Vesk
        ArmorSavant,
        Fearless,
        LowLightVision,
        NaturalWeapons,
        //Ysoki
        CheekPouches,
        Darkvision,
        Moxie,
        Scrounger,
    }


    public enum ThemeFeat
    {
        ThemeKnowledgeAcePilot,
        ThemeKnowledgeBountyHunter,
        ThemeKnowledgeIcon,
        ThemeKnowledgeMercenary,
        ThemeKnowledgeOutlaw,
        ThemeKnowledgePriest,
        ThemeKnowledgeScholar,
        ThemeKnowledgeSpacefarer,
        ThemeKnowledgeXenoseeker,
        ThemeKnowledgeThemeless,
        Certainty,
        ExtensiveStudies,
        SteelyDetermination,
        LoneWolf,
        NeedForSpeed,
        MasterPilot,
        SwiftHunter,
        Relentless,
        MasterHunter,
        Celebrity,
        MegaCelebrity,
        MasterIcon,
        Grunt,
        SquadLeader,
        Commander,
        LegalCorruption,
        BlackMarketConnections,
        MasterOutlaw,
        MantleOfTheClergy,
        DivineBoon,
        TrueCommunion,
        TipOfTheTongue,
        ResearchMaven,
        MasterScholar,
        EagerDabbler,
        JackofAllTrades,
        MasterExplorer,
        QuickPidgin,
        FirstContact,
        BrilliantDiscovery,
    }


    public enum Feat
    {
        WeaponFocus,
        ImprovedInitiative,
        Toughness,
        SkillFocus,
        Mobility,
    }

    public enum Skill
    {
        Culture,
        Acrobatics,
        Athletics,
        Bluff,
        Computers,
        Diplomacy,
        Engineering,
        Intimidate,
        LifeScience,
        Medicine,
        Mysticism,
        PhysicalScience,
        Piloting,
        Perception,
        SenseMotive,
        SleightOfHand,
        Stealth,
        Survival,
        Disguise,
        CustomStrength,
        CustomConstitution,
        CustomIntelligence,
        CustomWisdom,
        CustomCharisma,
        Spacecraft,
        Pilots,
        SpaceTravel,
        Vehicles,
        AlienSpecies,
        Races,
        Arts,
        Entertainment,
        Etiquette,
        Customs,
        History,
        PoliticalSituations,
        Languages,
        Dialects,
        Religion,
        Philosophy,
        Geography,
        EnvironmentalConditions,
        NotableLocations,
        CulturalLandmarks,
    }

   
}
