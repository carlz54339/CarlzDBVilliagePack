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
public class Good_Twin : Role
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
        if(trigger == ETriggerPhase.Start)
        {
            charRef.statuses.fm(ECharacterStatus.UnkillableByDemon, charRef);
            Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            Character pickedChar = new Character();
            chars = Characters.Instance.gq(chars, ECharacterType.Villager);
            chars = Characters.Instance.gx(chars, EAlignment.Good);
            pickedChar = chars[UnityEngine.Random.Range(0, chars.Count - 1)];
            foreach(Character c in Gameplay.CurrentCharacters)
            {
                if(c == pickedChar)
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
                    
                    for (int i = 0; i < allDatas.Length; i++)
                    {
                        if (allDatas[i].characterId == "EvilTwin_VP")
                        {
                            if (c.dl().characterId != allDatas[i].characterId)
                            {
                                c.dv(allDatas[i]);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
        if (trigger != ETriggerPhase.Day) return;
    }

    public override void bct(Character charRef)
    {
        if(charRef.killedByDemon == false )
            PlayerController.PlayerInfo.health.jl(3);
    }

    public override CharacterData bcz(Character charRef)
    {
        charRef.statuses.fm(ECharacterStatus.HealthyBluff, charRef);
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.dl().characterId == "EvilTwin_VP")
            {
                if (c.bluff != null)
                    return c.bluff;
                else
                {
                    CharacterData bluff = Characters.Instance.gd();
                    Gameplay.Instance.mm(bluff.type, bluff);
                    return bluff;
                }
            }
        }
        return null; //used in case something is missed
    }

    public Good_Twin() : base(ClassInjector.DerivedConstructorPointer<Good_Twin>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Good_Twin(System.IntPtr ptr) : base(ptr)
    {

    }
}
