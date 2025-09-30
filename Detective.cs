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
using System.ComponentModel.Design;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace CarlzVilliagePack;
[RegisterTypeInIl2Cpp]
public class Detective : Role
{
    Character chRef;
    private Il2CppSystem.Action actionCP;
    private Il2CppSystem.Action actionSP;
    private Il2CppSystem.Action actionCPD;
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
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.kx(3);
        CharacterPicker.OnCharactersPicked += actionCP;
        CharacterPicker.OnStopPick += actionSP;
    }

    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.kx(3);
        CharacterPicker.OnCharactersPicked += actionCPD;
        CharacterPicker.OnStopPick += actionSP;
    }

    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= actionCP;
        CharacterPicker.OnStopPick -= actionSP;
        CharacterPicker.OnCharactersPicked -= actionCPD;
    }

    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= actionCP;
        CharacterPicker.OnStopPick -= actionSP;

        int villagers = 0;
        int outcasts = 0;
        int minions = 0;
        int demons = 0;

        Il2CppSystem.Collections.Generic.List<int> ids = new Il2CppSystem.Collections.Generic.List<int>();
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in CharacterPicker.PickedCharacters)
        {
            if (c.dl().type == ECharacterType.Villager)
                villagers++;
            else if (c.dl().type == ECharacterType.Outcast || c.dq().characterId == "Wretch_80988916")
                outcasts++;
            else if (c.dl().type == ECharacterType.Minion)
                minions++;
            else
                demons++;

            ids.Add(c.id);
            chars.Add(c);
        }

        string info = $"Among #{ids[0]}, #{ids[1]}, #{ids[2]}\nthere are: ";

        if (villagers == 0)
            info = info + $"";
        else if (villagers == 1)
            info = info + $"{villagers} villager";
        else
            info = info + $"{villagers} villagers";

        if (outcasts == 0)
            info = info + $"";
        else if(outcasts == 1 && villagers == 0)
            info = info + $"{outcasts} outcast";
        else if (outcasts == 1)
            info = info + $", {outcasts} outcast";
        else if (outcasts == 2 && villagers == 0)
            info = info + $"{outcasts} outcasts";
        else if (outcasts == 2)
            info = info + $", {outcasts} outcasts";
        else
            info = info + $"{outcasts} outcasts";

        if (minions == 0)
            info = info + $"";
        else if(minions == 1 && villagers == 0 && outcasts == 0)
            info = info + $"{minions} minion";
        else if (minions == 1)
            info = info + $", {minions} minion";
        else if (minions == 2 && villagers == 0 && outcasts == 0)
            info = info + $"{minions} minions";
        else if (minions == 2)
            info = info + $", {minions} minions";
        else
            info = info + $"{minions} minions";

        if (demons == 0)
            info = info + $"";
        else if (demons == 1)
            info = info + $", {demons} demon";
        else if (demons == 2)
            info = info + $", {demons} demons";
        else
            info = info + $"{demons} demons";

        onActed?.Invoke(new ActedInfo(info, chars));
        Debug.Log($"{info}");
    }

    private void CharacterPickedDrunk()
    {
        CharacterPicker.OnCharactersPicked -= actionCPD;
        CharacterPicker.OnStopPick -= actionSP;

        int villagers = 0;
        int outcasts = 0;
        int minions = 0;
        int demons = 0;
        int fakeVC = 0;
        int fakeOC = 0;
        int fakeMC = 0;
        int fakeDC = 0;

        int rng = 0;

        Il2CppSystem.Collections.Generic.List<int> ids = new Il2CppSystem.Collections.Generic.List<int>();
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in CharacterPicker.PickedCharacters)
        {
            if (c.dl().type == ECharacterType.Villager)
                demons++;
            else if (c.dl().type == ECharacterType.Outcast || c.dq().characterId == "Wretch_80988916")
                minions++;
            else if (c.dl().type == ECharacterType.Minion)
                villagers++;
            else
                outcasts++;

            rng = UnityEngine.Random.Range(0, 4);

            if (rng == 0)
                fakeVC++;
            else if (rng == 1)
                fakeOC++;
            else if (rng == 2)
                fakeMC++;
            else
                fakeDC++;
            
            ids.Add(c.id);
            chars.Add(c);
        }

        if (villagers == fakeVC && outcasts == fakeOC && minions == fakeMC && demons == fakeDC)
        {
            if(villagers != 0)
            {
                fakeVC--;
                rng = UnityEngine.Random.Range(0, 3);
                if (rng == 0)
                    fakeOC++;
                else if (rng == 1)
                    fakeMC++;
                else
                    fakeDC++; 
            }
            else if(outcasts != 0)
            {
                fakeOC--;
                rng = UnityEngine.Random.Range(0, 3);
                if (rng == 0)
                    fakeVC++;
                else if (rng == 1)
                    fakeMC++;
                else
                    fakeDC++;
            }
            else if(minions != 0)
            {
                fakeMC--;
                rng = UnityEngine.Random.Range(0, 3);
                if (rng == 0)
                    fakeVC++;
                else if (rng == 1)
                    fakeOC++;
                else
                    fakeDC++;
            }
            else
            {
                fakeDC--;
                rng = UnityEngine.Random.Range(0, 3);
                if (rng == 0)
                    fakeVC++;
                else if (rng == 1)
                    fakeOC++;
                else
                    fakeMC++;
            }
        }

        string info = $"Among #{ids[0]}, #{ids[1]}, #{ids[2]}\nthere are: ";

        if (villagers == 0)
            info = info + $"";
        else if (villagers == 1)
            info = info + $"{villagers} villager";
        else
            info = info + $"{villagers} villagers";

        if (outcasts == 0)
            info = info + $"";
        else if (outcasts == 1 && villagers == 0)
            info = info + $"{outcasts} outcast";
        else if (outcasts == 1)
            info = info + $", {outcasts} outcast";
        else if (outcasts == 2 && villagers == 0)
            info = info + $"{outcasts} outcasts";
        else if (outcasts == 2)
            info = info + $", {outcasts} outcasts";
        else
            info = info + $"{outcasts} outcasts";

        if (minions == 0)
            info = info + $"";
        else if (minions == 1 && villagers == 0 && outcasts == 0)
            info = info + $"{minions} minion";
        else if (minions == 1)
            info = info + $", {minions} minion";
        else if (minions == 2 && villagers == 0 && outcasts == 0)
            info = info + $"{minions} minions";
        else if (minions == 2)
            info = info + $", {minions} minions";
        else
            info = info + $"{minions} minions";

        if (demons == 0)
            info = info + $"";
        else if (demons == 1)
            info = info + $", {demons} demon";
        else if (demons == 2)
            info = info + $", {demons} demons";
        else
            info = info + $"{demons} demons";

        onActed?.Invoke(new ActedInfo(info, chars));
        Debug.Log($"{info}");
    }

    public Detective() : base(ClassInjector.DerivedConstructorPointer<Detective>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        actionCP = new System.Action(CharacterPicked);
        actionSP = new System.Action(StopPick);
        actionCPD = new System.Action(CharacterPickedDrunk);
    }

    public Detective(System.IntPtr ptr) : base(ptr)
    {
        actionCP = new System.Action(CharacterPicked);
        actionSP = new System.Action(StopPick);
        actionCPD = new System.Action(CharacterPickedDrunk);
    }
}

