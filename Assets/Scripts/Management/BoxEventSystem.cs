using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This handles the geology events.
/// </summary>
public class BoxEventSystem : Singleton<BoxEventSystem>
{
    #region delegates and events
    public delegate void RockSelected();
    public static event RockSelected OnRockSelected;
    public delegate void AcidTestStart();
    public static event AcidTestStart OnAcidTestStart;
    public delegate void AcidTestEnd();
    public static event AcidTestEnd OnAcidTestEnd;
    public delegate void LensTestStart();
    public static event LensTestStart OnLensTestStart;
    public delegate void LensTestEnd();
    public static event LensTestEnd OnLensTestEnd;
    public delegate void AllTestsEnd();
    public static event AllTestsEnd OnAllTestsEnd;
    public delegate void PostQuestionsStart();
    public static event PostQuestionsStart OnPostQuestionsStart;
    public delegate void PostQuestionsEnd();
    public static event PostQuestionsEnd OnPostQuestionsEnd;
    public delegate void RockEnd();
    public static event RockEnd OnRockEnd;
    public delegate void AllRocksComplete();
    public static event AllRocksComplete OnAllRocksComplete;
    public delegate void SpatialTestStart();
    public static event SpatialTestStart OnSpatialTestStart;
    public delegate void SpatialTestEnd();
    public static event SpatialTestEnd OnSpatialTestEnd;
    #endregion

    [HideInInspector]
    public bool lensTestActive = false;

    public void RaiseRockSelected()
    {
        OnRockSelected?.Invoke();
    }

    public void RaiseAcidTestStart()
    {
        OnAcidTestStart?.Invoke();
    }

    public void RaiseAcidTestEnd()
    {
        //LevelProgress.Instance.currentRock.acidTestComplete = true;
        OnAcidTestEnd?.Invoke();
        /*if(LevelProgress.Instance.CheckIfCurrentRockTestsComplete())
        {
            RaiseAllTestsEnd();
        }*/
    }

    public void RaiseLensTestStart()
    {
        OnLensTestStart?.Invoke();
        lensTestActive = true;
    }

    public void RaiseLensTestEnd()
    {
        OnLensTestEnd?.Invoke();
        lensTestActive = false;
        /*if (LevelProgress.Instance.CheckIfCurrentRockTestsComplete())
        {
            RaiseAllTestsEnd();
        }*/
    }

    public void RaiseAllTestsEnd()
    {
        OnAllTestsEnd?.Invoke();
        /*if (!LevelProgress.Instance.CheckIfQuestionsNeeded())
        {
            RaiseRockEnd();
        }
        else
        {
            RaisePostQuestionsStart();
        }*/
    }

    public void RaisePostQuestionsStart()
    {
        OnPostQuestionsStart?.Invoke();
    }

    public void RaisePostQuestionsEnd()
    {
        OnPostQuestionsEnd?.Invoke();
        RaiseRockEnd();
    }

    public void RaiseRockEnd()
    {
        OnRockEnd?.Invoke();
        /*if(LevelProgress.Instance.CheckIfAllRocksComplete())
        {
            RaiseAllRocksComplete();
        }*/
    }

    public void RaiseAllRocksComplete()
    {
        OnAllRocksComplete?.Invoke();
    }

    public void RaiseSpatialTestStart()
    {
        OnSpatialTestStart?.Invoke();
    }

    public void RaiseSpatialTestEnd()
    {
        OnSpatialTestEnd?.Invoke();
    }
}
