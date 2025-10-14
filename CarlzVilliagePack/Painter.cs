using CarlzVilliagePack;
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
public class Painter : Role
{
    public System.Collections.Generic.List<Role> infoRoles = new System.Collections.Generic.List<Role>()
    {
        new Empath(),
        new Scout(),
        new Investigator(),
        new BountyHunter(),
        new Lookout(),
        new Knitter(),
        new Tracker(),
        new Shugenja(),
        new Noble(),
        new Bishop(),
        new Archivist(),
        new Acrobat2(),
    };
    public override string Description
    {
        get
        {
            return "";
        }
    }

    public override ActedInfo GetInfo(Character charRef)
    {
        System.Collections.Generic.List<Role> roles = new System.Collections.Generic.List<Role>(infoRoles);
        string info = "";
        int rng = UnityEngine.Random.Range(0, 2);
        bool bluffed = false;
        Role role = roles[UnityEngine.Random.Range(0, roles.Count)];
        roles.Remove(role);
        ActedInfo newInfo = new ActedInfo("");
        if(rng == 0)
        {
            newInfo = role.GetInfo(charRef);
        }
        else
        {
            newInfo = role.GetBluffInfo(charRef);
            bluffed = true;
        }
        info += newInfo.desc.ToString();
        info += "\n\n";
        role = roles[UnityEngine.Random.Range(0, roles.Count)];
        if(bluffed == true)
        {
            newInfo = role.GetInfo(charRef);
        }
        else
        {
            newInfo = role.GetBluffInfo(charRef);
        }
        info += newInfo.desc.ToString();
        newInfo = new ActedInfo(info);

        return newInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        System.Collections.Generic.List<Role> roles = new System.Collections.Generic.List<Role>(infoRoles);
        string info = "";
        Role role = roles[UnityEngine.Random.Range(0, roles.Count)];
        roles.Remove(role);
        ActedInfo newInfo = role.GetBluffInfo(charRef);
        info += newInfo.desc.ToString();
        info += "\n\n";
        role = roles[UnityEngine.Random.Range(0, roles.Count)];
        newInfo = role.GetBluffInfo(charRef);
        info += newInfo.desc.ToString();
        newInfo = new ActedInfo(info);

        return newInfo; 
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetBluffInfo(charRef));
    }

    public Painter() : base(ClassInjector.DerivedConstructorPointer<Painter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Painter(System.IntPtr ptr) : base(ptr)
    {

    }
}
