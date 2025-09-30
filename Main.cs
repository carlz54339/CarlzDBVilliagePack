global using Il2Cpp;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppSystem.IO;
using MelonLoader;
using CarlzVilliagePack;
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
        ClassInjector.RegisterTypeInIl2Cpp<Bribed>();
        ClassInjector.RegisterTypeInIl2Cpp<Detective>();
        ClassInjector.RegisterTypeInIl2Cpp<Therapist>();
        ClassInjector.RegisterTypeInIl2Cpp<Good_Twin>();
        ClassInjector.RegisterTypeInIl2Cpp<Evil_Twin>();
    }
    public override void OnLateInitializeMelon()
    {
        CharacterData detective = new CharacterData();
        detective.role = new Detective();
        detective.name = "Detective";
        detective.description = "Pick three characters, Learn how many of each role is there";
        detective.flavorText = "\"Best detective in the villiage. Still can't figure out who framed Wretch.\"";
        detective.hints = "They are they only villager that sees Wretch as an Outcast";
        detective.ifLies = "Shows opposite role of what was picked";
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
        mayor.description = "Will always start off as good. If corrupted, turn evil";
        mayor.flavorText = "\"Loves to take care of the town. Easily tempted with money.\"";
        mayor.hints = "Any corruption will turn them evil.\nThey will always be seen as truthful";
        mayor.ifLies = "";
        mayor.picking = false;
        mayor.startingAlignment = EAlignment.Good;
        mayor.type = ECharacterType.Villager;
        mayor.abilityUsage = EAbilityUsage.Once;
        mayor.bluffable = false;
        mayor.characterId = "Mayor_VP";
        mayor.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        mayor.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        mayor.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        mayor.color = new Color(1f, 0.935f, 0.7302f);
        Characters.Instance.startGameActOrder = insertAfterAct("Alchemist", mayor);

        CharacterData bribed = new CharacterData();
        bribed.role = new Bribed();
        bribed.name = "Mayor";
        bribed.description = "They have been bribed by an outside source.\nUnfortunate";
        bribed.flavorText = "\"Money was just too tempting to this mayor. RIP\"";
        bribed.hints = "Corruption doesnt mean an evil is always next to them";
        bribed.ifLies = "";
        bribed.picking = false;
        bribed.startingAlignment = EAlignment.Evil;
        bribed.type = ECharacterType.Minion;
        bribed.abilityUsage = EAbilityUsage.Once;
        bribed.bluffable = false;
        bribed.characterId = "Bribed_VP";
        bribed.artBgColor = new Color(1f, 0f, 0f);
        bribed.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        bribed.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        bribed.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData therapist = new CharacterData();
        therapist.role = new Therapist();
        therapist.name = "Therapist";
        therapist.description = "Will cure the Drunk if any are present.\nLearn who they cured";
        therapist.flavorText = "\"Actually cures Drunk. Alchemist is confused\"";
        therapist.hints = "";
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

        CharacterData good_Twin = new CharacterData();
        good_Twin.role = new Good_Twin();
        good_Twin.name = "Good Twin";
        good_Twin.description = "Will create an evil counterpart, who will always lie\nI always tell the truth\nIf killed, lose 8 health";
        good_Twin.flavorText = "\"Tries to lead people away from mischief. Only created more.\"";
        good_Twin.hints = "I use the same disguised villager not in play as my counterpart.";
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
        evil_Twin.description = "Always plays with a good twin in play\nAlways Lies\nHeal 5 health if killed and the good counterpart is alive";
        evil_Twin.flavorText = "\"Loves to cause mischief. Not very good at lying\"";
        evil_Twin.hints = "I use the same disguised villager not in play as my counterpart\nAnyone disguised as my disguise is either a Good Twin or an Evil.";
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

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            addRole(script.startingTownsfolks, mayor);
            addRole(script.startingTownsfolks, detective);
            addRole(script.startingTownsfolks, therapist);
            addRole(script.startingOutsiders, good_Twin);
        }
        // By the vanilla rule of one demon per village max
        //CustomScriptData newScriptData = GameObject.Instantiate(advancedAscension.possibleScriptsData[0]);
        //ScriptInfo newScript = newScriptData.scriptInfo;
        //newScript.startingDemons.Clear();
        //newScript.startingDemons.Add(myCustomDemonData);
        //int len = advancedAscension.possibleScriptsData.Length;
        //CustomScriptData[] newPSD = new CustomScriptData[len + 1];
        //for (int i = 0; i < len; i++)
        //{
        //    newPSD[i] = advancedAscension.possibleScriptsData[i];
        //}
        //newPSD[len] = newScriptData;
        //advancedAscension.possibleScriptsData = newPSD;
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