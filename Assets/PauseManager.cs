using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Visuals")] public GameObject pause;
    public Transform selector, menuOptions;

    [Header("Visuals/Items")] public GameObject items;
    public Transform itemsOptions, toolsOptions;
    public GameObject prefab_items;
    public TextMeshProUGUI bio_items;

    [Header("Visuals/Prefs")] public GameObject prefs;
    public Transform prefsOptions;

    // Audio

    [Header("Audio")] public AudioClip select;
    public AudioClip back;
    public AudioClip confirm;
    public AudioClip blocked;
    private AudioSource _source;
    private InventorySlot obj;
    public static bool headsUpDisplay;
    public static int dialogueSpeed;
    public static bool fullscreen;
    public static int maxFps;

    // Prefs

    private string hudState()
    {
        if (headsUpDisplay)
        {
            return "Yes";
        }
        else
        {
            return "No";
        }
    }

    private string screenState()
    {
        if (fullscreen)
        {
            return "Full";
        }
        else
        {
            return "Windowed";
        }
    }

    private string dialogueState()
    {
        string state = "";
        switch (dialogueSpeed)
        {
            case 0:
                state = "Slow";
                break;
            
            case 1:
                state = "Mid";
                break;
            
            case 2:
                state = "Fast";
                break;
        }

        return state;
    }

// Parameters

    public static bool paused;
    private int index;
    private List<Transform> options = new List<Transform>();
    private List<Transform> itemsOptions_list = new List<Transform>();
    private List<Transform> toolsOptions_list = new List<Transform>();
    private List<Transform> prefsOptions_list = new List<Transform>();
    private int state;
    private int currentMax;

    private bool itemsEnabled()
    {
        if (InventoryManagement_PLR.GetInstance().inventory.Container.Count <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    // Input

    private MainInput _input;

    private void Awake()
    {
        _input = new MainInput();
    }

    private void Start()
    {
        // Getting the options' transforms.
        
        foreach (Transform child in menuOptions)
        {
            options.Add(child);
        }
        
        foreach (Transform child in toolsOptions)
        {
            toolsOptions_list.Add(child);
        }
        
        foreach (Transform child in prefsOptions)
        {
            prefsOptions_list.Add(child);
        }

        maxFps = 60;
        dialogueSpeed = 2;
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

    private void Update()
    {
        // Clamping.

        if (state == 0)
        {
            index = Mathf.Clamp(index, 0, options.Count - 1);
            currentMax = options.Count - 1;

        } else if (state == 1)
        {
            index = Mathf.Clamp(index, 0, itemsOptions_list.Count - 1);
            currentMax = itemsOptions_list.Count - 1;

        } else if (state == 2)
        {
            index = Mathf.Clamp(index, 0, toolsOptions_list.Count - 1);
            currentMax = toolsOptions_list.Count - 1;
        } else if (state == 3)
        {
            index = Mathf.Clamp(index, 0, prefsOptions_list.Count - 1);
            currentMax = prefsOptions_list.Count - 1;
        } 

        // Alternating paused.
        
        if (_input.Player.Pause.triggered)
        {
            if (state == 0)
            {
                HandleGUI();
            }
        }
        
        // Changing index.

        if (paused && state != 2 && state != 3)
        {

            if (_input.Player.Up_Pause.triggered)
            {
                if (index != 0)
                {
                    index -= 1;
                    _source.PlayOneShot(select);
                }
                else
                {
                    _source.PlayOneShot(blocked);
                }
            }

            if (_input.Player.Down_Pause.triggered)
            {
                if (index != currentMax)
                {
                    index += 1;
                    _source.PlayOneShot(select);
                }
                else
                {
                    _source.PlayOneShot(blocked);
                }
            }
            
        } else if (paused && state == 2)
        {
            if (_input.Player.Left_Pause.triggered)
            {
                if (index != 0)
                {
                    index -= 1;
                    _source.PlayOneShot(select);
                }
                else
                {
                    _source.PlayOneShot(blocked);
                }
            }

            if (_input.Player.Right_Pause.triggered)
            {
                if (index != currentMax)
                {
                    index += 1;
                    _source.PlayOneShot(select);
                }
                else
                {
                    _source.PlayOneShot(blocked);
                }
            }
        } else if (paused && state == 3)
        {
            if (_input.Player.Up_Pause.triggered)
            {
                if (index != 0)
                {
                    index -= 1;
                    _source.PlayOneShot(select);
                }
                else
                {
                    _source.PlayOneShot(blocked);
                }
            }

            if (_input.Player.Down_Pause.triggered)
            {
                if (index != currentMax)
                {
                    index += 1;
                    _source.PlayOneShot(select);
                }
                else
                {
                    _source.PlayOneShot(blocked);
                }
            }
            
            if (_input.Player.Left_Pause.triggered)
            {
                HandleSpecialPrefsBack();
            }

            if (_input.Player.Right_Pause.triggered)
            {
                HandleSpecialPrefsFwd();
            }

            if (_input.Player.Fire.triggered)
            {
                if (index == prefsOptions_list.Count - 1)
                {
                    HandleBack();
                }
            }
        }

        //

        if (paused)
        {
            // Showing the GUI when paused is true.
            
            StartCoroutine(SetState(pause, 0.04f, true, "open", true));

            // Setting the selector's position

            if (state == 0)
            {
                selector.position = new Vector2(71, options[index].position.y);
                
            } else if (state == 1)
            {
                selector.position = new Vector2(itemsOptions_list[index].position.x - 175, itemsOptions_list[index].position.y);
                
            } else if (state == 2)
            {
                selector.position = new Vector2(toolsOptions_list[index].position.x - 86, toolsOptions_list[index].position.y);
            } else if (state == 3)
            {
                selector.position = new Vector2(prefsOptions_list[index].position.x - 190, prefsOptions_list[index].position.y + 5);
            }

            // Handling Confirms.

            if (_input.Player.Fire.triggered)
            {
                if (paused)
                {
                    HandleInput();
                }
            }
            
            // Handling Cancels.

            if (_input.Player.Cancel.triggered)
            {
                if (paused)
                {
                    HandleBack();
                }
            }
            
            // Activating all GUIS

            if (state == 1 || state == 2)
            {
                items.SetActive(true);
            }
            else
            {
                items.SetActive(false);
            }

            if (state == 2)
            {
                StartCoroutine(SetState(toolsOptions.gameObject, 0.20f, true, "open", false));
            }
            else
            {
                StartCoroutine(SetState(toolsOptions.gameObject, 0.20f, false, "open", false));
            }

            if (state == 3)
            {
                prefs.SetActive(true);
            }
            else
            {
                prefs.SetActive(false);
            }

        }
        else
        {
            // Hiding the GUI when paused is false.
            
            StartCoroutine(SetState(pause, 0.3f, false, "open", true));
        }
        
        // Items

        if (state == 1)
        {
            bio_items.text = itemsOptions_list[index].GetComponent<OWN_GUI>().item.Bio;
        }
        
        // Disabling Options

        if (!itemsEnabled())
        {
            options[0].GetComponent<TextMeshProUGUI>().color = Color.gray;
        }
        else
        {
            options[0].GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        if (state == 2)
        {

            if (obj != null)
            {
                if (!obj.item.canToss)
                {
                    toolsOptions_list[1].GetComponent<TextMeshProUGUI>().color = Color.gray;
                }
                else
                {
                    toolsOptions_list[1].GetComponent<TextMeshProUGUI>().color = Color.white;
                }

                if (!obj.item.canUse)
                {
                    toolsOptions_list[0].GetComponent<TextMeshProUGUI>().color = Color.gray;
                }
                else
                {
                    toolsOptions_list[0].GetComponent<TextMeshProUGUI>().color = Color.white;
                }
            }
        }
        
        // Handling the states of Prefs.

        prefsOptions_list[0].GetChild(2).GetComponent<TextMeshProUGUI>().text = hudState();
        prefsOptions_list[1].GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogueState();
        prefsOptions_list[2].GetChild(2).GetComponent<TextMeshProUGUI>().text = screenState();
        prefsOptions_list[3].GetChild(2).GetComponent<TextMeshProUGUI>().text = maxFps.ToString();

        Application.targetFrameRate = maxFps;
        Screen.fullScreen = fullscreen;
    }

    private void HandleGUI()
    {
        if (paused)
        {
            paused = false;
        }
        else
        {
            paused = true;
        }
    }

    private void HandleInput()
    {
        // When in normal menu.
        if (state == 0)
        {
            switch (index)
            {
                case 3:
                    HandleGUI();
                    _source.PlayOneShot(confirm);
                    break;

                case 0:
                    if (itemsEnabled())
                    {
                        StartCoroutine(wait());
                        _source.PlayOneShot(confirm);
                    }
                    else
                    {
                        _source.PlayOneShot(blocked);
                    }
                    break;
                
                case 2:
                    state = 3;
                    _source.PlayOneShot(confirm);
                    break;
            }
        }

        // When in Items menu.
        if (state == 1)
        {
            _source.PlayOneShot(confirm);
            obj = itemsOptions_list[index].GetComponent<OWN_GUI>()._slot;
            StartCoroutine(wait_two(2));
        }
        
        // When in Items' Tool menu.
        if (state == 2)
        {
            switch (index)
            {
                case 0:
                    if (obj.item.canUse)
                    {
                        Debug.Log("Used " + obj.item.Name);
                        UseItem();
                        HandleBack();
                    }
                    else
                    {
                        _source.PlayOneShot(blocked);
                    }
                    break;
                
                case 1:
                    if (obj.item.canToss)
                    {
                        Debug.Log("Tossed " + obj.item.Name);
                        InventoryManagement_PLR.GetInstance().inventory.TossItem(obj);
                        HandleBack();
                    }
                    else
                    {
                        _source.PlayOneShot(blocked);
                    }
                    break;
            }
        }
    }

    private void HandleItems()
    {
        // Getting and Setting Inventory
        foreach (InventorySlot slot in InventoryManagement_PLR.GetInstance().inventory.Container)
        {
            GameObject ownSlot = Instantiate(prefab_items, itemsOptions);
            ownSlot.GetComponent<OWN_GUI>().item = slot.item;
            ownSlot.GetComponent<OWN_GUI>()._slot = slot;
            ownSlot.GetComponent<OWN_GUI>().amount = slot.amount;
            ownSlot.GetComponent<TextMeshProUGUI>().text = ownSlot.GetComponent<OWN_GUI>().item.Name;
        }
        
        // Setting the options
        foreach (Transform child in itemsOptions)
        {
            itemsOptions_list.Add(child);
        }
        
        // Setting Up Params.
        state = 1;
        index = 0;
    }

    private void HandleBack()
    {
        if (state != 0)
        {
            
            if (state == 1)
            {
                //Destroying Items
                itemsOptions_list.Clear();

                //Destroying Items created before
                foreach (Transform child in itemsOptions)
                {
                    Destroy(child.gameObject);
                }
            }

            if (state == 2)
            {
                if (InventoryManagement_PLR.GetInstance().inventory.Container.Count >= 1)
                {
                    //Destroying Items
                    itemsOptions_list.Clear();

                    //Destroying Items created before
                    foreach (Transform child in itemsOptions)
                    {
                        Destroy(child.gameObject);
                    }

                    obj = null;
                    
                    StartCoroutine(wait());
                    
                    state -= 1;
                    index = 0;
                }
                else
                {
                    //Destroying Items
                    itemsOptions_list.Clear();

                    //Destroying Items created before
                    foreach (Transform child in itemsOptions)
                    {
                        Destroy(child.gameObject);
                    }

                    obj = null;

                    state = 0;
                    index = 0;
                }
            }
            else if (state == 3)
            {
                StartCoroutine(wait_two(0));;
                index = 0;
            }
            else
            {
                state -= 1;
                index = 0;
            }
        }
        else
        {
            HandleGUI();
        }
        
        _source.PlayOneShot(back);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.01f);
        HandleItems();
    }
    
    IEnumerator wait_two(int State)
    {
        yield return new WaitForSeconds(0.01f);
        state = State;
    }

    IEnumerator SetState(GameObject animator, float time, bool how, string bool_, bool hide)
    {
        if (how)
        {
                if (!animator.GetComponent<Animator>().GetBool(bool_))
                {
                    selector.gameObject.SetActive(false);
                    if (hide)
                    {
                        animator.SetActive(true);
                    }

                    animator.GetComponent<Animator>().SetBool(bool_, true);

                    yield return new WaitForSeconds(time);
                    selector.gameObject.SetActive(true);
                }
        }
        else
        {
                if (animator.GetComponent<Animator>().GetBool(bool_))
                {
                    animator.GetComponent<Animator>().SetBool(bool_, false);
                    if (hide)
                    {
                        yield return new WaitForSeconds(time);
                        Debug.Log("waited");
                        animator.SetActive(false);
                    }
                }
        }
    }

    private void UseItem()
    {
        InventoryManagement_PLR.GetInstance().inventory.TossItem(obj);
        PlayerStats.GetInstance().exp += obj.item.exp;
        PlayerStats.GetInstance().health += obj.item.health;
    }

    private void HandleSpecialPrefsFwd()
    {
        Debug.Log("called");
        switch (index)
        {
            case 0:
                if (headsUpDisplay)
                {
                    headsUpDisplay = false;
                }
                else
                {
                    headsUpDisplay = true;
                }
                break;
            
            case 1:
                if (dialogueSpeed == 0)
                {
                    dialogueSpeed = 1;
                } else if (dialogueSpeed == 1)
                {
                    dialogueSpeed = 2;
                } else if (dialogueSpeed == 2)
                {
                    dialogueSpeed = 0;
                }

                break;
            
            case 2:
                if (fullscreen)
                {
                    fullscreen = false;
                }
                else
                {
                    fullscreen = true;
                }
                break;
            
            case 3:
                if (maxFps < 300)
                {
                    maxFps += 10;
                }

                break;
        }
        
        _source.PlayOneShot(confirm);
    }
    
    private void HandleSpecialPrefsBack()
    {
        Debug.Log("called");
        switch (index)
        {
            case 0:
                if (headsUpDisplay)
                {
                    headsUpDisplay = false;
                }
                else
                {
                    headsUpDisplay = true;
                }
                break;
            
            case 1:
                if (dialogueSpeed == 0)
                {
                    dialogueSpeed = 2;
                } else if (dialogueSpeed == 1)
                {
                    dialogueSpeed = 0;
                } else if (dialogueSpeed == 2)
                {
                    dialogueSpeed = 1;
                }

                break;
            
            case 2:
                if (fullscreen)
                {
                    fullscreen = false;
                }
                else
                {
                    fullscreen = true;
                }
                break;
            
            case 3:
                if (maxFps > 30)
                {
                    maxFps -= 10;
                }

                break;
        }
        
        _source.PlayOneShot(confirm);
    }
}
