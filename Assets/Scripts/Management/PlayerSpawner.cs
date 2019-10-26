using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public SelectionType selectionSetting;
    public static SelectionType selectionType;

    public GameObject viveController;
    public GameObject leapGestureController;

    // Start is called before the first frame update
    void Awake()
    {
        if (selectionSetting != SelectionType.inheritPreviousScene)
        {
            selectionType = selectionSetting;
        }

        SpawnCharacter();
    }
    private void SpawnCharacter()
    {
        if (selectionType == SelectionType.controller)
        {
            leapGestureController.SetActive(false);
            viveController.SetActive(true);
        }
        else if (selectionType == SelectionType.gesture)
        {
            viveController.SetActive(false);
            leapGestureController.SetActive(true);
        }
    }
}
