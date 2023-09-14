using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents
{
    public event Action<string> onStartQuest;

    public void StartQuest(string name)
    {
        if (onStartQuest != null)
        {
            onStartQuest(name);
        }
    }
    
    public event Action<string> onAdvanceQuest;

    public void AdvanceQuest(string name)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(name);
        }
    }
    
    public event Action<string> onFinishQuest;

    public void FinishQuest(string name)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(name);
        }
    }
    
    public event Action<Quest> onQuestStateChange;

    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }
}
