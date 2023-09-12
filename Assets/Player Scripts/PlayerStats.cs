using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;

    public int health, maxHealth, exp, maxExp, stackLimit, level, tp, maxTp;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Something Happened.\n There is more than one instance of Player Stats (scripts) in this scene. Please delete one Script so the game can continue smoothly.");
        }

        instance = this;
    }
    
    public static PlayerStats GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        health = 100;
        maxHealth = 100;
        exp = 1;
        maxExp = 200;
        stackLimit = 8;
        level = 1;
        tp = 1;
        maxTp = 50;
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (exp >= maxExp)
        {
            LevelUp();
        }

        switch (level)
        {
            case 1:
                stackLimit = 8;
                maxExp = 200;
                break;
            
            case 2:
                stackLimit = 9;
                maxExp = 400;
                break;
            
            case 3:
                stackLimit = 10;
                maxExp = 600;
                break;
            
            case 4:
                stackLimit = 11;
                maxExp = 800;
                break;
            
            case 5:
                stackLimit = 12;
                maxExp = 1000;
                break;
            
            case 6:
                stackLimit = 13;
                maxExp = 1200;
                break;
            
            case 7:
                stackLimit = 14;
                maxExp = 1400;
                break;
            
            case 8:
                stackLimit = 15;
                maxExp = 1600;
                break;
            
            case 9:
                stackLimit = 16;
                maxExp = 1800;
                break;
            
            case 10:
                stackLimit = 17;
                maxExp = 2000;
                break;
            
            case 11:
                stackLimit = 18;
                maxExp = 2200;
                break;
            
            case 12:
                stackLimit = 19;
                maxExp = 2400;
                break;
            
            case 13:
                stackLimit = 20;
                maxExp = 2600;
                break;
        }
    }

    private void LevelUp()
    {
        level += 1;
        exp = 0;
        maxHealth = 100 * level;
        maxTp = 50 * level;
        PopupManagement_MNG.GetInstance().CreatePopUp("Leveled up to " + level + "!\nStack limit leveled up to " + stackLimit + "!");
    }
}
