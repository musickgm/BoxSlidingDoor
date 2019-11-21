using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreBoard : Singleton<ScoreBoard>
{
    [System.Serializable]
    public struct ScoreCell
    {
        public TextMeshProUGUI cellText;
        public Image bgImage;
        public bool recorded;
    }

    [System.Serializable]
    public class ScoreSet
    {
        public List<ScoreCell> set = new List<ScoreCell>();
    }

    public List<ScoreSet> scoreboardSets = new List<ScoreSet>();
    public Color bgColorSuccess;
    public Color bgColorFail;
    public Material altMaterial;
    public TextMeshProUGUI scoreText;
    private int score;


    public void UpdateCell(bool attemptSuccess, bool basketSuccess, int setNumber, int trialNumber)
    {
        if(scoreboardSets.Count == 0)
        {
            return;
        }
        ScoreCell currentCell = scoreboardSets[setNumber].set[trialNumber];
        /*if(scoreboardSets[setNumber].set[trialNumber].recorded)
        {
            return;
        }
        scoreboardSets[setNumber].set[trialNumber].recorded = true;*/
        if(!attemptSuccess)
        {
            currentCell.bgImage.color = bgColorFail;
            currentCell.cellText.text = "";
            UpdateScore(-2);
        }
        else
        {
            currentCell.bgImage.color = bgColorSuccess;
            currentCell.cellText.text = "X";
            if(basketSuccess)
            {
                UpdateScore(2);
                currentCell.cellText.fontSharedMaterial = altMaterial;
            }
            else
            {
                UpdateScore(1);
            }
        }
    }

    private void UpdateScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }
}
