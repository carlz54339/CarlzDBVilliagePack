global using Il2Cpp;
using CarlzVilliagePack;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using UnityEngine;
using static Il2Cpp.Interop;
using static Il2CppSystem.Array;

[assembly: MelonInfo(typeof(MainMod), "CarlzVilliagePack", "1.0", "Carlz")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace CarlzVilliagePack;
public class MainMod : MelonMod
{
    //public static Sprite[] allSprites = Array.Empty<Sprite>();

    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Mayor>();
        ClassInjector.RegisterTypeInIl2Cpp<Detective>();
        ClassInjector.RegisterTypeInIl2Cpp<Therapist>();
        ClassInjector.RegisterTypeInIl2Cpp<Good_Twin>();
        ClassInjector.RegisterTypeInIl2Cpp<Evil_Twin>();
        ClassInjector.RegisterTypeInIl2Cpp<Phantom>();
        ClassInjector.RegisterTypeInIl2Cpp<Painter>();
        ClassInjector.RegisterTypeInIl2Cpp<Executioner>();
        ClassInjector.RegisterTypeInIl2Cpp<Germaphobe>();
    }
    public override void OnLateInitializeMelon()
    {
        CharacterData detective = new CharacterData();
        detective.role = new Detective();
        detective.name = "Detective";
        detective.description = "Pick three characters, Learn how many of each role is there";
        detective.flavorText = "\"Best detective in the village. Still argues with the Judge\"";
        detective.hints = "Wretch will be seen as an Minion";
        detective.ifLies = "Shows 3 random roles";
        detective.picking = true;
        detective.startingAlignment = EAlignment.Good;
        detective.type = ECharacterType.Villager;
        detective.abilityUsage = EAbilityUsage.Once;
        detective.bluffable = true;
        detective.characterId = "Detective_VP";
        detective.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        detective.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        detective.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        detective.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData mayor = new CharacterData();
        mayor.role = new Mayor();
        mayor.name = "Mayor";
        mayor.description = "Will always start off as good. Will turn evil if they sit next to an evil";
        mayor.flavorText = "\"Loves to take care of the town. Easily tempted with money.\"";
        mayor.hints = "If they turn evil, they will be presented with the Bribed status";
        mayor.ifLies = "";
        mayor.picking = false;
        mayor.startingAlignment = EAlignment.Good;
        mayor.type = ECharacterType.Outcast;
        mayor.abilityUsage = EAbilityUsage.Once;
        mayor.bluffable = false;
        mayor.characterId = "Mayor_VP";
        mayor.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        mayor.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        mayor.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        mayor.color = new Color(0.9659f, 1f, 0.4472f);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", mayor);

        CharacterData therapist = new CharacterData();
        therapist.role = new Therapist();
        therapist.name = "Therapist";
        therapist.description = "Will cure the Drunk if any are present.\nLearn who they cured";
        therapist.flavorText = "\"Actually cures Drunk. Alchemist is confused\"";
        therapist.hints = "The cured will be a villager not in play.\nDemons will be able to double claim those that are sobered.";
        therapist.ifLies = "Will claim a random card to be cured";
        therapist.picking = false;
        therapist.startingAlignment = EAlignment.Good;
        therapist.type = ECharacterType.Villager;
        therapist.abilityUsage = EAbilityUsage.Once;
        therapist.bluffable = false;
        therapist.characterId = "Therapist_VP";
        therapist.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        therapist.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        therapist.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        therapist.color = new Color(1f, 0.935f, 0.7302f);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", therapist);

        CharacterData painter = new CharacterData();
        painter.role = new Painter();
        painter.name = "Painter";
        painter.description = "Shows two pieces of info. One of the info is lying";
        painter.flavorText = "\"Creates great pieces.\nOnly uses it to argue with Poet.\"";
        painter.hints = "";
        painter.ifLies = "Both of my info are false";
        painter.picking = false;
        painter.startingAlignment = EAlignment.Good;
        painter.type = ECharacterType.Villager;
        painter.abilityUsage = EAbilityUsage.Once;
        painter.bluffable = true;
        painter.characterId = "Painter_VP";
        painter.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        painter.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        painter.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        painter.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData germaphobe = new CharacterData();
        germaphobe.role = new Germaphobe();
        germaphobe.name = "Germaphobe";
        germaphobe.description = "They are immune to corruption\nLearn who tried to corrupt them";
        germaphobe.flavorText = "\"Paranoid of any plague. Doesn't trust anyone.\"";
        germaphobe.hints = "Drunk and Puppet will claim that they are clean\n\nAbility will not work for possessed Germaphobes";
        germaphobe.ifLies = "State a random Good character as the corruption source.";
        germaphobe.picking = false;
        germaphobe.startingAlignment = EAlignment.Good;
        germaphobe.type = ECharacterType.Villager;
        germaphobe.abilityUsage = EAbilityUsage.Once;
        germaphobe.bluffable = true;
        germaphobe.characterId = "Germaphobe_VP";
        germaphobe.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        germaphobe.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        germaphobe.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        germaphobe.color = new Color(1f, 0.935f, 0.7302f);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", germaphobe);

        CharacterData good_Twin = new CharacterData();
        good_Twin.role = new Good_Twin();
        good_Twin.name = "Good Twin";
        good_Twin.description = "Will create an evil counterpart, who will always lie\nI always tell the truth\nIf I am executed, lose 8 health";
        good_Twin.flavorText = "\"Tries to lead people away from mischief. Only created more.\"";
        good_Twin.hints = "I use the same disguised villager not in play as my counterpart.\nDemons can double claim my bluff.\nA special statement will occur if for some reason an evil twin wasnt created";
        good_Twin.ifLies = "";
        good_Twin.picking = false;
        good_Twin.startingAlignment = EAlignment.Good;
        good_Twin.type = ECharacterType.Outcast;
        good_Twin.abilityUsage = EAbilityUsage.Once;
        good_Twin.bluffable = false;
        good_Twin.characterId = "GoodTwin_VP";
        good_Twin.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        good_Twin.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        good_Twin.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        good_Twin.color = new Color(0.9659f, 1f, 0.4472f);
        Characters.Instance.startGameActOrder = insertAfterAct("Counsellor", good_Twin);

        CharacterData evil_Twin = new CharacterData();
        evil_Twin.role = new Evil_Twin();
        evil_Twin.name = "Evil Twin";
        evil_Twin.description = "Always plays with a good twin in play\nAlways Lies\nIf i am executed, heal 5 health if the good counterpart is alive";
        evil_Twin.flavorText = "\"Loves to cause mischief. Not very good at lying\"";
        evil_Twin.hints = "I use the same disguised villager not in play as my counterpart\nDemons can double claim my bluff.";
        evil_Twin.ifLies = "";
        evil_Twin.picking = false;
        evil_Twin.startingAlignment = EAlignment.Evil;
        evil_Twin.type = ECharacterType.Outcast;
        evil_Twin.abilityUsage = EAbilityUsage.Once;
        evil_Twin.bluffable = false;
        evil_Twin.characterId = "EvilTwin_VP";
        evil_Twin.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        evil_Twin.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        evil_Twin.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        evil_Twin.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData executioner = new CharacterData();
        executioner.role = new Executioner();
        executioner.name = "Executioner";
        executioner.description = "Targets a random good player. Lose if you execute the target";
        executioner.flavorText = "\"Calls for bombardier to be executed. Chaos ensues!\"";
        executioner.hints = "The executioner will always be seen as truthful\nTheir info will have the target as the evil\nIf no target is present, it will bluff like normal";
        executioner.ifLies = "";
        executioner.picking = false;
        executioner.startingAlignment = EAlignment.Evil;
        executioner.type = ECharacterType.Outcast;
        executioner.abilityUsage = EAbilityUsage.Once;
        executioner.bluffable = false;
        executioner.characterId = "Executioner_VP";
        executioner.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        executioner.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        executioner.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        executioner.color = new Color(0.9659f, 1f, 0.4472f);
        Characters.Instance.startGameActOrder = insertAfterAct("Mayor", executioner);

        CharacterData execBluff = new CharacterData();
        execBluff.role = new ExecBluff();
        execBluff.name = "";
        execBluff.description = "";
        execBluff.flavorText = "\"\"";
        execBluff.hints = "";
        execBluff.ifLies = "";
        execBluff.picking = false;
        execBluff.startingAlignment = EAlignment.Good;
        execBluff.type = ECharacterType.Villager;
        execBluff.abilityUsage = EAbilityUsage.Once;
        execBluff.bluffable = false;
        execBluff.characterId = "ExecBluff_VP";
        execBluff.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        execBluff.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        execBluff.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        execBluff.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData lycaon = new CharacterData();
        lycaon.role = new Lycaon();
        lycaon.name = "Lycaon";
        lycaon.description = "Kill a random Good Character on game start and lose 3 hp\nWhen killed, resurrect the Good Character";
        lycaon.flavorText = "\"Angered Bishop and got punished by divine intervention. He doesn't regret anything.\"";
        lycaon.hints = "Not a hint, but credit's to Bitterbug for the idea!!";
        lycaon.ifLies = "";
        lycaon.picking = false;
        lycaon.startingAlignment = EAlignment.Evil;
        lycaon.type = ECharacterType.Minion;
        lycaon.abilityUsage = EAbilityUsage.Once;
        lycaon.bluffable = false;
        lycaon.characterId = "Lycaon_VP";
        lycaon.artBgColor = new Color(1f, 0f, 0f);
        lycaon.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        lycaon.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        lycaon.color = new Color(0.8491f, 0.4555f, 0f);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", lycaon);

        CharacterData phantom = new CharacterData();
        phantom.role = new Phantom();
        phantom.name = "Phantom";
        phantom.description = "A random outcast and minion are added to the deck view\nOne villager is posessed by the phantom. They cannot use abilities and will lie.";
        phantom.flavorText = "\"Tries to haunt village. Cant figure out how to use abilities correctly.\"";
        phantom.hints = "Phantom will never appear in the village.\nThe posessed will always be a unique villager not in play.";
        phantom.ifLies = "";
        phantom.picking = false;
        phantom.startingAlignment = EAlignment.Evil;
        phantom.type = ECharacterType.Demon;
        phantom.abilityUsage = EAbilityUsage.Once;
        phantom.bluffable = false;
        phantom.characterId = "Phantom_VP";
        phantom.artBgColor = new Color(1f, 0f, 0f);
        phantom.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        phantom.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        phantom.color = new Color(1f, 0.3811f, 0.3811f);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", phantom);

        CustomScriptData phantomScriptData = new CustomScriptData();
        phantomScriptData.name = "Phantom_1";
        ScriptInfo phantomScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> phantomList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        phantomList.Add(phantom);
        phantomScript.mustInclude = phantomList;
        phantomScript.startingDemons = phantomList;
        phantomScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        phantomScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        phantomScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount phantomCounter1 = new CharactersCount(7, 4, 1, 1, 1);
        phantomCounter1.dOuts = phantomCounter1.outs + 1;
        CharactersCount phantomCounter2 = new CharactersCount(8, 5, 1, 1, 1);
        phantomCounter2.dOuts = phantomCounter2.outs + 1;
        CharactersCount phantomCounter3 = new CharactersCount(9, 5, 1, 2, 1);
        phantomCounter3.dOuts = phantomCounter3.outs + 1;
        CharactersCount phantomCounter4 = new CharactersCount(10, 6, 1, 1, 2);
        phantomCounter4.dOuts = phantomCounter4.outs + 1;
        Il2CppSystem.Collections.Generic.List<CharactersCount> phantomCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
        phantomCounterList.Add(phantomCounter1);
        phantomCounterList.Add(phantomCounter2);
        phantomCounterList.Add(phantomCounter3);
        phantomCounterList.Add(phantomCounter4);
        phantomScript.characterCounts = phantomCounterList;
        phantomScriptData.scriptInfo = phantomScript;

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        Il2CppReferenceArray<CharacterData> advancedAscensionDemons = new Il2CppReferenceArray<CharacterData>(advancedAscension.demons.Length + 1);
        advancedAscensionDemons = advancedAscension.demons;
        advancedAscensionDemons[advancedAscensionDemons.Length - 1] = phantom;
        advancedAscension.demons = advancedAscensionDemons;
        Il2CppReferenceArray<CharacterData> advancedAscensionStartingDemons = new Il2CppReferenceArray<CharacterData>(advancedAscension.startingDemons.Length + 1);
        advancedAscensionStartingDemons = advancedAscension.startingDemons;
        advancedAscensionStartingDemons[advancedAscensionStartingDemons.Length - 1] = phantom;
        advancedAscension.startingDemons = advancedAscensionStartingDemons;
        Il2CppReferenceArray<CustomScriptData> advancedAscensionScriptsData = new Il2CppReferenceArray<CustomScriptData>(advancedAscension.possibleScriptsData.Length + 1);
        advancedAscensionScriptsData = advancedAscension.possibleScriptsData;
        advancedAscensionScriptsData[advancedAscensionScriptsData.Length - 1] = phantomScriptData;
        advancedAscension.possibleScriptsData = advancedAscensionScriptsData;
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            addRole(script.startingOutsiders, mayor);
            addRole(script.startingTownsfolks, detective);
            addRole(script.startingTownsfolks, therapist);
            addRole(script.startingOutsiders, good_Twin);
            addRole(script.startingTownsfolks, painter);
            addRole(script.startingOutsiders, executioner);
            addRole(script.startingTownsfolks, germaphobe);
            addRole(script.startingMinions, lycaon);
        }

        ExecutionerStuff.getWinCons();
        // By the vanilla rule of one demon per village max

    }
    public void addRole(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data)
    {
        if (list.Contains(data))
        {
            return;
        }
        list.Add(data);
    }
    public CharacterData[] allDatas = Array.Empty<CharacterData>();
    public override void OnUpdate()
    {
        if (allDatas.Length == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                allDatas = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    allDatas[i] = loadedCharList[i]!.Cast<CharacterData>();
                }
            }
        }
    }
    public CharacterData[] insertAfterAct(string previous, CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++)
        {
            if (inserted)
            {
                newActList[i + 1] = actList[i];
            }
            else
            {
                newActList[i] = actList[i];
                if (actList[i].name == previous)
                {
                    newActList[i + 1] = data;
                    inserted = true;
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
}
