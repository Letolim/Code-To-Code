using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
using System;

public interface IFeat 
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Enum[] RequirementsSkill { get; set; }
    public Enum[] RequirementsFeat { get; set; }
    public Enum[] RequirementsProficiencey { get; set; }
    public int RequiredLevel { get; set; }

    public class WeaponFocus : AbilityGrantingFeat
    {

    }

    public static IFeat GetFeat<T>(T feat) where T : System.Enum
    {
        if (typeof(T) == typeof(ClassFeat))
            return GetClassFeat((ClassFeat)(object)feat);

        if (typeof(T) == typeof(RaceFeat))
            return GetRaceFeat((RaceFeat)(object)feat);

        return null;
    }

    public static IFeat GetFeat(Enum type, string name)
    {
        if (type.GetType() == typeof(ClassFeat))
            return GetClassFeat(name);

        if (type.GetType() == typeof(RaceFeat))
            return GetClassFeat(name);

        return null;
    }

    public static IFeat[] GetFeat(Enum type, int level)
    {
        if (type.GetType() == typeof(ClassFeat))
            return GetClassFeat(level);

        if (type.GetType() == typeof(RaceFeat))
            return GetClassFeat(level);

        return null;
    }

    private static IFeat[] GetClassFeat(int level)
    {
        //GameData.Attribute foundAttribute = Skills.attributes.Find(a => a.Name == skill.ToString());

        return level switch
        {
            1 => new IFeat[]{},
            2 => new IFeat[]{},
            _ => null,
        };
    }

    private static IFeat GetClassFeat(string classFeat)
    {
        return classFeat switch
        {
            "WeaponFocus" => new WeaponFocus(),
            "WeaponFocuss" => new WeaponFocus(),
            _ => null,
        };
    }

    private static IFeat GetClassFeat(ClassFeat classFeat)
    {
        return classFeat switch
        {
            ClassFeat.Celebrity => new WeaponFocus(),
            ClassFeat.MegaCelebrity => new WeaponFocus(),
            _ => null,
        };
    }

    private static IFeat GetRaceFeat(RaceFeat raceFeat)
    {
        return raceFeat switch
        {
            RaceFeat.Blindsense => new WeaponFocus(),
            RaceFeat.CheekPouches => new WeaponFocus(),
            _ => null,
        };
    }
}

public class AbilityGrantingFeat : IFeat
{
    public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Enum RequirementsSkill { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Enum RequirementsFeat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Enum RequirementsProficiencey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int RequiredLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public class SkillGrantingFeat : IFeat
{
    public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Enum RequirementsSkill { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Enum RequirementsFeat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Enum RequirementsProficiencey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int RequiredLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

