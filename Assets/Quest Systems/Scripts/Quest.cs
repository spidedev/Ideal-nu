using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;

    public QuestState state;

    public int currentStepIndex { get; private set; }

    public Quest(QuestInfoSO info)
    {
        this.info = info;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentStepIndex < info.questStepsPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.name);
            Debug.Log(info.name);
        }
    }

    private GameObject GetCurrentStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepsPrefabs[currentStepIndex];
        }
        else
        {
            Debug.LogError("Something Happened.\nTried to get step prefab, but StepIndex was out of range, indicating that there's no current step: QuestID: " + info.name + ", StepIndex: " + currentStepIndex);
        }

        return questStepPrefab;
    }
}
