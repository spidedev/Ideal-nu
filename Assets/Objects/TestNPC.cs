using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using randomL = System.Random;

public class TestNPC : MonoBehaviour
{
    private bool _playerNear, _timercontinue, _cooldown, _canCooldown;
    private GameObject _prompt;
    private Slider _promptCooldown;
    private float _timer, _maxTime, _secsToCooldown;
    [SerializeField] private ObjectSettings _settings;
    [SerializeField] private TextAsset _dialogue;
    public AudioSource source;
    public static GameObject selected;
    
    [Header("Mission Only")]
    
    public bool isMission;

    private int plrLvl;

    [SerializeField] QuestInfoSO questInfoForPoint;
    private string QuestName;
    private QuestState currentQuestState;
    public bool StartPoint, FinishPoint;
    [SerializeField] private TextAsset _finishdialogue;
    

    [Header("(Don't use if not an Item.)")]
    public ItemObject item;

    public int amount;

    private void Awake()
    {
        if (isMission)
        {
            QuestName = questInfoForPoint.name;
        }
    }

    private void ChangeLVL(int level)
    {
        plrLvl = level;
    }

    // Start is called before the first frame update
    void Start()
    {
        _prompt = transform.GetChild(0).transform.GetChild(0).gameObject;
        _promptCooldown = _prompt.GetComponent<Slider>();
        _maxTime = _settings.MaxTimeToPressDown;
        _prompt.SetActive(false);
        _canCooldown = _settings.CanCooldown;
        _secsToCooldown = _settings.WaitAfterCooldown;
        source = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        _timer = Mathf.Clamp(_timer, 0, _maxTime);
        
        _promptCooldown.value = _timer;
        _promptCooldown.maxValue = _maxTime;

        if (_cooldown || DialogueManager.GetInstance()._dialogueIsPlaying || PauseManager.paused)
        {
            _prompt.SetActive(false);
            return;
        }

        if (selected == gameObject)
        {
            _prompt.SetActive(true);
        }
        else
        {
            _prompt.SetActive(false);
        }

        if (_playerNear)
        {
            if (selected == null)
            {
                selected = gameObject;
            }
        }
        else
        {
            if (selected == gameObject)
            {
                selected = null;
            }
        }

        if (_timercontinue)
        {
            _timer += 0.3f;
        }
    }

    private void OnEnable()
    {
        PlayerMovement._confirmed += AddTimer;
        PlayerMovement._canceledconfirmed += StopTimer;
        if (isMission)
        {
            QuestEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
            PlayerStats.LevelChanged += ChangeLVL;
        }
    }

    private void OnDisable()
    {
        PlayerMovement._confirmed -= AddTimer;
        PlayerMovement._canceledconfirmed -= StopTimer;
        
        if (isMission)
        {
            QuestEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
            PlayerStats.LevelChanged -= ChangeLVL;
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.name.Equals(QuestName))
        {
            currentQuestState = quest.state;
            Debug.Log("Quest with name of: " + quest.info.name + " updated its state to " + currentQuestState.ToString().ToLower());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNear = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNear = false;
            _timer = 0;
        }
    }

    void AddTimer()
    {
        if (selected == gameObject)
        {
            _timercontinue = true;
        }
    }

    void StopTimer()
    {
        if (_timer >= _maxTime)
        {
            Announce();
        }
        _timercontinue = false;
        _timer = 0;
    }

    void Announce()
    {
        if (selected == gameObject)
        {
            if (_settings.type == ObjectSettings.Type.NPC)
            {
                if (!isMission || isMission && currentQuestState != QuestState.CAN_FINISH && currentQuestState != QuestState.FINISHED)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(_dialogue);
                }
                else if (isMission && currentQuestState == QuestState.CAN_FINISH)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(_finishdialogue);
                }
                else if (isMission && currentQuestState == QuestState.FINISHED)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(_finishdialogue);
                }

                if (isMission)
                {
                    if (currentQuestState.Equals(QuestState.CAN_START) && StartPoint)
                    {
                        QuestEventsManager.instance.questEvents.StartQuest(QuestName);
                    } else if (currentQuestState.Equals(QuestState.CAN_FINISH) && FinishPoint)
                    {
                        QuestEventsManager.instance.questEvents.FinishQuest(QuestName);
                    }
                    else
                    {
                        if (currentQuestState.Equals(QuestState.REQUIREMENTS_NOT_MET))
                        {
                            if (plrLvl < questInfoForPoint.levelRequirement)
                            {
                                PopupManagement_MNG.GetInstance().CreatePopUp("Your level is too low for this mission.\nBe level " + questInfoForPoint.levelRequirement + " to start the mission");
                            }
                            else
                            {
                                PopupManagement_MNG.GetInstance().CreatePopUp("You have to do something else before starting this mission.");
                            }
                        }
                    }
                }
            } else if (_settings.type == ObjectSettings.Type.Item)
            {
                if (_settings.DialogueNeeded)
                {
                    DialogueManager.GetInstance().EnterDialogueMode(_dialogue);
                    AddObject();
                }
                else
                {
                    AddObject();
                }
            }
        }
        else
        {
            Debug.Log("Player not near");
        }
    }

    public void AddObject()
    {
        InventoryManagement_PLR.GetInstance().inventory.AddItem(item, amount);
        source.PlayOneShot(_settings.clips[Random.Range(0, _settings.clips.Length)]);
        Destroy(gameObject, 0.04f);
    }

    IEnumerator CoolingDown()
    {
        _cooldown = true;
        yield return new WaitForSeconds(_secsToCooldown);
        _cooldown = false;
    }
}
