using CarlzVilliagePack;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppTMPro;
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
public class ExecBluff : Role
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
        int rng = UnityEngine.Random.Range(0, 6);
        if (charRef.bluff.name == "Oracle" || charRef.bluff.name == "Poet" && rng == 0)
        {
            Il2CppSystem.Collections.Generic.List<Character> oracleInfo = new Il2CppSystem.Collections.Generic.List<Character>();
            oracleInfo = getOracleInfo();
            Il2CppSystem.Collections.Generic.List<CharacterData> minion = new Il2CppSystem.Collections.Generic.List<CharacterData>(Gameplay.Instance.GetScriptCharacters().Pointer);
            minion = Characters.Instance.FilterCharacterType(minion, ECharacterType.Minion);
            if (minion.Count == 0)
                minion = new Il2CppSystem.Collections.Generic.List<CharacterData>(Gameplay.Instance.GetAllAscensionCharacters().Pointer);
            CharacterData minionChoice = minion[UnityEngine.Random.Range(0, minion.Count)];
            string info = $"#{oracleInfo[0].id} or #{oracleInfo[1].id} is a {minionChoice.name}";
            ActedInfo newInfo = new ActedInfo(info, oracleInfo);
            return newInfo;
        }
        else if (charRef.bluff.name == "Hunter" || charRef.bluff.name == "Poet" && rng == 1)
        {
            int distance = 0;
            distance = getHunterInfo(charRef);
            string info = "";
            if (distance == 1)
                info += $"I am {distance} card away from closest Evil";
            else
                info += $"I am {distance} cards away from closest Evil";
            ActedInfo newInfo = new ActedInfo(info, null);
            return newInfo;
        }
        else if (charRef.bluff.name == "Empress" || charRef.bluff.name == "Poet" && rng == 2)
        {
            Il2CppSystem.Collections.Generic.List<Character> empressInfo = new Il2CppSystem.Collections.Generic.List<Character>();
            empressInfo = getEmpressInfo();
            string info = $"One is evil:\n#{empressInfo[0].id}, #{empressInfo[1].id}, or #{empressInfo[2].id}";
            ActedInfo newInfo = new ActedInfo(info, empressInfo);
            return newInfo;
        }
        else if (charRef.bluff.name == "Bishop" || charRef.bluff.name == "Poet" && rng == 3)
        {
            Il2CppSystem.Collections.Generic.List<Character> bishopInfo = new Il2CppSystem.Collections.Generic.List<Character>();
            bishopInfo = getBishopInfo();
            string info = $"Between\n#{bishopInfo[0].id}, #{bishopInfo[1].id}, #{bishopInfo[2].id}\nthere is:\n";
            bishopInfo = ListHelper.ShuffleList(bishopInfo);
            if (bishopInfo[0].statuses.statuses.Contains(TargetStatus.target))
                info += "Minion, ";
            else
                info += $"{bishopInfo[0].GetCharacterData().type.ToString()}, ";

            if (bishopInfo[1].statuses.statuses.Contains(TargetStatus.target))
                info += "Minion, ";
            else
                info += $"{bishopInfo[1].GetCharacterData().type.ToString()} ";

            if (bishopInfo[2].statuses.statuses.Contains(TargetStatus.target))
                info += "and Minion";
            else
                info += $"and {bishopInfo[2].GetCharacterData().type.ToString()}";
            ActedInfo newInfo = new ActedInfo(info, bishopInfo);
            return newInfo;
        }
        else if (charRef.bluff.name == "Enlightened" || charRef.bluff.name == "Poet" && rng == 4)
        {
            int direction = 0;
            direction = getEnlighenedInfo(charRef);
            string info = "";
            if (direction == 0)
                info += "Closest Evil is: Clockwise";
            else if (direction == 1)
                info += "Closest Evil is: Counter-clockwise";
            else
                info += "Closest Evil is equidistant";
            ActedInfo newInfo = new ActedInfo(info, null);
            return newInfo;
        }
        else
        {
            Character targetChar = new Character();
            targetChar = getPoetInfo();
            string info = $"#{targetChar.id}\nis Evil";
            ActedInfo newInfo = new ActedInfo(info, null);
            return newInfo;
        }
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetInfo(charRef));
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return null;
    }

    private Il2CppSystem.Collections.Generic.List<Character> getOracleInfo()
    {
        Il2CppSystem.Collections.Generic.List<Character> chosenData = new Il2CppSystem.Collections.Generic.List<Character>();
        Il2CppSystem.Collections.Generic.List<Character> goodChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
        Character targetChar = new Character();
        bool targetSpotted = false;
        foreach(Character c in goodChars)
        {
            if (c.statuses.statuses.Contains(TargetStatus.target))
            {
                targetChar = c;
                targetSpotted = true;
                break;
            }
        }
        goodChars = Characters.Instance.FilterAlignmentCharacters(goodChars, EAlignment.Good);
        if (targetSpotted)
        {
            goodChars.Remove(targetChar);
            Character randomChar = new Character();
            randomChar = goodChars[UnityEngine.Random.Range(0, goodChars.Count)];
            chosenData.Add(randomChar);
            chosenData.Add(targetChar);
            if (chosenData[0].id > chosenData[1].id)
            {
                Character temp = new Character();
                temp = chosenData[1];
                chosenData[1] = chosenData[0];
                chosenData[0] = temp;
            }
            return chosenData;
        }
        else
        {
            Character randomChar1 = new Character();
            Character randomChar2 = new Character();
            randomChar1 = goodChars[UnityEngine.Random.Range(0, goodChars.Count)];
            goodChars.Remove(randomChar1);
            randomChar2 = goodChars[UnityEngine.Random.Range(0, goodChars.Count)];
            chosenData.Add(randomChar1);
            chosenData.Add(randomChar2);
            if (chosenData[0].id > chosenData[1].id)
            {
                Character temp = new Character();
                temp = chosenData[1];
                chosenData[1] = chosenData[0];
                chosenData[0] = temp;
            }
            return chosenData;
        }
    }

    private int getHunterInfo(Character charRef)
    {
        int distanceToTarget = 0;
        Il2CppSystem.Collections.Generic.List<Character> inPlayChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
        inPlayChars = CharactersHelper.GetSortedListWithCharacterFirst(inPlayChars, charRef);

        inPlayChars.RemoveAt(0);
        int currentCount = 0;
        bool targetFound = false;
        for(int i = 0; i < inPlayChars.Count; i++)
        {
            currentCount++;
            if (inPlayChars[i].statuses.statuses.Contains(TargetStatus.target))
            {
                distanceToTarget = currentCount;
                targetFound = true;
                break;
            }
        }
        int currentCountReverse = 0;
        for(int i = inPlayChars.Count - 1; i >= 0; i--)
        {
            currentCountReverse++;
            if (inPlayChars[i].statuses.statuses.Contains(TargetStatus.target))
            {
                targetFound = true;
                if (currentCountReverse < currentCount)
                    distanceToTarget = currentCountReverse;
                break;
            }
        }
        if (targetFound)
        {
            return distanceToTarget;
        }
        else
        {
            Character randomChar = new Character();
            randomChar = inPlayChars[UnityEngine.Random.Range(0, inPlayChars.Count - 1)];
            currentCount = 0;
            for (int i = 0; i < inPlayChars.Count; i++)
            {
                currentCount++;
                if (inPlayChars[i] = randomChar)
                {
                    distanceToTarget = currentCount;
                    break;
                }
            }
            currentCountReverse = 0;
            for (int i = inPlayChars.Count - 1; i >= 0; i--)
            {
                currentCountReverse++;
                if (inPlayChars[i] = randomChar)
                {
                    if (currentCountReverse < currentCount)
                        distanceToTarget = currentCountReverse;
                    break;
                }
            }
            return distanceToTarget;
        }
    }

    private Il2CppSystem.Collections.Generic.List<Character> getEmpressInfo()
    {
        Il2CppSystem.Collections.Generic.List<Character> chosenData = new Il2CppSystem.Collections.Generic.List<Character>();
        Il2CppSystem.Collections.Generic.List<Character> gameplayData = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
        Character targetChar = new Character();
        Character randomChar1 = new Character();
        Character randomChar2 = new Character();
        Character randomChar3 = new Character();
        bool targetFound = false;
        gameplayData = Characters.Instance.FilterAlignmentCharacters(gameplayData, EAlignment.Good);
        foreach(Character c in gameplayData)
        {
            if (c.statuses.statuses.Contains(TargetStatus.target))
            {
                targetChar = c;
                targetFound = true;
                break;
            }
        }
        if (targetFound)
        {
            randomChar1 = targetChar;
            gameplayData.Remove(randomChar1);
            randomChar2 = gameplayData[UnityEngine.Random.Range(0, gameplayData.Count)];
            gameplayData.Remove(randomChar2);
            randomChar3 = gameplayData[UnityEngine.Random.Range(0, gameplayData.Count)];
            chosenData.Add(randomChar1);
            chosenData.Add(randomChar2);
            chosenData.Add(randomChar3);
            for(int i = 0; i < chosenData.Count - 1; i++)
            {
                for(int j = 0; j < chosenData.Count - 1; j++)
                {
                    if (chosenData[j].id > chosenData[j + 1].id)
                    {
                        Character temp = new Character();
                        temp = chosenData[j + 1];
                        chosenData[j + 1] = chosenData[j];
                        chosenData[j] = temp;
                    }
                }
            }
            return chosenData;
        }
        else
        {
            randomChar1 = gameplayData[UnityEngine.Random.Range(0, gameplayData.Count)];
            gameplayData.Remove(randomChar1);
            randomChar2 = gameplayData[UnityEngine.Random.Range(0, gameplayData.Count)];
            gameplayData.Remove(randomChar2);
            randomChar3 = gameplayData[UnityEngine.Random.Range(0, gameplayData.Count)];
            chosenData.Add(randomChar1);
            chosenData.Add(randomChar2);
            chosenData.Add(randomChar3);
            for (int i = 0; i < chosenData.Count - 1; i++)
            {
                for (int j = 0; j < chosenData.Count - 1; j++)
                {
                    if (chosenData[j].id > chosenData[j + 1].id)
                    {
                        Character temp = new Character();
                        temp = chosenData[j + 1];
                        chosenData[j + 1] = chosenData[j];
                        chosenData[j] = temp;
                    }
                }
            }
            return chosenData;
        }
    }

    private Il2CppSystem.Collections.Generic.List<Character> getBishopInfo()
    {
        Il2CppSystem.Collections.Generic.List<Character> chosenData = new Il2CppSystem.Collections.Generic.List<Character>();
        Character targetChar = new Character();
        Character villageChar = new Character();
        Character outcastChar = new Character();
        Character evilChar = new Character();
        bool targetFound = false;
        foreach(Character c in Gameplay.CurrentCharacters)
        {
            if (c.statuses.statuses.Contains(TargetStatus.target))
            {
                targetChar = c;
                targetFound = true;
                break;
            }
        }
        if (targetFound)
        {
            evilChar = targetChar;
            Il2CppSystem.Collections.Generic.List<Character> villageChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            villageChars = Characters.Instance.FilterCharacterType(villageChars, ECharacterType.Villager);
            villageChars.Remove(targetChar);
            villageChar = villageChars[UnityEngine.Random.Range(0, villageChars.Count)];

            Il2CppSystem.Collections.Generic.List<Character> outcastChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            outcastChars = Characters.Instance.FilterCharacterType(outcastChars, ECharacterType.Outcast);
            outcastChar = outcastChars[UnityEngine.Random.Range(0, outcastChars.Count)];

            chosenData.Add(villageChar);
            chosenData.Add(outcastChar);
            chosenData.Add(evilChar);

            for (int i = 0; i < chosenData.Count - 1; i++)
            {
                for (int j = 0; j < chosenData.Count - 1; j++)
                {
                    if (chosenData[j].id > chosenData[j + 1].id)
                    {
                        Character temp = new Character();
                        temp = chosenData[j + 1];
                        chosenData[j + 1] = chosenData[j];
                        chosenData[j] = temp;
                    }
                }
            }
            return chosenData;
        }
        else
        {
            Il2CppSystem.Collections.Generic.List<Character> villageChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            villageChars = Characters.Instance.FilterCharacterType(villageChars, ECharacterType.Villager);
            villageChar = villageChars[UnityEngine.Random.Range(0, villageChars.Count)];
            chosenData.Add(villageChar);

            villageChars.Remove(villageChar);
            villageChar = villageChars[UnityEngine.Random.Range(0, villageChars.Count)];
            chosenData.Add(villageChar);

            villageChars.Remove(villageChar);
            villageChar = villageChars[UnityEngine.Random.Range(0, villageChars.Count)];

            for (int i = 0; i < chosenData.Count - 1; i++)
            {
                for (int j = 0; j < chosenData.Count - 1; j++)
                {
                    if (chosenData[j].id > chosenData[j + 1].id)
                    {
                        Character temp = new Character();
                        temp = chosenData[j + 1];
                        chosenData[j + 1] = chosenData[j];
                        chosenData[j] = temp;
                    }
                }
            }
            return chosenData;
        }
    }

    private int getEnlighenedInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> charList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
        int distanceToTarget = 0;
        int clockwiseDistance = 0;
        int counterClockwiseDistance = 0;
        bool targetFound = false;
        charList = CharactersHelper.GetSortedListWithCharacterFirst(charList, charRef);
        charList.RemoveAt(0);
        
        for(int i = 0; i < charList.Count; i++)
        {
            distanceToTarget++;
            if (charList[i].statuses.statuses.Contains(TargetStatus.target))
            {
                counterClockwiseDistance = distanceToTarget;
                targetFound = true;
                break;
            }
        }
        distanceToTarget = 0;
        for(int i = charList.Count - 1; i >= 0; i--)
        {
            distanceToTarget++;
            if (charList[i].statuses.statuses.Contains(TargetStatus.target))
            {
                clockwiseDistance = distanceToTarget;
                targetFound = true;
                break;
            }
        }
        if (targetFound)
        {
            if (counterClockwiseDistance > clockwiseDistance)
                return 0;
            else if (counterClockwiseDistance < clockwiseDistance)
                return 1;
            else
                return 2;
        }
        else
        {
            distanceToTarget = 0;
            clockwiseDistance = 0;
            counterClockwiseDistance = 0;
            for (int i = 0; i < charList.Count; i++)
            {
                distanceToTarget++;
                if (charList[i].GetAlignment() == EAlignment.Evil)
                {
                    counterClockwiseDistance = distanceToTarget;
                    break;
                }
            }
            distanceToTarget = 0;
            for (int i = charList.Count - 1; i >= 0; i--)
            {
                distanceToTarget++;
                if (charList[i].GetAlignment() == EAlignment.Evil)
                {
                    clockwiseDistance = distanceToTarget;
                    break;
                }
            }
            if (counterClockwiseDistance > clockwiseDistance)
                return 0;
            if (counterClockwiseDistance < clockwiseDistance)
                return 1;
            else
                return 2;
        }
    }

    private Character getPoetInfo()
    {
        Character targetInfo = new Character();
        bool targetFound = false;
        foreach(Character c in Gameplay.CurrentCharacters)
        {
            if (c.statuses.statuses.Contains(TargetStatus.target))
            {
                targetInfo = c;
                targetFound = true;
                break;
            }
        }
        if (targetFound)
            return targetInfo;
        else
        {
            Il2CppSystem.Collections.Generic.List<Character> goodChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            goodChars = Characters.Instance.FilterAlignmentCharacters(goodChars, EAlignment.Good);
            targetInfo = goodChars[UnityEngine.Random.Range(0, goodChars.Count)];
            return targetInfo;
        }
    }

    public ExecBluff() : base(ClassInjector.DerivedConstructorPointer<ExecBluff>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public ExecBluff(System.IntPtr ptr) : base(ptr)
    {

    }
}
public static class ExecutionerStuff
{
    public static WinConditions winConditions;
    public static GameObject exeLoss;
    public static void getWinCons()
    {
        GameObject winCon = GameObject.Find("Game/Gameplay/Content/WinConditions");
        winConditions = winCon.GetComponent<WinConditions>();
        System.Action<Character> action = new System.Action<Character>(checkExe);
        GameplayEvents.OnCharacterKilled += action;
        exeLoss = GameObject.Instantiate(winConditions.autoLose);
        GameObject exeLossNote = exeLoss.transform.FindChild("Note/Text (TMP)").gameObject;
        TextMeshProUGUI exeLossText = exeLossNote.GetComponent<TextMeshProUGUI>();
        exeLossText.text = "<color=red>Evils Win</color>\n\nThe execution target was killed!";
    }
    public static void checkExe(Character ch)
    {
        if (ch.statuses.statuses.Contains(TargetStatus.target))
        {
            ch.RevealAllReal();
            ExeLose();
        }
    }
    public static void ExeLose()
    {
        exeLoss.SetActive(true);
        winConditions.Lose();
    }
}