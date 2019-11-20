using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlider : Singleton<DoorSlider>
{
    public Vector3 doorOpenPosition;
    public Vector3 doorClosedPosition;
    public Color goodColor;
    public Color badColor;

    private float doorSpeed;
    private float timeBetweenCloses = 0;
    private Transform door;
    private float journeyLength;
    private IEnumerator doorCycle;


    private void OnEnable()
    {
        BoxEventSystem.OnTrialStart += NewTrial;
        BoxEventSystem.OnTrialEnd += SetLasersAlarm;
    }

    private void OnDisable()
    {
        BoxEventSystem.OnTrialStart -= NewTrial;
        BoxEventSystem.OnTrialEnd -= SetLasersAlarm;
    }

    // Start is called before the first frame update  
    void Start()
    {
        door = GetComponent<Transform>();
        journeyLength = Vector3.Distance(doorOpenPosition, doorClosedPosition);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if(Box.ballClone == null)
            {
                return;
            }
            TrialManager.Instance.SetAlarm();
        }
        else if (other.CompareTag("Ball"))
        {
            TrialManager.Instance.SetAlarm();
        }
    }

    private void NewTrial()
    {
        DetermineTimeBetweenCloses(ConditionManager.Instance.frequencyValues[(int)TrialManager.currentCondition.frequency]);
        SetLasers(false);
        StartCycle();
    }

    public void StartCycle()
    {
        if(doorCycle != null)
        {
            StopCoroutine(doorCycle);
        }
        doorCycle = CycleDoor();
        StartCoroutine(doorCycle);
    }

    public void EndCycle()
    {
        if(doorCycle != null)
        {
            StopCoroutine(doorCycle);
        }
        doorCycle = ReturnDoor();
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

    private IEnumerator ReturnDoor()
    {
        yield return new WaitForEndOfFrame();
        float fracJourney = 0;
        float elapsedTime = 0;
        float distCovered;
        Vector3 currentPosition = door.localPosition;
        float returnJourneyLength = Vector3.Distance(currentPosition, doorClosedPosition);

        while (fracJourney < 1)
        {
            elapsedTime += Time.deltaTime;
            distCovered = elapsedTime * doorSpeed;
            fracJourney = distCovered / returnJourneyLength;

            door.localPosition = Vector3.Lerp(currentPosition, doorClosedPosition, fracJourney);
            yield return new WaitForEndOfFrame();
        }
    }

    public void DetermineTimeBetweenCloses(float frequency)
    {
        float timeForACycle = 60 / frequency;
        //float timeToBeStill = timeForACycle - ((journeyLength * 2) / doorSpeed);
        //timeBetweenCloses = timeToBeStill / 2;
        doorSpeed = (journeyLength * 2) / timeForACycle;
    }



    private void SetLasersAlarm(Condition _condition, bool _success)
    {
        if(!_success)
        {
            SetLasers(true);
        }
    }

    private void SetLasers(bool alarmed)
    {
        Weapon lasers = GetComponent<Weapon>();
        if(lasers != null)
        {
            if(alarmed)
            {
                lasers.bladeColor = badColor;
            }
            else
            {
                lasers.bladeColor = goodColor;
            }
            lasers.InitializeBladeColor();
        }

    }
}
