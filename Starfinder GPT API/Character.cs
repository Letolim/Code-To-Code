using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameData;
using static Class;

public class Character
{
    public string Name { get; set; }
    public string Race { get; set; }

    public string currentLocation { get; set; }

    public int MaximumHitpoints { get; set; }
    public int CurrentHitpoints { get; set; }

    public int CarryingCappacity { get; set; }
    public int CarryingWeight { get; set; }

    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }

    public string[] Inventory { get; set; }


    //public void HelloWorld()
    //{
    //    string Player = "Name = {Red-X} Race = {Android}";
    //    string PlayerAbilitys = "PlayerAbilitys = {Strength =  " + Strength + ", Dexterity =  " + Dexterity + ", Constitution =  " + Constitution + ", Intelligence =  " + Intelligence + ", Wisdom =  " + Wisdom + ", Charisma =  " + Charisma + "}";
    //    string PlayerBurden = "Burden = {CarryingCappacity = " + CarryingCappacity + ",  CarryingWeight = " + CarryingWeight + " }";
    //    string PlayerHp = "PlayerAbilitys = { CurrentHP =  " + CurrentHitpoints + ", MaximumHitpoints =  " + MaximumHitpoints + "}";
    //    string PlayerInventory = "Example Item {Name:Sword, Weight:5 Kg, Damage: 1d8+2 (Energy), Value 55000 Credits} Inventory Content = {";
    //    for (int i = 0; i < Inventory.Length; i++)
    //    {
    //        PlayerInventory += Inventory[i];
    //        PlayerInventory += ", ";
    //    }
    //    PlayerInventory += "}";
    //}
    //                    $"Evaluate the effect of the improvisation '{improvisation.Name}' on the target. " +
    //                    $"
    //                    $"Is the target a robot? {target.IsRobot}. " +
    //                    $"Is the target undead? {target.IsUndead}. " +
    //                    $"Is the target immune to morale bonuses? {target.IsImmuneToMoraleBonuses}. " +
    //                    $"The improvisation is mind-affecting: {improvisation.IsMindAffecting}. " +
    //                    $"The improvisation grants a morale bonus: {improvisation.GrantsMoraleBonus}. " +
    //                    $"Can the improvisation be applied to the target?";



}

public class CharacterStats
{
    public Race race;
    public Deity Deity { get; set; }
    public string Name { get; set; }
    public float Height { get; set; }
    public int Age { get; set; }
    public int Weight { get; set; }

    public Class Class { get; set; }
    public List<CharacterClassLevel> ClassLevels { get; set; }

    public Theme CharacterTheme { get; set; }


    public Feat[] Feats { get; set; } 
    public Skill Skill {  get; set; }

    public SkillContainer skills;
    public SkillContainer skillModifiers;
    public AbilityContainer abilitieBase;
    public AbilityContainer abilities;
    public AbilityContainer abilitieModifiers;

    public int HP { get; set; }
    public int RP { get; set; }

    public Specialization CharacterSpecialization { get; set; }

    public CharacterStats(string name, string raceDescription, Race characterRace, Specialization characterSpecialization, Theme characterTheme, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, float minHeight, float maxHeight, float height, int age, Skill skill, Feat[] feats) 
    {
        Name = name;
        Height = height;
        Age = age;
        Feats = feats;
        Skill = skill;
        race = Race.Races.Human;

        //if race.skills == null
    }

    public void ApplyItemStats(bool add, Equipment equipment)//Buffs terrain what eva
    {

    }

    public void UpdateAbilitys(int[] strengthMods = null, int[] dexterityMods = null, int[] constitutionMods = null, int[] intelligenceMods = null, int[] wisdomMods = null, int[] charismaMods = null)
    {
        abilities.Strength = abilitieBase.Strength + race.RaceAbilitys.Strength + strengthMods.Sum();
        abilities.Dexterity = abilitieBase.Dexterity + race.RaceAbilitys.Dexterity + dexterityMods.Sum();
        abilities.Constitution = abilitieBase.Constitution + race.RaceAbilitys.Constitution + constitutionMods.Sum();
        abilities.Intelligence = abilitieBase.Intelligence + race.RaceAbilitys.Intelligence + intelligenceMods.Sum();
        abilities.Wisdom = abilitieBase.Wisdom + race.RaceAbilitys.Wisdom + wisdomMods.Sum();
        abilities.Charisma = abilitieBase.Charisma + race.RaceAbilitys.Charisma + charismaMods.Sum();
    }

    public void CalculateAbilityModifiers(int[] strengthModifierMods = null, int[] dexterityModifierMods = null, int[] constitutionModifierMods = null, int[] intelligenceModifierMods = null, int[] wisdomModifierMods = null, int[] charismaModifierMods = null)
    {
        abilitieModifiers.Strength = (abilities.Strength - 10) / 2 + strengthModifierMods?.Sum() ?? 0;
        abilitieModifiers.Dexterity = (abilities.Dexterity - 10) / 2 + dexterityModifierMods?.Sum() ?? 0;
        abilitieModifiers.Constitution = (abilities.Constitution - 10) / 2 + constitutionModifierMods?.Sum() ?? 0;
        abilitieModifiers.Intelligence = (abilities.Intelligence - 10) / 2 + intelligenceModifierMods?.Sum() ?? 0;
        abilitieModifiers.Wisdom = (abilities.Wisdom - 10) / 2 + wisdomModifierMods?.Sum() ?? 0;
        abilitieModifiers.Charisma = (abilities.Charisma - 10) / 2 + charismaModifierMods?.Sum() ?? 0;
    }


    public void CalculateSkillModifiers(Skill skill, int[] Modifier = null)
    {
        skillModifiers.SetProperty(skill.ToString(), CalculateModifier(skills.Acrobatics, skillModifiers.Acrobatics) + Modifier?.Sum() ?? 0);
    }

    public void CalculateSkillModifiers(int[] acrobaticsModifierMods = null, int[] athleticsModifierMods = null, int[] bluffModifierMods = null, int[] computersModifierMods = null, int[] cultureModifierMods = null, int[] diplomacyModifierMods = null, int[] engineeringModifierMods = null, int[] intimidateModifierMods = null, int[] lifeScienceModifierMods = null, int[] medicineModifierMods = null, int[] mysticismModifierMods = null, int[] physicalScienceModifierMods = null, int[] pilotingModifierMods = null, int[] perceptionModifierMods = null, int[] senseMotiveModifierMods = null, int[] sleightOfHandModifierMods = null, int[] stealthModifierMods = null, int[] survivalModifierMods = null, int[] disguiseModifierMods = null, int[] customStrengthModifierMods = null, int[] customConstitutionModifierMods = null, int[] customIntelligenceModifierMods = null, int[] customWisdomModifierMods = null, int[] customCharismaModifierMods = null)
    {


        skillModifiers.Acrobatics = CalculateModifier(skills.Acrobatics, skillModifiers.Acrobatics) + acrobaticsModifierMods?.Sum() ?? 0;
        skillModifiers.Athletics = CalculateModifier(skills.Athletics, skillModifiers.Athletics) + athleticsModifierMods?.Sum() ?? 0;
        skillModifiers.Bluff = CalculateModifier(skills.Bluff, skillModifiers.Bluff) + bluffModifierMods?.Sum() ?? 0;
        skillModifiers.Computers = CalculateModifier(skills.Computers, skillModifiers.Computers) + computersModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Spacecraft = CalculateModifier(skills.Culture.Spacecraft, skillModifiers.Culture.Spacecraft) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Pilots = CalculateModifier(skills.Culture.Pilots, skillModifiers.Culture.Pilots) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.SpaceTravel = CalculateModifier(skills.Culture.SpaceTravel, skillModifiers.Culture.SpaceTravel) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Vehicles = CalculateModifier(skills.Culture.Vehicles, skillModifiers.Culture.Vehicles) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.AlienSpecies = CalculateModifier(skills.Culture.AlienSpecies, skillModifiers.Culture.AlienSpecies) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Races = CalculateModifier(skills.Culture.Races, skillModifiers.Culture.Races) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Arts = CalculateModifier(skills.Culture.Arts, skillModifiers.Culture.Arts) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Entertainment = CalculateModifier(skills.Culture.Entertainment, skillModifiers.Culture.Entertainment) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Etiquette = CalculateModifier(skills.Culture.Etiquette, skillModifiers.Culture.Etiquette) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Customs = CalculateModifier(skills.Culture.Customs, skillModifiers.Culture.Customs) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.History = CalculateModifier(skills.Culture.History, skillModifiers.Culture.History) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.PoliticalSituations = CalculateModifier(skills.Culture.PoliticalSituations, skillModifiers.Culture.PoliticalSituations) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Languages = CalculateModifier(skills.Culture.Languages, skillModifiers.Culture.Languages) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Dialects = CalculateModifier(skills.Culture.Dialects, skillModifiers.Culture.Dialects) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Religion = CalculateModifier(skills.Culture.Religion, skillModifiers.Culture.Religion) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Philosophy = CalculateModifier(skills.Culture.Philosophy, skillModifiers.Culture.Philosophy) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.Geography = CalculateModifier(skills.Culture.Geography, skillModifiers.Culture.Geography) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.EnvironmentalConditions = CalculateModifier(skills.Culture.EnvironmentalConditions, skillModifiers.Culture.EnvironmentalConditions) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.NotableLocations = CalculateModifier(skills.Culture.NotableLocations, skillModifiers.Culture.NotableLocations) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Culture.CulturalLandmarks = CalculateModifier(skills.Culture.CulturalLandmarks, skillModifiers.Culture.CulturalLandmarks) + cultureModifierMods?.Sum() ?? 0;
        skillModifiers.Diplomacy = CalculateModifier(skills.Diplomacy, skillModifiers.Diplomacy) + diplomacyModifierMods?.Sum() ?? 0;
        skillModifiers.Engineering = CalculateModifier(skills.Engineering, skillModifiers.Engineering) + engineeringModifierMods?.Sum() ?? 0;
        skillModifiers.Intimidate = CalculateModifier(skills.Intimidate, skillModifiers.Intimidate) + intimidateModifierMods?.Sum() ?? 0;
        skillModifiers.LifeScience = CalculateModifier(skills.LifeScience, skillModifiers.LifeScience) + lifeScienceModifierMods?.Sum() ?? 0;
        skillModifiers.Medicine = CalculateModifier(skills.Medicine, skillModifiers.Medicine) + medicineModifierMods?.Sum() ?? 0;
        skillModifiers.Mysticism = CalculateModifier(skills.Mysticism, skillModifiers.Mysticism) + mysticismModifierMods?.Sum() ?? 0;
        skillModifiers.PhysicalScience = CalculateModifier(skills.PhysicalScience, skillModifiers.PhysicalScience) + physicalScienceModifierMods?.Sum() ?? 0;
        skillModifiers.Piloting = CalculateModifier(skills.Piloting, skillModifiers.Piloting) + pilotingModifierMods?.Sum() ?? 0;
        skillModifiers.Perception = CalculateModifier(skills.Perception, skillModifiers.Perception) + perceptionModifierMods?.Sum() ?? 0;
        skillModifiers.SenseMotive = CalculateModifier(skills.SenseMotive, skillModifiers.SenseMotive) + senseMotiveModifierMods?.Sum() ?? 0;
        skillModifiers.SleightOfHand = CalculateModifier(skills.SleightOfHand, skillModifiers.SleightOfHand) + sleightOfHandModifierMods?.Sum() ?? 0;
        skillModifiers.Stealth = CalculateModifier(skills.Stealth, skillModifiers.Stealth) + stealthModifierMods?.Sum() ?? 0;
        skillModifiers.Survival = CalculateModifier(skills.Survival, skillModifiers.Survival) + survivalModifierMods?.Sum() ?? 0;
        skillModifiers.Disguise = CalculateModifier(skills.Disguise, skillModifiers.Disguise) + disguiseModifierMods?.Sum() ?? 0;
        skillModifiers.CustomStrength = CalculateModifier(skills.CustomStrength, skillModifiers.CustomStrength) + customStrengthModifierMods?.Sum() ?? 0;
        skillModifiers.CustomConstitution = CalculateModifier(skills.CustomConstitution, skillModifiers.CustomConstitution) + customConstitutionModifierMods?.Sum() ?? 0;
        skillModifiers.CustomIntelligence = CalculateModifier(skills.CustomIntelligence, skillModifiers.CustomIntelligence) + customIntelligenceModifierMods?.Sum() ?? 0;
        skillModifiers.CustomWisdom = CalculateModifier(skills.CustomWisdom, skillModifiers.CustomWisdom) + customWisdomModifierMods?.Sum() ?? 0;
        skillModifiers.CustomCharisma = CalculateModifier(skills.CustomCharisma, skillModifiers.CustomCharisma) + customCharismaModifierMods?.Sum() ?? 0;
    }

    private int CalculateModifier(int skillRank, int abilityMod)
    {
        return skillRank != 0 ? 3 + skillRank + abilityMod: 0;
    }
}