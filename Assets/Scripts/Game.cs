using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    [Header("Счет-------------------------------------------")]
    public const int countButt = 1;
    private double score;
    public Text scoreText;
    public Text scoreEarningText;
    

    //[Header("Валюты------------------------------------------")]
    /*
    public Text gemsText;
    private int gems;
    public Text coinsText;
    private int coins;
    */

    [Header("Платные бонусы----------------------------------")]
    public double boostBonus = 1;
    public Button[] boostButt;
    public float[] boostTime;
    public double[] boostPrice;

    [Header("Магазин улучшений-------------------------------")]
    public Text[] shopItemsText;
    public Button[] shopButts;

    public double autoBonus;
    public int[] shopPrice;
    public double[] shopBonus;
    public int[] buyProgress;
    public Text[] buyProgressText;
    public int[] ultimateGoalProgress;
    private bool firstBuying;

    [Header("Бонус за клики")]
    public double price_clickBonus;
    public double upCount_clickBonus;
    private double clickBonus;
    [Space]
    public int level_clickBonus;
    public Text level_clickBonusT;
    [Space]
    public int ultimateGoal_clickBonus;
    //public Text ultimateGoal_clickBonusT; при переходе на следующий уровень рисунка должно увеличиться и макс уровень улучшения

    [Header("Бонус за время вне игры")]
    public int maxTime_exitBonus;
    public double price_exitBonus;
    public int upCount_exitBonus;
    public int level_exitBonus;
    public Text level_exitBonusT;
    public int ultimateGoal_exitBonus;
    //public Text ultimateGoal_exitBonusT; при переходе на следующий уровень рисунка должно увеличиться и макс уровень улучшения


    private Save sv = new Save();

    private void Start()
    {
        StartCoroutine(bonusPerSecond());
    }
    
    private void addClickBonus()
    {
        if(score >= price_clickBonus)
        {
            if(level_clickBonus != ultimateGoal_clickBonus)
            {
                score -= price_clickBonus;
                clickBonus += upCount_clickBonus;
                level_clickBonus++;
                level_clickBonusT.text = level_clickBonus.ToString();
                //поменять бонус к бонусклику и цену
            }
            else
                Debug.Log("Покачто вы не можете улучшить ваш бонусклик! Перейдите на следующий уровень");
        }
        else
            Debug.Log("У вас не хватает валюты!");
    }

    private void addAutoBonus_ExitDame()
    {
        if(score >= price_exitBonus)
        {
            if(level_exitBonus != ultimateGoal_exitBonus)
            {
                score -= price_exitBonus;
                maxTime_exitBonus += upCount_exitBonus;
                level_exitBonus++;
                level_exitBonusT.text = level_exitBonus.ToString();
                //поменять бонус к exitбонус и цену
            }
            else
                Debug.Log("Покачто вы не можете улучшить ваш бонусклик! Перейдите на следующий уровень");
        }
        else
            Debug.Log("У вас не хватает валюты!");
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        scoreEarningText.text = (autoBonus * 2 * boostBonus).ToString() + "  per/sec";
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("SV"))
        { 
            sv = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
            double totalBonus = 0;

            DateTime dt = new DateTime(sv.date[0], sv.date[1], sv.date[2], sv.date[3], sv.date[4], sv.date[5]);
            TimeSpan ts = DateTime.Now - dt;

            for (int i = 0; i < countButt; i++)
            {
                buyProgress[i] = sv.buyProgress[i];
                totalBonus += shopBonus[i]; 
                autoBonus += buyProgress[i] * shopBonus[i];
                buyProgressText[i].text = buyProgress[i].ToString();
            }
            score = sv.score;
            score += totalBonus * ts.TotalSeconds;
        }
    }
    
    private void OnApplicationQuit()
    {
        sv.score = score;
        sv.firstBuying = firstBuying;
        sv.buyProgress = new int[countButt];
        for (int i = 0; i < countButt; i++)
        {
            sv.buyProgress[i] = buyProgress[i];
        }
        sv.date[0] = DateTime.Now.Year; sv.date[1] = DateTime.Now.Month; sv.date[2] = DateTime.Now.Day; sv.date[3] = DateTime.Now.Hour; sv.date[4] = DateTime.Now.Minute; sv.date[5] = DateTime.Now.Second;
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(sv));
    }

    public void OnClick()
    {
        score += clickBonus + 1;
    }

    public void shopButt_AddBonus(int index)
    {
        if (score >= shopPrice[index])
        {
            if (buyProgress[index] < ultimateGoalProgress[index])
            {
                firstBuying = true;
                score -= shopPrice[index];

                if (buyProgress[index] != ultimateGoalProgress[index] - 1)
                {
                    shopPrice[index] *= 2;
                    shopItemsText[index].text = shopPrice[index].ToString();
                }
                else
                    shopItemsText[index].text = "Soldout!";

                buyProgress[index]++;
                buyProgressText[index].text = buyProgress[index].ToString();
                autoBonus += shopBonus[index];
            }
            else
            {
                Debug.Log("Этот цвет больше не доступен!");
            }
        }
        else
        {
            Debug.Log("Вам не хватает ляпов!" + " У вас " + score + ", а улучшение стоит " + shopPrice[index]);
        }
    }

    public void OnClickBoost(int index)
    {
        if (firstBuying)
        {
            if (boostPrice[index] <= score)
            {
                score -= boostPrice[index];
                StartCoroutine(boostAutoBonus(boostTime[index], index));
            }
            else
                Debug.Log("У вас не хватает валюты(");
        }
        else
            Debug.Log("Вы еще не купили ни один цвет, улучшение бесполезно(");
    }

    IEnumerator bonusPerSecond()
    {
        while (true)
        {
            score += autoBonus * boostBonus;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator boostAutoBonus(float time, int index)
    {
        boostButt[index].interactable = false;
        boostBonus *= 2;
        yield return new WaitForSeconds(time);
        boostBonus /= 2;
        boostButt[index].interactable = true;
    }
}

[Serializable]
public class Save
{
    public double score;
    public int[] buyProgress;
    public bool firstBuying;
    public int[] date = new int[6];
}