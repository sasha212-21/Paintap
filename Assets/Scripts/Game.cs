using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [Header("Валюты")]
    private int score;
    public Text scoreText;

    //private int gems;
    //public Text gemsText;
    //private int coins;
    //public Text coinsText;

    [Header("Магазин улучшений")]
    //public GameObject shopPan;
    private int bonus;
    public int[] shopCost;
    public int[] shopBonus;
    public Text[] shopButtText;

    //private int[] buyProgress;
    //public int[] ultimateGoalProgress;
    //public Text[] buyProgressText;
    //public Text[] ultimateGoalProgressText;

    //[Header("Магазин роботников")]
    //public GameObject shopPanWorker;


    private void Update()
    {
        scoreText.text = score;
    }

    public void OnClick()
    {
        score += bonus + 1;
    }


    public void shopButt_AddBonus(int index)
    {
        if (score >= shopCost[index])
        {
            bonus += shopBonus[index];
            score -= shopCost[index];
            shopCost[index] *= 2;
            shopButtText[index].text = shopCost[index];
        }
        else
        {
            {
                Debug.Log("Вам не хватает ляпов!" + " У вас " + score + ", а улучшение стоит " + shopCost[index]);
            }
        }
    }


    //Код для открытия и зак. панели магазина
    /*public void shopPan_ShowAndHide()
    {
        shopPan.SetActive(!shopPan.activeSelf);
    }*/


}