using System;
using GameData;
using System.Collections.Generic;
using System.Linq;

public class Class
{
    public int StaminaPoints { get; set; }
    public int SkillRanksPerLevel { get; set; }
    public Specialization specialization { get; set; }

    public Skill[] ClassSkills { get; set;}
    
    public System.Enum[][] LevelUpChoisesClassFeature { get; set; }
    
    public string CustomSkillName { get; set; }
    public string CustomSkillDescription { get; set; }

    public SkillContainer Skills { get; set; }
    public ClassAbilitiesContainer ClassAbilities { get; set; } 


    public void ModSkillPoint(Skill skill, int value)
    {
        GameData.Attribute foundAttribute = Skills.attributes.Find(a => a.Name == skill.ToString());

        if (foundAttribute != null)
            foundAttribute.Value += value;
        else
            Console.WriteLine("Attribute not found");
    }

    public Class(int staminaPoints, int skillRanksPerLevel, Skill[] classSkills, string customSkillName, string customSkillDescription, ClassAbilitiesContainer classAbilitiesContainer, System.Enum[][] levelUpChoisesClassFeature)
    {
        Skills = new SkillContainer();
        StaminaPoints = staminaPoints;
        SkillRanksPerLevel = skillRanksPerLevel;
        ClassSkills = classSkills;
        ClassAbilities = classAbilitiesContainer;
        LevelUpChoisesClassFeature = levelUpChoisesClassFeature;

        LevelUpChoisesClassFeature = new System.Enum[][] { 
            /*Level  1*/  new System.Enum[] { ClassFeatEnvoy.ExpertiseT1, ClassFeatEnvoy.CleverFeint, ClassFeatEnvoy.DispiritingTaunt, ClassFeatEnvoy.DontQuit, ClassFeatEnvoy.ExpandedAttunement, ClassFeatEnvoy.GetEm, ClassFeatEnvoy.InspiringBoost,   },     
            /*Level  2*/  new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  3*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },   
            /*Level  4*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise }, 
            /*Level  5*/  new System.Enum[] { ClassFeatEnvoy.AlteredBearing },    
            /*Level  6*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise }, 
            /*Level  7*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },    
            /*Level  8*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  9*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },
            /*Level  10*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  11*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },
            /*Level  12*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  13*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },
            /*Level  14*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  15*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },
            /*Level  16*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  17*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },
            /*Level  18*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise },
            /*Level  19*/ new System.Enum[] { ClassFeatEnvoy.AlteredBearing },
            /*Level  20*/ new System.Enum[] { ClassFeatEnvoy.CautiousExpertise }

        };

        bool hasCustomSkill = classSkills.Contains(Skill.CustomStrength) || classSkills.Contains(Skill.CustomConstitution) || classSkills.Contains(Skill.CustomIntelligence) || classSkills.Contains(Skill.CustomWisdom) || classSkills.Contains(Skill.CustomCharisma);

        if (hasCustomSkill)
        {
            CustomSkillName = customSkillName;
            CustomSkillDescription = customSkillDescription;
        }
    }

    public class CharacterClassLevel
    {
        public CharacterClass Class { get; set; }
        public int Level { get; set; }

        public CharacterClassLevel(CharacterClass characterClass, int level)
        {
            Class = characterClass;
            Level = level;
        }

        public void AddClassLevel(CharacterClass characterClass, int level, List<CharacterClassLevel> characterClassLevel)
        {
            var existingClass = characterClassLevel.Find(c => c.Class == characterClass);

            if (existingClass != null)
                existingClass.Level += level;
            else
                characterClassLevel.Add(new CharacterClassLevel(characterClass, level));
        }
    }
}

public static class Classes
{
    public static readonly Class Envoy = new Class(
        6,
        8,
        new Skill[] {
            Skill.Acrobatics, Skill.Athletics, Skill.Bluff, Skill.Computers, Skill.Culture, Skill.Diplomacy,
            Skill.Disguise, Skill.Engineering, Skill.Intimidate, Skill.Medicine, Skill.Perception, Skill.Piloting,
            Skill.SenseMotive, Skill.SleightOfHand, Skill.Stealth, Skill.CustomStrength, Skill.CustomConstitution, Skill.CustomCharisma, Skill.CustomIntelligence, Skill.CustomWisdom
        },
        "CustomSkillName", "Custom skill description.", new ClassAbilitiesContainer(new int[] { 0, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8, 9, 9, 10, 11, 12, 12, 13, 14, 15 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }));

    public static readonly Class Mechanic = new Class(
        6,
        4,
        new Skill[] {
            Skill.Athletics, Skill.Computers, Skill.Engineering, Skill.Medicine, Skill.Perception, Skill.PhysicalScience,
            Skill.Piloting , Skill.CustomStrength, Skill.CustomConstitution, Skill.CustomCharisma, Skill.CustomIntelligence, Skill.CustomWisdom
        },
        "CustomSkillName", "Custom skill description.", new ClassAbilitiesContainer(new int[] { 0, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8, 9, 9, 10, 11, 12, 12, 13, 14, 15 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 }));


    public static readonly Class Mystic = new Class(
        6,
        6,
        new Skill[] {
            Skill.Bluff, Skill.Culture, Skill.Diplomacy, Skill.Disguise, Skill.Intimidate, Skill.LifeScience,
            Skill.Medicine, Skill.Mysticism, Skill.Perception, Skill.SenseMotive, Skill.Survival,
            Skill.CustomStrength, Skill.CustomConstitution, Skill.CustomCharisma, Skill.CustomIntelligence, Skill.CustomWisdom
        },
        "CustomSkillName", "Custom skill description.", new ClassAbilitiesContainer(new int[] { 0, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8, 9, 9, 10, 11, 12, 12, 13, 14, 15 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }));

    public static readonly Class Operative = new Class(
        6,
        6,
        new Skill[] {
            Skill.Acrobatics, Skill.Athletics, Skill.Bluff, Skill.Computers, Skill.Culture, Skill.Disguise,
            Skill.Engineering, Skill.Intimidate, Skill.Medicine, Skill.Perception, Skill.Piloting, Skill.SenseMotive,
            Skill.SleightOfHand, Skill.Stealth, Skill.Survival, Skill.CustomStrength, Skill.CustomConstitution, Skill.CustomCharisma, Skill.CustomIntelligence, Skill.CustomWisdom
        },
        "CustomSkillName", "Custom skill description.", new ClassAbilitiesContainer(new int[] { 0, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8, 9, 9, 10, 11, 12, 12, 13, 14, 15 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }));

    public static readonly Class Soldier = new Class(
        7,
        7,
        new Skill[] {
            Skill.Acrobatics, Skill.Athletics, Skill.Engineering, Skill.Intimidate, Skill.Medicine, Skill.Piloting,
            Skill.Survival,  Skill.CustomStrength, Skill.CustomConstitution, Skill.CustomCharisma, Skill.CustomIntelligence, Skill.CustomWisdom
        },
        "CustomSkillName", "Custom skill description.", new ClassAbilitiesContainer(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 10, 10, 11, 11, 12 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }));
   
    public static readonly Class Technomancer = new Class(
        7,
        4,
        new Skill[] {
            Skill.Computers, Skill.Engineering, Skill.LifeScience, Skill.Mysticism, Skill.PhysicalScience, Skill.Piloting,
            Skill.SleightOfHand,  Skill.CustomStrength, Skill.CustomConstitution, Skill.CustomCharisma, Skill.CustomIntelligence, Skill.CustomWisdom
        },
        "CustomSkillName", "Custom skill description.", new ClassAbilitiesContainer(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 }, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }, new int[] { 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6}, new int[] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12 }));
}
