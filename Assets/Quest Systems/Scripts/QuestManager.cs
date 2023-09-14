using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private int currentLevel;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        QuestEventsManager.instance.questEvents.onStartQuest += StartQuest;
        QuestEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        QuestEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
        
        PlayerStats.LevelChanged += PlayerLevelChange;
    }
    
    private void OnDisable()
    {
        QuestEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        QuestEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        QuestEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;

        PlayerStats.LevelChanged -= PlayerLevelChange;
    }

    private void PlayerLevelChange(int lvl)
    {
        Debug.Log("Change");
        currentLevel = lvl;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        if (currentLevel < quest.info.levelRequirement)
        {
            meetsRequirements = false;
        }

        foreach (QuestInfoSO prerequisite in quest.info.questRequisites)
        {
            if (getQuestById(prerequisite.name).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            QuestEventsManager.instance.questEvents.QuestStateChange(quest);
        }
        
    }

    private void ChangeQuestState(string Name, QuestState state)
    {
        Quest quest = getQuestById(Name);
        quest.state = state;
        QuestEventsManager.instance.questEvents.QuestStateChange(quest);
        
    }

    public void StartQuest(string name)
    {
        Quest quest = getQuestById(name);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.name, QuestState.IN_PROGRESS);
    }
    
    public void AdvanceQuest(string name)
    {
        Quest quest = getQuestById(name);
        
        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
            PopupManagement_MNG.GetInstance().CreatePopUp("Step: " + quest.currentStepIndex + "/" + (quest.info.questStepsPrefabs.Length - 1));
        }
        else
        {
            ChangeQuestState(quest.info.name, QuestState.CAN_FINISH);
        }
    }
    
    public void FinishQuest(string name)
    {
        Quest quest = getQuestById(name);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.name, QuestState.FINISHED);
        PopupManagement_MNG.GetInstance().CreatePopUp("Quest Completed");
    }

    private void ClaimRewards(Quest quest)
    {
        PlayerStats.instance.GainCurrency(quest.info.goldReward);
        PlayerStats.instance.GainExp(quest.info.expReward);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.name))
            {
                Debug.LogWarning("Something Happened.\nDuplicate ID found when creating Quest Map: " + questInfo.name);
            }

            idToQuestMap.Add(questInfo.name, new Quest(questInfo));
            
        }

        return idToQuestMap;
    }

    private Quest getQuestById(string name_)
    {
        Quest quest = questMap[name_];

        if (quest == null)
        {
            Debug.LogError("Something Happened.\nID not found in Quest Map: " + name_);
        }

        return quest;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.name, QuestState.CAN_START);
            }
        }
    }
}
