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
public class Germaphobe : Role
{
    private Il2CppSystem.Collections.Generic.Dictionary<int, Character> source = new();
    public override string Description
    {
        get
        {
            return "";
        }
    }

    public override ActedInfo GetInfo(Character charRef)
    {
        string info = $"";
        if(charRef.GetRegisterAs().name == "Puppet")
            info += $"I am clean";
        else if (source[charRef.id] == null)
            info += $"I am clean";
        else
            info += $"#{source[charRef.id].id} is dirty";

        ActedInfo newInfo = new ActedInfo(info);
        return newInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        string info = $"";
        if (charRef.statuses.statuses.Contains(ECharacterStatus.Corrupted))
            info += $"I am clean";
        else
        {
            Il2CppSystem.Collections.Generic.List<Character> charList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            charList = CharactersHelper.GetSortedListWithCharacterFirst(charList, charRef);
            charList.RemoveAt(0);
            if (Calculator.RollDice(10) <= 5)
            {
                Character pz = new Character();
                Il2CppSystem.Collections.Generic.List<Character> goodChars = new Il2CppSystem.Collections.Generic.List<Character>();
                goodChars = charList;
                goodChars = Characters.Instance.FilterAlignmentCharacters(goodChars, EAlignment.Good);
                pz = goodChars[UnityEngine.Random.Range(0, goodChars.Count - 1)];
                info += $"#{pz.id} is dirty";
            }
            else
            {
                Il2CppSystem.Collections.Generic.List<Character> adjacentChars = new Il2CppSystem.Collections.Generic.List<Character>();
                if (charList[0].GetAlignment() == EAlignment.Good)
                    adjacentChars.Add(charList[0]);
                if (charList[charList.Count - 1].GetAlignment() == EAlignment.Good)
                    adjacentChars.Add(charList[charList.Count - 1]);
                if(charList.Count == 0)
                {
                    Character pz = new Character();
                    Il2CppSystem.Collections.Generic.List<Character> goodChars = new Il2CppSystem.Collections.Generic.List<Character>();
                    goodChars = charList;
                    goodChars = Characters.Instance.FilterAlignmentCharacters(goodChars, EAlignment.Good);
                    pz = goodChars[UnityEngine.Random.Range(0, goodChars.Count - 1)];
                    info += $"#{pz.id} is dirty";
                }
                else
                {
                    Character chosenChar = new Character();
                    chosenChar = adjacentChars[UnityEngine.Random.Range(0, adjacentChars.Count)];
                    info += $"#{chosenChar.id} is dirty";
                }
            }
        }

        ActedInfo newInfo = new ActedInfo(info);
        return newInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (!source.ContainsKey(charRef.id))
            source[charRef.id] = null;
        if(trigger == ETriggerPhase.Start)
        {
            source[charRef.id] = null;
        }
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetInfo(charRef));
    }

    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (!source.ContainsKey(charRef.id))
            source[charRef.id] = null;
        if(trigger == ETriggerPhase.Start)
        {
            source[charRef.id] = null;
            bool poisoned = false;
            bool plagued = false;
            bool convinced = false;
            bool controlled = false;
            bool pookaFound = false;
            Il2CppSystem.Collections.Generic.List<Character> charList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            charList = CharactersHelper.GetSortedListWithCharacterFirst(charList, charRef);
            charList.RemoveAt(0);

            if (charRef.statuses.statuses.Contains(ECharacterStatus.Corrupted) && charRef.GetAlignment() == EAlignment.Good && charRef.GetRegisterAs().name != "Drunk")
            {
                Il2CppSystem.Collections.Generic.List<Character> adjacentChars = new Il2CppSystem.Collections.Generic.List<Character>();
                adjacentChars.Add(charList[0]);
                adjacentChars.Add(charList[charList.Count - 1]);

                if (adjacentChars[0].GetAlignment() == EAlignment.Evil)
                {
                    if (adjacentChars[0].GetRegisterAs().name == "Pooka")
                    {
                        source[charRef.id] = adjacentChars[0];
                        charRef.statuses.statuses.Remove(ECharacterStatus.Corrupted);
                        convinced = true;
                    }
                    else if (adjacentChars[0].GetRegisterAs().name == "Poisoner")
                    {
                        if (!convinced)
                        {
                            poisoned = true;
                            Il2CppSystem.Collections.Generic.List<Character> poisonList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                            poisonList = CharactersHelper.GetSortedListWithCharacterFirst(poisonList, adjacentChars[0]);
                            poisonList.RemoveAt(0);
                            Il2CppSystem.Collections.Generic.List<Character> pac = new Il2CppSystem.Collections.Generic.List<Character>();
                            pac.Add(poisonList[0]);
                            pac.Add(poisonList[poisonList.Count - 1]);
                            if (pac[0].statuses.statuses.Contains(ECharacterStatus.Corrupted) && pac[0].GetAlignment() == EAlignment.Good)
                            {
                                if (pac[0] != charRef)
                                {
                                    Il2CppSystem.Collections.Generic.List<Character> pacList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                                    pacList = CharactersHelper.GetSortedListWithCharacterFirst(pacList, pac[0]);
                                    pacList.RemoveAt(0);
                                    Il2CppSystem.Collections.Generic.List<Character> pacAc = new Il2CppSystem.Collections.Generic.List<Character>();
                                    pacAc.Add(pacList[0]);
                                    pacAc.Add(pacList[pacList.Count - 1]);
                                    if (pacAc[0].GetRegisterAs().name == "Pooka")
                                        pookaFound = true;
                                    if (pacAc[1].GetRegisterAs().name == "Pooka")
                                        pookaFound = true;
                                    if(!pookaFound)
                                        poisoned = false;
                                    pookaFound = false;
                                }
                            }
                            if (pac[1].statuses.statuses.Contains(ECharacterStatus.Corrupted) && pac[1].GetAlignment() == EAlignment.Good)
                            {
                                if (pac[1] != charRef)
                                {
                                    {
                                        Il2CppSystem.Collections.Generic.List<Character> pacList1 = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                                        pacList1 = CharactersHelper.GetSortedListWithCharacterFirst(pacList1, pac[1]);
                                        pacList1.RemoveAt(0);
                                        Il2CppSystem.Collections.Generic.List<Character> pacAc1 = new Il2CppSystem.Collections.Generic.List<Character>();
                                        pacAc1.Add(pacList1[0]);
                                        pacAc1.Add(pacList1[pacList1.Count - 1]);
                                        if (pacAc1[0].GetRegisterAs().name == "Pooka")
                                            pookaFound = true;
                                        if (pacAc1[1].GetRegisterAs().name == "Pooka")
                                            pookaFound = true;
                                        if (!pookaFound)
                                            poisoned = false;
                                        pookaFound = false;
                                    }
                                }
                            }
                            if (poisoned)
                                source[charRef.id] = adjacentChars[0];
                        }
                    }
                }

                if (adjacentChars[1].GetAlignment() == EAlignment.Evil)
                {
                    if (adjacentChars[1].GetRegisterAs().name == "Pooka")
                    {
                        source[charRef.id] = adjacentChars[1];
                        charRef.statuses.statuses.Remove(ECharacterStatus.Corrupted);
                        convinced = true;
                        if (poisoned)
                            poisoned = false;
                    }
                    else if (adjacentChars[1].GetRegisterAs().name == "Poisoner")
                    {
                        if (!convinced)
                        {
                            poisoned = true;
                            Il2CppSystem.Collections.Generic.List<Character> poisonList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                            poisonList = CharactersHelper.GetSortedListWithCharacterFirst(poisonList, adjacentChars[1]);
                            poisonList.RemoveAt(0);
                            Il2CppSystem.Collections.Generic.List<Character> pac1 = new Il2CppSystem.Collections.Generic.List<Character>();
                            pac1.Add(poisonList[0]);
                            pac1.Add(poisonList[poisonList.Count - 1]);
                            if (pac1[0].statuses.statuses.Contains(ECharacterStatus.Corrupted) && pac1[0].GetAlignment() == EAlignment.Good)
                            {
                                if (pac1[0] != charRef)
                                {
                                    Il2CppSystem.Collections.Generic.List<Character> pacList2 = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                                    pacList2 = CharactersHelper.GetSortedListWithCharacterFirst(pacList2, pac1[0]);
                                    pacList2.RemoveAt(0);
                                    Il2CppSystem.Collections.Generic.List<Character> pacAc2 = new Il2CppSystem.Collections.Generic.List<Character>();
                                    pacAc2.Add(pacList2[0]);
                                    pacAc2.Add(pacList2[pacList2.Count - 1]);
                                    if (pacAc2[0].GetRegisterAs().name == "Pooka")
                                        pookaFound = true;
                                    if (pacAc2[1].GetRegisterAs().name == "Pooka")
                                        pookaFound = true;
                                    if (!pookaFound)
                                        poisoned = false;
                                    pookaFound = false;
                                }
                            }
                            if (pac1[1].statuses.statuses.Contains(ECharacterStatus.Corrupted) && pac1[1].GetAlignment() == EAlignment.Good)
                            {
                                if (pac1[1] != charRef)
                                {
                                    Il2CppSystem.Collections.Generic.List<Character> pacList3 = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                                    pacList3 = CharactersHelper.GetSortedListWithCharacterFirst(pacList3, pac1[1]);
                                    pacList3.RemoveAt(0);
                                    Il2CppSystem.Collections.Generic.List<Character> pacAc3 = new Il2CppSystem.Collections.Generic.List<Character>();
                                    pacAc3.Add(pacList3[0]);
                                    pacAc3.Add(pacList3[pacList3.Count - 1]);
                                    if (pacAc3[0].GetRegisterAs().name == "Pooka")
                                        pookaFound = true;
                                    if (pacAc3[1].GetRegisterAs().name == "Pooka")
                                        pookaFound = true;
                                    if (!pookaFound)
                                        poisoned = false;
                                    pookaFound = false;
                                }
                            }
                        }
                    }
                }

                if(!poisoned && !convinced)
                {
                    foreach(Character c in Gameplay.CurrentCharacters)
                    {
                        if(c.GetRegisterAs().name == "Plague Doctor")
                        {
                            plagued = true;
                            source[charRef.id] = c;
                            break;
                        }
                    }
                }
            }
            if (poisoned || convinced || plagued)
            {
                charRef.statuses.CheckIfCanCurePoisonAndCure();
            }
            else if (charRef.statuses.statuses.Contains(PossessedStatus.pos))
                source[charRef.id] = charRef;
        }
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetBluffInfo(charRef));
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return null;
    }

    public Germaphobe() : base(ClassInjector.DerivedConstructorPointer<Germaphobe>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Germaphobe(System.IntPtr ptr) : base(ptr)
    {

    }
}
