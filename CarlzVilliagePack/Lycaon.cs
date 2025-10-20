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
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace CarlzVilliagePack;
[RegisterTypeInIl2Cpp]
public class Lycaon : Minion
{
    private Il2CppSystem.Collections.Generic.Dictionary<int, Character> _deadCharacter = new();
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
        WaitTask();
        if (!_deadCharacter.ContainsKey(charRef.id))
            _deadCharacter[charRef.id] = null;
        if(trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> list = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            list = Characters.Instance.FilterCharacterMissingStatus(list, ECharacterStatus.UnkillableByDemon);
            list = Characters.Instance.FilterAlignmentCharacters(list, EAlignment.Good);
            _deadCharacter[charRef.id] = list[UnityEngine.Random.Range(0, list.Count)];
            _deadCharacter[charRef.id].statuses.AddStatus(LycaonText.lycaonKill, charRef);
            _deadCharacter[charRef.id].KillByDemon(charRef);
            PlayerController.PlayerInfo.health.Damage(3);
        }
    }

    private async void WaitTask()
    {
        await Task.Delay(1200);
        return;
    }

    public override void ActOnDied(Character charRef)
    {
        _deadCharacter[charRef.id].InitWithNoReset(_deadCharacter[charRef.id].GetRegisterAs());
        _deadCharacter[charRef.id].statuses.statuses.Remove(LycaonText.lycaonKill);
    }

    public Lycaon() : base(ClassInjector.DerivedConstructorPointer<Lycaon>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Lycaon(System.IntPtr ptr) : base(ptr)
    {

    }
}

public static class LycaonText
{
    public static ECharacterStatus lycaonKill = (ECharacterStatus)909;
    [HarmonyPatch(typeof(Character), nameof(Character.ShowDescription))]
    public static class ChangeKillByDemonText
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.killedByDemon && __instance.statuses.Contains(lycaonKill))
            {
                HintInfo info = new HintInfo();
                info.text = "Killed by a minion\ncan not be revealed";
                UIEvents.OnShowHint.Invoke(info, __instance.hintPivot);
            }
        }
    }
}
