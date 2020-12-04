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
    

    [Header("Валюты------------------------------------------")]
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
    public List<Item> shopItems = new List<Item>();
    public Text[] shopItemsText;
    public Button[] shopButts;

    public double autoBonus;
    public int[] shopPrice;
    public double[] shopBonus;
    public int[] buyProgress;
    public Text[] buyProgressText;
    public int[] ultimateGoalProgress;
    private bool firstBuying;
    private double clickBonus;

    private Save sv = new Save();

    private void Start()
    {
        StartCoroutine(bonusPerSecond());
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
            score = sv.score;
            for (int i = 0; i < countButt; i++)
            {
                buyProgress[i] = sv.buyProgress[i];
                autoBonus += buyProgress[i] * shopBonus[i];
                buyProgressText[i].text = buyProgress[i].ToString();
            }
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
                //bonus += shopBonus[index]; бонус до кликов за покупку цвета
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
}