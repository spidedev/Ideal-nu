using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventsManager : MonoBehaviour
{
    public static QuestEventsManager instance;

    public QuestEvents questEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Something Happened.\nThere is more than one instance of the script QuestEventsManager.");
        }

        instance = this;

        questEvents = new QuestEvents();
    }
}
