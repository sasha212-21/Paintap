using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
    
    [Header("Валюты------------------------------------------")]
    public Text scoreText;
    public Text scoreEarningText;
    private double score;
    
    /*private int gems;
    public Text gemsText;
    private int coins;
    public Text coinsText;*/

    [Header("Платные бонусы----------------------------------")]
    public Button[] boostButt;
    public float[] boostTime;
    public double boostBonus = 1;
    public double[] boostPrice;

    [Header("Магазин улучшений-------------------------------")]
    public double autoBonus;
    private bool firstBuying;
    private double clickBonus;
    public int[] shopPrice;
    public double[] shopBonus;
    public Text[] shopButtText;
    public int[] buyProgress;
    public int[] ultimateGoalProgress;
    public Text[] buyProgressText;

    private void Start()
    {
        StartCoroutine(bonusPerSecond());
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        scoreEarningText.text = (autoBonus * 2 * boostBonus).ToString() + "  per/sec";
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
                    shopButtText[index].text = shopPrice[index].ToString();
                }
                else
                    shopButtText[index].text = "Soldout!";

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