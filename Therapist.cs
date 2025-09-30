using CarlzVilliagePack;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Runtime.InteropServices;
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
public class Therapist : Role
{
    bool drunkInPlay = false;
    Character chars = new Character();
    int id = 0;
    string info = "";
    public override string Description
    {
        get
        {
            return "";
        }
    }

    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("", null);
    }

    public override ActedInfo bcr(Character charRef)
    {
        return new ActedInfo("", null);
    }

    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            drunkInPlay = false;
            chars = new Character();
            info = "";
            id = 0;
            if (!charRef.statuses.statuses.Contains(ECharacterStatus.BrokenAbility))
            {
                foreach (Character c in Gameplay.CurrentCharacters)
                {
                    if (c.dl().characterId == "Drunk_15369527")
                    {
                        drunkInPlay = true;
                        id = c.id;
                        CharacterData disguise = Characters.Instance.gd();
                        Gameplay.Instance.mm(disguise.type, disguise);
                        c.dv(disguise);
                        chars = c;
                        break;
                    }
                }
            }
            return;
        }
        if (trigger != ETriggerPhase.Day) return;
        Il2CppSystem.Collections.Generic.List<Character> chRef = new Il2CppSystem.Collections.Generic.List<Character>();
        chRef.Add(chars);
        info = "";
        if (drunkInPlay == false || (drunkInPlay == true && charRef.statuses.statuses.Contains(ECharacterStatus.BrokenAbility)))
            info += $"I couldn't cure any drunks";
        else
            info += $"#{id} has been sobered";

        onActed?.Invoke(new ActedInfo(info, chRef));
    }

    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            id = 0;
            drunkInPlay = false;
            int count = 0;
            info = "";
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                count++;
            }
            int rng = UnityEngine.Random.Range(0, count) + 1;
            id = rng;

            foreach(Character c in Gameplay.CurrentCharacters)
            {
                if (c.id == rng)
                    chars = c;
            }
            return;
        }
        if (trigger != ETriggerPhase.Day) return;
        Il2CppSystem.Collections.Generic.List<Character> chRef = new Il2CppSystem.Collections.Generic.List<Character>();
        chRef.Add(chars);
        info = "";
        info += $"#{id} has been sobered";

        onActed?.Invoke(new ActedInfo(info, chRef));
    }

    public Therapist() : base(ClassInjector.DerivedConstructorPointer<Therapist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Therapist(System.IntPtr ptr) : base(ptr)
    {

    }
}
