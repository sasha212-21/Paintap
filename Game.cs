using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Game : MonoBehaviour
{
    [Header("Счет-------------------------------------------")]
    private double score;
    public Text scoreText;
    public Text scoreEarningPerSecText;
    

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
    public const int countButt = 1;
    public Text[] shopPriceText;
    //public Button[] shopButts;
    [Space]
    private double autoBonusPerSec;
    public int[] shopPrice;
    public double[] shopBonus;
    public int[] level_autoBonus;
    public Text[] level_autoBonusText;
    public int[] maxLevel_autoBonus;

    [Header("Бонус за клики")]
    public double price_clickBonus;
    public double improvCoef_clickBonus;
    private double clickBonus;
    [Space]
    public int level_clickBonus;
    public Text level_clickBonusT;
    [Space]
    public int maxLevel_clickBonus;
    //public Text ultimateGoal_clickBonusT; при переходе на следующий уровень рисунка должно увеличиться и макс уровень улучшения

    [Header("Бонус за время вне игры")]
    public int maxTime_exitBonus;
    public double price_exitBonus;
    public int improvCoef_exitBonus;
    public int level_exitBonus;
    public Text level_exitBonusT;
    public int maxLevel_exitBonus;
    //public Text ultimateGoal_exitBonusT; при переходе на следующий уровень рисунка должно увеличиться и макс уровень улучшения


    private Save sv = new Save();

    private void Start()
    {
        StartCoroutine(bonusPerSecond());
    }
    
    private void OnClick_ImprovClickBonus()
    {
        if(score >= price_clickBonus)
        {
            if(level_clickBonus != maxLevel_clickBonus)
            {
                score -= price_clickBonus;
                clickBonus += improvCoef_clickBonus;
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

    private void OnClick_ImprovExitBonus()
    {
        if(score >= price_exitBonus)
        {
            if(level_exitBonus != maxLevel_exitBonus)
            {
                score -= price_exitBonus;
                maxTime_exitBonus += improvCoef_exitBonus;
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
        scoreEarningPerSecText.text = (autoBonusPerSec * 2 * boostBonus).ToString() + "  per/sec";
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
                level_autoBonus[i] = sv.level_autoBonus[i];
                totalBonus += shopBonus[i]; 
                autoBonusPerSec += level_autoBonus[i] * shopBonus[i];
                level_autoBonusText[i].text = level_autoBonus[i].ToString();
            }
            score = sv.score;
            
            print("Вы отсутствували " + ts.TotalHours + ". ");

            if(maxTime_exitBonus > ts.TotalHours){
                score += totalBonus * ts.TotalSeconds;
                print("В момент вашего отсутствия было накоплено " + totalBonus * maxTime_exitBonus);
            }
            else{
                score += totalBonus * maxTime_exitBonus;
                print("Максимальное время автодобычи " + maxTime_exitBonus);
                if(level_exitBonus != maxLevel_exitBonus)
                    print("Вы можете улучшить этот показатель." + "В момент вашего отсутствия было накоплено " + totalBonus * maxTime_exitBonus);
            }
        }
    }
    
    private void OnApplicationQuit()
    {
        sv.score = score;
        sv.level_autoBonus = new int[countButt];
        for (int i = 0; i < countButt; i++)
        {
            sv.level_autoBonus[i] = level_autoBonus[i];
        }
        sv.date[0] = DateTime.Now.Year; sv.date[1] = DateTime.Now.Month; sv.date[2] = DateTime.Now.Day; sv.date[3] = DateTime.Now.Hour; sv.date[4] = DateTime.Now.Minute; sv.date[5] = DateTime.Now.Second;
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(sv));
    }

    public void OnClick()
    {
        score += clickBonus + 1;
    }

    public void OnClick_ImprovAutoBonus(int index)
    {
        if (score >= shopPrice[index])
        {
            if (level_autoBonus[index] < maxLevel_autoBonus[index])
            {
                firstBuying = true;
                score -= shopPrice[index];

                if (level_autoBonus[index] != maxLevel_autoBonus[index] - 1)
                {
                    shopPrice[index] *= 2;
                    shopPriceText[index].text = shopPrice[index].ToString();
                }
                else
                    shopPriceText[index].text = "Soldout!";

                level_autoBonus[index]++;
                level_autoBonusText[index].text = level_autoBonus[index].ToString();
                autoBonusPerSec += shopBonus[index];
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

    public void OnClick_Boost(int index)
    {
        for(int i = 0; i < countButt; i++){
            if(level_autoBonus[i])
        }
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
            score += autoBonusPerSec * boostBonus;
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
    public int[] level_autoBonus;
    public int[] date = new int[6];
}