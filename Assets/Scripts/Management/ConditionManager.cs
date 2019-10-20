using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Condition
{
    public ObjectSize size;
    public DoorFrequency frequency;
}

[System.Serializable]
public class ConditionList
{
    [Tooltip("Set size to the number of sets (or rather the number of conditions for a given frequency). " +
        "Each list should have the same frequency throughout. There should be the same number of conditions" +
        "for each list")]
    public List<Condition> list = new List<Condition>();
}


public enum ObjectSize { size1, size2, size3, size4, size5};
public enum DoorFrequency { a, b, c, d, e, f, g, h};

/// <summary>
/// Condition manager manages our sets of trials. 
/// A set is as large as the number of frequency groups (or number of different frequencies).
/// Make sure that there are the same number of condition lists for each frequency group.
/// </summary>
public class ConditionManager : Singleton<ConditionManager>
{
    [Tooltip("All the different radius values to coorelate with size1, size2, etc")]
    public float[] radiusValues;
    [Tooltip("All the different frequency values to coorelate with a, b, c, d, e, etc")]
    public float[] frequencyValues;

    [Tooltip("Set size to the number of different frequencies")]
    public List<ConditionList> frequencyGroup = new List<ConditionList>();



    [ReadOnly] public List<ConditionList> Sets = new List<ConditionList>();
    private int numSets;
    private int numTrialsInSet;

    private void Start()
    {
        InitializeLists();
        BoxEventSystem.Instance.RaiseStartExperiment();
    }

    void InitializeLists()
    {
        numSets = frequencyGroup[0].list.Count;
        numTrialsInSet = frequencyGroup.Count;

        //Randomize order of existing frequency groups
        for (int i = 0; i < frequencyGroup.Count; i++)
        {
            frequencyGroup[i] = RandomizeList(frequencyGroup[i]);
        }
        //Fill "Set" with new condition lists
        for (int i = 0; i < numSets; i++)
        {
            //Create/add a new set to "Sets"
            Sets.Add(CreateSet(numTrialsInSet, i));
        }
        //Randomize the sets
        for (int i = 0; i < numSets; i++)
        {
            Sets[i] = RandomizeList(Sets[i]);
        }
    }


    /// <summary>
    /// Create a set.
    /// </summary>
    /// <param name="setSize"></param> How big is the set? How many trials.
    /// <param name="setIndex"></param> Which set are we on?
    /// <returns></returns>
    private ConditionList CreateSet(int setSize, int setIndex)
    {
        ConditionList newSet = new ConditionList();
        List<ObjectSize> maxedSizes = new List<ObjectSize>();

        //If we're almost halfway through the sets (but not to the last 2) - Prioritize list(s)
        if (setIndex >= (numSets/2 - 1) && setIndex < numSets - 1)
        {
            for (int i = 0; i < setSize; i++)
            {
                PrioritizeList(i);
            }
        }

        //Add conditions to list
        for (int i = 0; i < setSize; i++)
        {
            Condition newCondition = PullConditionFromFrequencyList(i, maxedSizes);
            newSet.list.Add(newCondition);
            //See if the size is maxed out (so future new conditions will be of the other size)
            if (!maxedSizes.Contains(newCondition.size) && i+1 < setSize)
            {
                if(DetermineIfSizeIsMaxed(newSet, newCondition.size))
                {
                    maxedSizes.Add(newCondition.size);
                }
            }
        }

        return newSet;
    }

    /// <summary>
    /// Take a condition from the frequency list and put it in the new set.
    /// </summary>
    /// <param name="groupNumber"></param> What frequency group # are we on (a, b, c, etc)
    /// <param name="_maxedSizes"></param> Which sizes do we need to avoid pulling from (already enough of them). 
    /// <returns></returns>
    private Condition PullConditionFromFrequencyList(int groupNumber, List<ObjectSize> _maxedSizes)
    {
        bool validCondition = false;        //Is the condition we pulled of an appropriate size
        Condition pulledCondition = new Condition();
        int index = 0;

        //Keep going through the group until we find a condition with a size that's not maxed (or we go all the way through). 
        while (index < frequencyGroup[groupNumber].list.Count && !validCondition)
        {
            pulledCondition = frequencyGroup[groupNumber].list[index];
            if (!_maxedSizes.Contains(pulledCondition.size))
            {
                validCondition = true;
            }

            index++;
        }

        //If we didn't find a valid condition (size), log an error
        if(!validCondition)
        {
            Debug.LogError("Ran out of conditions to pull from that haven't maxed their size out");
        }

        //Remove the pulled condition from the old list so that it isn't reused.
        frequencyGroup[groupNumber].list.Remove(pulledCondition);

        return pulledCondition;
    }

    /// <summary>
    /// See if we have enough of that size for the given set.
    /// </summary>
    /// <param name="currentSet"></param> The set that we are currently adding to.
    /// <param name="currentSize"></param> The size that we are checking to see if it is maxed.
    /// <returns></returns>
    private bool DetermineIfSizeIsMaxed(ConditionList currentSet, ObjectSize currentSize)
    {
        int maxNumber = numTrialsInSet / radiusValues.Length;       //The max of each size for a set.
        int currentNumber = 0;

        //See how many of that size do we already have.
        for (int i = 0; i < currentSet.list.Count; i++)
        {
            if (currentSet.list[i].size == currentSize)
            {
                currentNumber++;
            }
        }

        return currentNumber >= maxNumber;
    }

    /// <summary>
    /// Given a list, randomize it.
    /// </summary>
    /// <param name="currentList"></param> The list to be randomized.
    /// <returns></returns>
    private ConditionList RandomizeList(ConditionList currentList)
    {
        for (int i = 0; i < currentList.list.Count; i++)
        {
            Condition temp = currentList.list[i];
            int randomIndex = Random.Range(i, currentList.list.Count);
            currentList.list[i] = currentList.list[randomIndex];
            currentList.list[randomIndex] = temp;
        }

        return currentList;
    }

    /// <summary>
    /// Prioritize the list to put any sizes that only have 1 left last.
    /// Also, if a set only has one size left, put this list first in the order so it gets priority
    /// </summary>
    /// <param name="listIndex"></param> which list (or frequency bucket) are we on
    /// <returns></returns>
    private void PrioritizeList(int listIndex)
    {
        bool priorityList = false;      //Does this list need to be first
        ObjectSize sizeWithOneLeft = new ObjectSize();
        List<int> amountOfEachSize = new List<int>(new int[System.Enum.GetValues(typeof(ObjectSize)).Length]);

        //Determine the amount of each size in the group (i.e. 2 small, 1 large)
        for (int i = 0; i < frequencyGroup[listIndex].list.Count; i++)
        {
            amountOfEachSize[(int)frequencyGroup[listIndex].list[i].size]++;
        }

        //Determine which size only has one left (if applicable).
        for (int i = 0; i < amountOfEachSize.Count; i++)
        {
            if (amountOfEachSize[i] == 1)
            {
                sizeWithOneLeft = (ObjectSize)i;
                priorityList = true;
            }
        }

        //Place the size with only one left at the end
        for (int i = 0; i < frequencyGroup[listIndex].list.Count; i++)
        {
            if (frequencyGroup[listIndex].list[i].size == sizeWithOneLeft)  
            {
                Condition temp = frequencyGroup[listIndex].list[i];
                frequencyGroup[listIndex].list.RemoveAt(i);
                frequencyGroup[listIndex].list.Add(temp);
            }
        }

        //Put any lists that have a size with only one remaining at the front (priority)
        if(priorityList)
        {
            ConditionList temp = frequencyGroup[listIndex];
            frequencyGroup.RemoveAt(listIndex);
            frequencyGroup.Insert(0, temp);
        }
    }
}
