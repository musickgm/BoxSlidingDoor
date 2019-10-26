using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDependentObjects : MonoBehaviour
{
    public GameObject[] controllerObjects;
    public GameObject[] gestureObjects;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerSpawner.selectionType == SelectionType.controller)
        {
            ToggleItems(true, false);
        }
        else if (PlayerSpawner.selectionType == SelectionType.gesture)
        {
            ToggleItems(false, true);
        }
    }

    private void ToggleItems(bool usingController, bool usingGesture)
    {
        for(int i = 0; i < controllerObjects.Length; i++)
        {
            controllerObjects[i].SetActive(usingController);
        }
        for(int i = 0; i < gestureObjects.Length; i++)
        {
            gestureObjects[i].SetActive(usingGesture);
        }
    }
}
