using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step101 : QuestStep
{
    private int spaces;

    private MainInput _input;

    private void Awake()
    {
        _input = new MainInput();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        if (_input.Player.ShowGUI.triggered)
        {
            spaces += 1;
            PopupManagement_MNG.GetInstance().CreatePopUp(spaces + "/6");
        }

        if (spaces >= 6)
        {
            FinishQuestStep();
        }
    }
}
