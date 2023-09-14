using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    private string questName;

    public void InitializeQuestStep(string questId)
    {
        this.questName = questId;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            
            QuestEventsManager.instance.questEvents.AdvanceQuest(questName);
            
            Destroy(this.gameObject);
        }
    }
}
