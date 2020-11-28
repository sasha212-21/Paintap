using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private int score;
    public Text scoreText;

    [Header("Магазин улучшений")]
    public GameObject shopPan;
    private int bonus;
    public int[] shopCost;
    public int[] shopBonus;
    public Text[] shopButtText;

    [Header("Магазин роботников")]
    public GameObject shopPan;


    private void Update()
    {
        scoreText.text = score + "$";
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
            shopButtText[index].text = shopCost[index] + "$";
        }
        else
        {
            {
                Debug.Log("Вам не хватает денег!" + " У вас " + score + ", а улучшение стоит " + shopCost[index]);
            }
        }
    }



    public void shopPan_ShowAndHide()
    {
        shopPan.SetActive(!shopPan.activeSelf);
    }



}