using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    public TextMeshProUGUI health, sth, level;
    public Slider healthSlider, sthSlider, HUDHEALTH, HUDSTH;
    public Animator gui;
    public Animator alwaysHUD;
    
    [Header("Audio")]
    public AudioClip back;
    public AudioClip confirm;
    private AudioSource _source;
    
    public MainInput _input;

    private int currentLVL;

    public bool alwaysHUD_bool, closeAHUD;

    private void Awake()
    {
        _input = new MainInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        _source = gameObject.AddComponent<AudioSource>();
    }
    
    private void OnEnable()
    {
        _input.Enable();
        PlayerStats.LevelChanged += updateLevel;
    }

    private void OnDisable()
    {
        _input.Disable();
        PlayerStats.LevelChanged -= updateLevel;
    }

    private void updateLevel(int level)
    {
        currentLVL = level;
    }

    // Update is called once per frame
    void Update()
    {
        alwaysHUD_bool = PauseManager.headsUpDisplay;
        
        health.text = PlayerStats.instance.health + "/" + PlayerStats.instance.maxHealth;
        sth.text = PlayerStats.instance.tp + "/" + PlayerStats.instance.maxTp;
        healthSlider.value = PlayerStats.instance.health;
        healthSlider.maxValue = PlayerStats.instance.maxHealth;
        sthSlider.value = PlayerStats.instance.tp;
        sthSlider.maxValue = PlayerStats.instance.maxTp;
        level.text = "Level " + currentLVL.ToString();
        
        
        HUDHEALTH.value = PlayerStats.instance.health;
        HUDHEALTH.maxValue = PlayerStats.instance.maxHealth;
        HUDSTH.value = PlayerStats.instance.tp;
        HUDSTH.maxValue = PlayerStats.instance.maxTp;

        if (!closeAHUD && alwaysHUD_bool)
        {
            if (!alwaysHUD.GetBool("open"))
            {
                alwaysHUD.SetBool("open", true);
            }
        } else if (!alwaysHUD_bool)
        {
            if (alwaysHUD.GetBool("open"))
            {
                alwaysHUD.SetBool("open", false);
            }
        }

        if (_input.Player.ShowGUI.triggered)
        {
            if (gui.GetBool("open"))
            {
                if (alwaysHUD_bool)
                {
                    alwaysHUD.SetBool("open", true);
                }

                closeAHUD = false;
                gui.SetBool("open", false);
                _source.PlayOneShot(back);
            }
            else
            {
                if (alwaysHUD.GetBool("open"))
                {
                    alwaysHUD.SetBool("open", false);
                }
                
                gui.SetBool("open", true);
                closeAHUD = true;
                _source.PlayOneShot(confirm);
            }
        }
        
        if (_input.Player.Cancel.triggered)
        {
            if (gui.GetBool("open"))
            {
                if (alwaysHUD.GetBool("open"))
                {
                    alwaysHUD.SetBool("open", false);
                }

                closeAHUD = false;
                gui.SetBool("open", false);
                _source.PlayOneShot(back);
            }
        }
    }
}
