using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    public TextMeshProUGUI health, sth;
    public Slider healthSlider, sthSlider, HUDHEALTH, HUDSTH;
    public Animator gui;
    public Animator alwaysHUD;
    
    [Header("Audio")]
    public AudioClip back;
    public AudioClip confirm;
    private AudioSource _source;
    
    public MainInput _input;

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
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        alwaysHUD_bool = PauseManager.headsUpDisplay;
        
        health.text = PlayerStats.GetInstance().health + "/" + PlayerStats.GetInstance().maxHealth;
        sth.text = PlayerStats.GetInstance().tp + "/" + PlayerStats.GetInstance().maxTp;
        healthSlider.value = PlayerStats.GetInstance().health;
        healthSlider.maxValue = PlayerStats.GetInstance().maxHealth;
        sthSlider.value = PlayerStats.GetInstance().tp;
        sthSlider.maxValue = PlayerStats.GetInstance().maxTp;
        
        HUDHEALTH.value = PlayerStats.GetInstance().health;
        HUDHEALTH.maxValue = PlayerStats.GetInstance().maxHealth;
        HUDSTH.value = PlayerStats.GetInstance().tp;
        HUDSTH.maxValue = PlayerStats.GetInstance().maxTp;

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
