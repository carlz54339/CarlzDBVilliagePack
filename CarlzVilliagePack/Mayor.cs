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
public class Mayor : Role
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
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> charList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            charList = CharactersHelper.GetSortedListWithCharacterFirst(charList, charRef);

            charList.RemoveAt(0);
            Il2CppSystem.Collections.Generic.List<Character> ajacentEvils = new Il2CppSystem.Collections.Generic.List<Character>();
            if (charList[0].alignment == EAlignment.Evil)
            {
                ajacentEvils.Add(charList[0]);
            }
            if (charList[charList.Count - 1].alignment == EAlignment.Evil)
            {
                ajacentEvils.Add(charList[charList.Count - 1]);
            }

            if (ajacentEvils.Count <= 0) return;
            charRef.alignment = EAlignment.Evil;
            charRef.statuses.AddStatus(BribedStatus.bribed, charRef);
            charRef.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
        }
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return null;
    }

    public Mayor() : base(ClassInjector.DerivedConstructorPointer<Mayor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Mayor(System.IntPtr ptr) : base(ptr)
    {

    }
}

public static class BribedStatus
{
    public static ECharacterStatus bribed = (ECharacterStatus)505;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(bribed))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF00FF><size=18>\nBribed</color></size>";
            }
        }
    }
}
