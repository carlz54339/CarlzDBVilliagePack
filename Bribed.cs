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
public class Bribed : Minion
{
    Character charReference;
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

    public override CharacterData bcz(Character charRef)
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
        CharacterData character = new CharacterData();
        for (int i = 0; i < allDatas.Length; i++)
        {
            if (allDatas[i].characterId == "Mayor_VP")
            {
                character = allDatas[i];
                break;
            }
        }
        return character;
    }

    public Bribed() : base(ClassInjector.DerivedConstructorPointer<Bribed>())
    {
        MelonLogger.Msg("Entered construct");
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        charReference = this.charRef;
    }

    public Bribed(System.IntPtr ptr) : base(ptr)
    {
        MelonLogger.Msg("Entered construct");
        charReference = this.charRef;
    }
}
