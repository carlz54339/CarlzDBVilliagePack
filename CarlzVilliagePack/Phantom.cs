using CarlzVilliagePack;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Runtime.InteropServices;
using JetBrains.Annotations;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace CarlzVilliagePack;
[RegisterTypeInIl2Cpp]
public class Phantom : Demon
{
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
            //get random minion
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayMi = Gameplay.Instance.GetAscensionAllStartingCharacters();
            notInPlayMi = Characters.Instance.FilterNotInDeckCharactersUnique(notInPlayMi);
            notInPlayMi = Characters.Instance.FilterCharacterType(notInPlayMi, ECharacterType.Minion);

            CharacterData data1 = null;
            if(notInPlayMi.Count == 0)
            {
                notInPlayMi = Gameplay.Instance.GetAllAscensionCharacters();
                notInPlayMi = Characters.Instance.FilterCharacterType(notInPlayMi, ECharacterType.Minion);
            }

            data1 = notInPlayMi[UnityEngine.Random.Range(0, notInPlayMi.Count - 1)];
            Gameplay.Instance.AddScriptCharacter(data1.type, data1);
            
            //get random outcast
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayOu = Gameplay.Instance.GetAscensionAllStartingCharacters();
            notInPlayOu = Characters.Instance.FilterNotInDeckCharactersUnique(notInPlayOu);
            notInPlayOu = Characters.Instance.FilterCharacterType(notInPlayOu, ECharacterType.Outcast);

            CharacterData data2 = null;
            if (notInPlayOu.Count == 0) 
            { 
                notInPlayOu = Gameplay.Instance.GetAllAscensionCharacters();
                notInPlayOu = Characters.Instance.FilterCharacterType(notInPlayMi, ECharacterType.Outcast);
            }

            data2 = notInPlayOu[UnityEngine.Random.Range(0, notInPlayOu.Count - 1)];
            Gameplay.Instance.AddScriptCharacter(data2.type, data2);

            CharacterData getVillager = Characters.Instance.GetRandomUniqueVillagerBluff();
            Gameplay.Instance.AddScriptCharacterIfAble(getVillager.type, getVillager);
            charRef.InitWithNoReset(getVillager);
            charRef.alignment = EAlignment.Evil;
            charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
            charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
            charRef.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            charRef.statuses.AddStatus(PossessedStatus.pos, charRef);
        }
    }

    public Phantom() : base(ClassInjector.DerivedConstructorPointer<Phantom>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Phantom(System.IntPtr ptr) : base(ptr)
    {

    }
}

public static class PossessedStatus
{
    public static ECharacterStatus pos = (ECharacterStatus)205;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(pos))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF7F7F><size=18>\nPossessed</color></size>";
            }
        }
    }
}

public static class scoutInfo
{
    [HarmonyPatch(typeof(Scout), nameof(Scout.GetInfo))]
    public static class si
    {
        public static ActedInfo Postfix(Character __instance)
        {
            string info = "";

            Il2CppSystem.Collections.Generic.List<Character> allEvils = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            allEvils = Characters.Instance.FilterRealAlignmentCharacters(allEvils, EAlignment.Evil);
            allEvils = Characters.Instance.RemoveCharacterType<Recluse>(allEvils);

            Character pickedEvil = allEvils[UnityEngine.Random.Range(0, allEvils.Count)];
            Scout scout = new Scout();
            int closestEvil = scout.GetClosestEvilToEvil(pickedEvil, __instance);

            if (pickedEvil.statuses.statuses.Contains(PossessedStatus.pos))
            {
                pickedEvil.GetRegisterAs().name = "Phantom";
            }

            info = scout.ConjourInfo(pickedEvil.GetRegisterAs().name, closestEvil);
            ActedInfo newInfo = new ActedInfo(info);
            return newInfo;
        }
    }
}