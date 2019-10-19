using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlider : Singleton<DoorSlider>
{
    public Vector3 doorOpenPosition;
    public Vector3 doorClosedPosition;
    public float doorSpeed;
    public Transform leftHand;
    public Transform rightHand;

    private float timeBetweenCloses;
    private Transform door;
    private float journeyLength;


    private void OnEnable()
    {
        BoxEventSystem.OnTrialStart += NewTrial;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnTrialStart -= NewTrial;
    }

    // Start is called before the first frame update  
    void Start()
    {
        door = GetComponent<Transform>();
        journeyLength = Vector3.Distance(doorOpenPosition, doorClosedPosition);

    }


    private void Update()
    {
        if (GetComponent<Collider>().bounds.Contains(leftHand.position)
            || GetComponent<Collider>().bounds.Contains(rightHand.position))
        {
            print("Hand cookies??");
        }
    }

    private void NewTrial()
    {
        DetermineTimeBetweenCloses(ConditionManager.Instance.frequencyValues[(int)TrialManager.currentCondition.frequency]);
        StartCycle();
    }

    public void StartCycle()
    {
        IEnumerator doorCycle = CycleDoor();
        StartCoroutine(doorCycle);
    }

    private IEnumerator CycleDoor()
    {
        yield return new WaitForEndOfFrame();
        float fracJourney = 0;
        float elapsedTime = 0;
        float distCovered;

        while (fracJourney < 1)
        {
            elapsedTime += Time.deltaTime;
            distCovered = elapsedTime * doorSpeed;
            fracJourney = distCovered / journeyLength;

            door.localPosition = Vector3.Lerp(doorClosedPosition, doorOpenPosition, fracJourney);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(timeBetweenCloses);

        fracJourney = 0;
        elapsedTime = 0;

        while (fracJourney < 1)
        {
            elapsedTime += Time.deltaTime;
            distCovered = elapsedTime * doorSpeed;
            fracJourney = distCovered / journeyLength;

            door.localPosition = Vector3.Lerp(doorOpenPosition, doorClosedPosition, fracJourney);
            yield return new WaitForEndOfFrame();
        }

        BoxEventSystem.Instance.RaiseDoorClosed(timeBetweenCloses);
        //yield return new WaitForSeconds(timeBetweenCloses);
    }

    public void DetermineTimeBetweenCloses(float frequency)
    {
        float timeForACycle = 60 / frequency;
        float timeToBeStill = timeForACycle - ((journeyLength * 2) / doorSpeed);
        timeBetweenCloses = timeToBeStill / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        if(other.CompareTag("Hand"))
        {
            print("Hand in cookie jar!");
        }
        else if(other.CompareTag("Ball"))
        {
            print("Ball collision");
        }
    }
}
