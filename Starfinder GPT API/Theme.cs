using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public abstract class Theme
{
    public string Name {  get; set; }
    public string Description { get; set; }
    public AbilityContainer Abilites { get; set; }
    public ThemeFeat[] Feats { get; set; }

    public Theme()
    {
        Feats = new ThemeFeat[4];
    }

    public Theme(string name, string description, AbilityContainer abilites)
    {
        Name = name;
        Description = description;
        Abilites = abilites;
    }

    public abstract void LevelUp(int level);
}

public class ThemelessTheme : Theme
{
    public ThemelessTheme(string name, string description, AbilityContainer abilities)
    {
        Name = name;
        Description = description;
        Abilites = abilities;
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeThemeless;
                break;
            case 6:
                Feats[1] = ThemeFeat.Certainty;
                break;
            case 12:
                Feats[2] = ThemeFeat.ExtensiveStudies;
                break;
            case 18:
                Feats[3] = ThemeFeat.SteelyDetermination;
                break;
        }
    }
}

public class AcePilotTheme : Theme
{
    public AcePilotTheme() 
    {
        Name = "Ace Pilot";
        Description = "You are most comfortable at the controls of a vehicle, whether it’s a\r\nstarship racing through the inky void of space or a ground vehicle zooming\r\nbetween trees, around boulders, and across dusty badlands. You might be\r\na member of an elite military force, the recipient of intense courses of\r\ntraining. Alternatively, you might be a total amateur with innate skills that\r\nmake you a much-admired hotshot.";
        Abilites = new AbilityContainer(dexterity: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1: 
                Feats[0] = ThemeFeat.ThemeKnowledgeAcePilot;
                break;
            case 6:
                Feats[1] = ThemeFeat.LoneWolf;
                break;
            case 12:
                Feats[2] = ThemeFeat.NeedForSpeed;
                break;
            case 18:
                Feats[3] = ThemeFeat.MasterPilot;
                break;
        }
    }
}

public class BountyHunterTheme : Theme
{
    public BountyHunterTheme()
    {
        Name = "Bounty Hunter";
        Description = "You track people down for money. It is a dangerous profession, as most of\r\nyour targets understandably don’t wish to be caught. You wouldn’t have\r\nit any other way. You might have a code of ethics, never taking jobs that,\r\nsay, target children or members of your own race. You might hunt down only\r\nescaped criminals. Or you might be completely amoral, taking any job that\r\ncomes along—for the right price.";
        Abilites = new AbilityContainer(constitution: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeBountyHunter;
                break;
            case 6:
                Feats[1] = ThemeFeat.SwiftHunter;
                break;
            case 12:
                Feats[2] = ThemeFeat.Relentless;
                break;
            case 18:
                Feats[3] = ThemeFeat.MasterHunter;
                break;
        }
    }
}

public class IconTheme : Theme
{
    public IconTheme()
    {
        Name = "Icon";
        Description = "Thanks to interstellar transmissions and Drift travel, the galaxy is smaller\r\nthan ever, and this connectivity has facilitated your ascension to celebrity\r\nstatus. You might be a famous performer or a celebrated scientist, but\r\neither way, you get recognized on the Pact Worlds and in associated\r\nsystems. Your reason for traveling to unknown worlds might be to further\r\nspread your acclaim or to escape the limelight.";
        Abilites = new AbilityContainer(charisma: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeIcon;
                break;
            case 6:
                Feats[1] = ThemeFeat.Celebrity;
                break;
            case 12:
                Feats[2] = ThemeFeat.MegaCelebrity;
                break;
            case 18:
                Feats[3] = ThemeFeat.MasterIcon;
                break;
        }
    }
}

public class MercenaryTheme : Theme
{
    public MercenaryTheme()
    {
        Name = "Mercenary";
        Description = "Whether you take jobs that match your ethical beliefs or you fight for\r\nanyone who can afford your services, you are a hired gun. You might take\r\npride in your past accomplishments, proudly displaying trophies of your kills,\r\nor you might be laden with guilt over being the sole survivor of a mission\r\ngone terribly wrong. You most likely work with other mercenaries and are\r\nfamiliar with the methodologies of military actions all across the galaxy.";
        Abilites = new AbilityContainer(strength: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeMercenary;
                break;
            case 6:
                Feats[1] = ThemeFeat.Grunt;
                break;
            case 12:
                Feats[2] = ThemeFeat.SquadLeader;
                break;
            case 18:
                Feats[3] = ThemeFeat.Commander;
                break;
        }
    }
}

public class OutlawTheme : Theme
{
    public OutlawTheme()
    {
        Name = "Mercenary";
        Description = "Due to the sins of your past or your current unlawful behavior, you are a\r\nwanted individual somewhere in the Pact Worlds. You might not even be\r\nguilty and are striving to clear your good name. Or you might fully admit to\r\nbeing a criminal but believe the laws you break are unjust. Whatever the\r\ncase, boarding a starship headed to the Vast might be just the thing you\r\nneed until the heat dies down—or until you’re dragged off to prison.";
        Abilites = new AbilityContainer(dexterity: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeOutlaw;
                break;
            case 6:
                Feats[1] = ThemeFeat.LegalCorruption;
                break;
            case 12:
                Feats[2] = ThemeFeat.BlackMarketConnections;
                break;
            case 18:
                Feats[3] = ThemeFeat.MasterOutlaw;
                break;
        }
    }
}

public class PriestTheme : Theme
{
    public PriestTheme()
    {
        Name = "Priest";
        Description = "You are a member of an organized religion or similar association. Your belief,\r\nwhether it has been a part of you since childhood or it came to you later\r\nin life, is an integral part of your character. You might travel the stars\r\nproselytizing your deity, or your church might have sent you out on a\r\nspecific holy (or unholy) mission. No matter what obstacles life puts in your\r\nway, you always have the conviction of your beliefs to fall back on.";
        Abilites = new AbilityContainer(intelligence: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgePriest;
                break;
            case 6:
                Feats[1] = ThemeFeat.MantleOfTheClergy;
                break;
            case 12:
                Feats[2] = ThemeFeat.DivineBoon;
                break;
            case 18:
                Feats[3] = ThemeFeat.TrueCommunion;
                break;
        }
    }
}

public class ScholarTheme : Theme
{
    public ScholarTheme()
    {
        Name = "Scholar";
        Description = "You are an erudite intellectual, pitting your brain against problems and\r\npuzzles that others would find daunting. You might be an instructor of\r\na specific topic at a large university or a dabbler in a number of fields of\r\nstudy. You could be exploring the galaxy in search of ancient artifacts or\r\nnew scientific phenomena. Whatever your motivation, you are sure that\r\nthe answers you seek are out there.";
        Abilites = new AbilityContainer(intelligence: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeScholar;
                break;
            case 6:
                Feats[1] = ThemeFeat.TipOfTheTongue;
                break;
            case 12:
                Feats[2] = ThemeFeat.ResearchMaven;
                break;
            case 18:
                Feats[3] = ThemeFeat.MasterScholar;
                break;
        }
    }
}

public class SpacefarerTheme : Theme
{
    public SpacefarerTheme()
    {
        Name = "Spacefarer";
        Description = "Your longing to journey among the stars can’t be sated. You yearn for the\r\nadventure of stepping onto a distant world and exploring its secrets. You\r\ntend to greet every new opportunity with bravery and fortitude, confident\r\nthat your multitude of skills will pull you through. Perhaps you simply find\r\njoy in the act of traveling with your companions, or perhaps you are just out\r\nto line your pockets with all sorts of alien loot!";
        Abilites = new AbilityContainer(constitution: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeSpacefarer;
                break;
            case 6:
                Feats[1] = ThemeFeat.EagerDabbler;
                break;
            case 12:
                Feats[2] = ThemeFeat.JackofAllTrades;
                break;
            case 18:
                Feats[3] = ThemeFeat.MasterExplorer;
                break;
        }
    }
}

public class XenoseekerTheme : Theme
{
    public XenoseekerTheme()
    {
        Name = "Spacefarer";
        Description = "The thought of meeting alien life-forms excites you. The more different\r\ntheir appearances and customs are from yours, the better! You either\r\nbelieve they have much to teach you or you want to prove you are better\r\nthan them. Of course, the only way to accomplish your goal is to leave\r\nthe Pact Worlds and travel to the Vast, where a virtually endless number\r\nof aliens await.";
        Abilites = new AbilityContainer(charisma: 1);
        LevelUp(1);
    }

    public override void LevelUp(int level)
    {
        switch (level)
        {
            case 1:
                Feats[0] = ThemeFeat.ThemeKnowledgeXenoseeker;
                break;
            case 6:
                Feats[1] = ThemeFeat.QuickPidgin;
                break;
            case 12:
                Feats[2] = ThemeFeat.FirstContact;
                break;
            case 18:
                Feats[3] = ThemeFeat.BrilliantDiscovery;
                break;
        }
    }
}