using CarlzVilliagePack;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace CarlzVilliagePack;
[RegisterTypeInIl2Cpp]
public class Executioner : Role
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "";
        }
    }

    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if(trigger == ETriggerPhase.Start)
        {
            charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            charRef.statuses.AddStatus(ECharacterStatus.UnkillableByDemon, charRef);
            Il2CppSystem.Collections.Generic.List<Character> charList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            charList = Characters.Instance.FilterAlignmentCharacters(charList, EAlignment.Good);
            Character wretch = new Character();
            bool foundWretch = false;
            foreach(Character c in charList)
            {
                if (c.dataRef.characterId == "Wretch_80988916")
                {
                    wretch = c;
                    foundWretch = true;
                    break;
                }
            }
            if(foundWretch)
                charList.Remove(wretch);
            Character pickedCharToTarget = new Character();
            pickedCharToTarget = charList[UnityEngine.Random.Range(0, charList.Count)];
            pickedCharToTarget.statuses.AddStatus(TargetStatus.target, charRef);
            pickedCharToTarget.statuses.AddStatus(ECharacterStatus.UnkillableByDemon, charRef);
            pickedCharToTarget.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            if (pickedCharToTarget.GetCharacterData().name == "Knight")
                pickedCharToTarget.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        }
    }

    public override CharacterData GetBluffIfAble(Character charRef)
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
        CharacterData bluff = new CharacterData();
        Il2CppSystem.Collections.Generic.List<CharacterData> possibleBluffs = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        for(int i = 0; i < allDatas.Length; i++)
        {
            if (allDatas[i].characterId == "ExecBluff_VP")
                bluff = allDatas[i];
            else if (allDatas[i].characterId == "Bishop_58855542")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Empress_13782227")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Gossip_85354100")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Oracle_07039445")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Hunter_93427887")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Enlightened_62576217")
                possibleBluffs.Add(allDatas[i]);
        }
        CharacterData chosenBluff = new CharacterData();
        chosenBluff = possibleBluffs[UnityEngine.Random.Range(0, possibleBluffs.Count)];
        bluff.name = chosenBluff.name;
        bluff.description = chosenBluff.description;
        bluff.flavorText = chosenBluff.flavorText;
        bluff.hints = chosenBluff.hints;
        bluff.ifLies = chosenBluff.ifLies;
        bluff.art_cute = chosenBluff.art_cute;
        bluff.backgroundArt = chosenBluff.backgroundArt;
        return bluff;
    }

    public Executioner() : base(ClassInjector.DerivedConstructorPointer<Executioner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Executioner(System.IntPtr ptr) : base(ptr)
    {

    }
}

public static class TargetStatus
{
    public static ECharacterStatus target = (ECharacterStatus)707;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(target))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=orange><size=18>\nTargeted</color></size>";
            }
        }
    }
}
