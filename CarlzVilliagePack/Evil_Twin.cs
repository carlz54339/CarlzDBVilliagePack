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
public class Evil_Twin : Role
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

    public override void ActOnDied(Character charRef)
    {
        Character chars = new Character();
        foreach(Character c in Gameplay.CurrentCharacters)
        {
            if(c.GetRegisterAs().characterId == "GoodTwin_VP")
            {
                chars = c;
                break;
            }
        }
        if(chars.state != ECharacterState.Dead)
        {
            PlayerController.PlayerInfo.health.Damage(-5);
            int health = PlayerController.PlayerInfo.health.value.GetValue();
            if(health > 10)
            {
                PlayerController.PlayerInfo.health.Damage(health - 10);
            }
        }
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        foreach(Character c in Gameplay.CurrentCharacters)
        {
            if(c.GetRegisterAs().characterId == "GoodTwin_VP")
            {
                if (c.bluff != null)
                {
                    return c.bluff;
                }
                else
                {
                    CharacterData bluff = Characters.Instance.GetRandomUniqueVillagerBluff();
                    Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
                    return bluff;
                }
            }
        }
        return null; //used for debugging in case something is missed
    }

    public Evil_Twin() : base(ClassInjector.DerivedConstructorPointer<Evil_Twin>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Evil_Twin(System.IntPtr ptr) : base(ptr)
    {

    }
}
