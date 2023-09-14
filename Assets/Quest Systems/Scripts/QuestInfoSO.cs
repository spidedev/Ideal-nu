using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Information", menuName = "Quest System/QuestInfo", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [Header("General")] 
    
    public string name;

    [Header("Requirements")] public int levelRequirement;

    public QuestInfoSO[] questRequisites;

    [Header("Steps")] public GameObject[] questStepsPrefabs;

    [Header("Rewards")] public int goldReward;
    public int expReward;
}
