using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Threading.Tasks;
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
    public System.Collections.Generic.List<string> VillagerWrongInfo = new System.Collections.Generic.List<string>()
    {
        "3 villagers",
        "2 villagers, 1 outcast",
        "1 villager, 2 outcasts",
        "3 outcasts",
    };
    public System.Collections.Generic.List<string> MinionWrongInfo = new System.Collections.Generic.List<string>()
    {
        "3 minions",
        "1 villager, 2 minions",
        "1 outcast, 2 minions",
        "2 villagers, 1 minion",
        "1 villager, 1 outcast, 1 minion",
        "2 outcasts, 1 minion",
    };
    public System.Collections.Generic.List<string> DemonWrongInfo = new System.Collections.Generic.List<string>()
    {
        "2 outcasts, 1 demon",
        "1 outcast, 1 minion, 1 demon",
        "1 villager, 1 outcast, 1 demon",
        "2 villagers, 1 demon",
        "1 villager, 1 minion, 1 demon",
        "2 minions, 1 demon",
    };
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
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(3);
        CharacterPicker.OnCharactersPicked += actionCP;
        CharacterPicker.OnStopPick += actionSP;
    }

    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(3);
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
            if (c.GetCharacterType() == ECharacterType.Villager)
                villagers++;
            else if (c.GetCharacterType() == ECharacterType.Outcast)
                outcasts++;
            else if (c.GetCharacterType() == ECharacterType.Minion)
                minions++;
            else
                demons++;

            ids.Add(c.id);
            chars.Add(c);
        }
        for (int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                if(j != 2)
                {
                    if (ids[j] > ids[j + 1])
                    {
                        int temp = ids[j + 1];
                        ids[j + 1] = ids[j];
                        ids[j] = temp;
                    }
                }
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
        int villagerCount = 0;
        int outcastCount = 0;
        int minionCount = 0;
        int demonCount = 0;
        int rng = 0;

        Il2CppSystem.Collections.Generic.List<int> ids = new Il2CppSystem.Collections.Generic.List<int>();
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in CharacterPicker.PickedCharacters)
        {
            if (c.GetCharacterType() == ECharacterType.Villager)
                villagers++;
            else if (c.GetCharacterType() == ECharacterType.Outcast)
                outcasts++;
            else if (c.GetCharacterType() == ECharacterType.Minion)
                minions++;
            else
                demons++;
            ids.Add(c.id);
            chars.Add(c);
        }
        foreach(Character c in Gameplay.CurrentCharacters)
        {
            if (c.GetCharacterType() == ECharacterType.Villager)
                villagerCount++;
            else if (c.GetCharacterType() == ECharacterType.Outcast)
                outcastCount++;
            else if (c.GetCharacterType() == ECharacterType.Minion)
                minionCount++;
            else
                demonCount++;
        }
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (j != 2)
                {
                    if (ids[j] > ids[j + 1])
                    {
                        int temp = ids[j + 1];
                        ids[j + 1] = ids[j];
                        ids[j] = temp;
                    }
                }
            }
        }
        string info = $"Among #{ids[0]}, #{ids[1]}, #{ids[2]},\n there are: ";
        if(demonCount > 0 && demons >= 1 || demonCount == 0)
        {
            if(minionCount > 0 && minions >= 1 || minionCount == 0)
            {
                if (outcastCount >= 3)
                    rng = UnityEngine.Random.Range(0, VillagerWrongInfo.Count);
                else if (outcastCount == 2)
                    rng = UnityEngine.Random.Range(0, VillagerWrongInfo.Count - 1);
                else if (outcastCount == 1)
                    rng = UnityEngine.Random.Range(0, VillagerWrongInfo.Count - 2);
                else
                    rng = 0;
                info += VillagerWrongInfo[rng];
            }
            else
            {
                if (minionCount > 2 && outcastCount >= 2)
                    rng = UnityEngine.Random.Range(0, MinionWrongInfo.Count);
                else if (minionCount > 2)
                    rng = UnityEngine.Random.Range(0, MinionWrongInfo.Count - 1);
                else if(minionCount == 2 && outcastCount >= 2)
                    rng = UnityEngine.Random.Range(1, MinionWrongInfo.Count);
                else if (minionCount == 2)
                    rng = UnityEngine.Random.Range(1, MinionWrongInfo.Count - 1);
                else if (outcastCount >= 2)
                    rng = UnityEngine.Random.Range(3, MinionWrongInfo.Count);
                else if (outcastCount == 1)
                    rng = UnityEngine.Random.Range(3, MinionWrongInfo.Count - 1);
                else
                    rng = 3;
                info += MinionWrongInfo[rng];
            }
        }
        else
        {
            if (minionCount >= 2 && outcastCount >= 2)
                rng = UnityEngine.Random.Range(0, DemonWrongInfo.Count);
            else if (minionCount >= 2 && outcastCount == 1)
                rng = UnityEngine.Random.Range(1, DemonWrongInfo.Count);
            else if (minionCount == 1 && outcastCount >= 2)
                rng = UnityEngine.Random.Range(0, DemonWrongInfo.Count - 1);
            else if (minionCount == 1 && outcastCount == 1)
                rng = UnityEngine.Random.Range(1, DemonWrongInfo.Count - 1);
            else if (minionCount == 1)
                rng = UnityEngine.Random.Range(4, DemonWrongInfo.Count - 1);
            else if (outcastCount == 1)
                rng = UnityEngine.Random.Range(3, DemonWrongInfo.Count - 2);
            else
                rng = 4;
            info += DemonWrongInfo[rng];
        }
        onActed?.Invoke(new ActedInfo(info, chars));
        Debug.Log($"");
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



