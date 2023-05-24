using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayCanvas : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI SubTitle;

    public void UpdateTitleAndSubtitle()
    {
        int currentLevel = GameManager.Instance.CurrentGameLevel;
        int needCount = GameManager.Instance.SnowSled.NeedCount;
        Title.text = $"Level : {currentLevel}";
        SubTitle.text = $"Find <b>{needCount}</b> Reindeers!";
    }
}
