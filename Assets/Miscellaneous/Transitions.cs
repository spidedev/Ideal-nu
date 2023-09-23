using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitions : MonoBehaviour
{
    public Animator anim;

    public static Transitions instance;

    public static bool transitioning;
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Something Happened.\n There is more than one instance of Transitions (scripts) in this scene. Please delete one Manager so the game can continue smoothly.");
        }

        instance = this;
    }

    public void TransitionFunc()
    {
        StartCoroutine(Transition());
    }

    public IEnumerator Transition()
    {
        transitioning = true;
        anim.SetBool("in", true);
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("in", false);
        transitioning = false;
    }
    
}
