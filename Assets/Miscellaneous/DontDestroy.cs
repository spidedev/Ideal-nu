using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static List<string> dontdestroys = new List<string>();

    public bool bootup;
    // Start is called before the first frame update

    private void Awake()
    {
        bootup = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        int index = 0;
        foreach (string found in dontdestroys)
        {
            index += 1;
            Debug.Log(found + " found at index " + index);
        }
        if (!dontdestroys.Contains(gameObject.name))
        {
            DontDestroyOnLoad(gameObject);
            dontdestroys.Add(gameObject.name);
            bootup = true;
        }
        else
        {
            Destroy(gameObject);
        }

        if (bootup)
        {
            foreach (Transform child in transform)
            {
                if (child.name != "Dialogue Box" && child.name != "Pause")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
