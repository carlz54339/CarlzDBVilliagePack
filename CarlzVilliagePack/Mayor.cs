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
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using CarlzVilliagePack;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace CarlzVilliagePack;
[RegisterTypeInIl2Cpp]
public class Mayor : Role
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
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
        
    }

    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        if(trigger == ETriggerPhase.Start)
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
            if (charRef.statuses.statuses.Contains(ECharacterStatus.Corrupted))
            {
                for (int i = 0; i < allDatas.Length; i++)
                {
                    if (allDatas[i].characterId == "Bribed_VP")
                    {
                        if (charRef.dl().characterId != allDatas[i].characterId)
                        {
                            charRef.dx(allDatas[i]);
                            break;
                        }
                    }
                }
            }
        }
    }

    public Mayor() : base(ClassInjector.DerivedConstructorPointer<Mayor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Mayor(System.IntPtr ptr) : base(ptr)
    {

    }
}