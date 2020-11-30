
using UnityEngine;
using UnityEngine.UI;



public class Game : MonoBehaviour
{
    public Text scoreText;
    public int score;
    private int bonus = 1;
    [Header("Shop")]
    public int[] shopCosts;
    public int[] shopBonuses;
    public Text[] shopBttnsText;
    public GameObject shopPan;
    public GameObject Settings;


    private void  Update ()
    {
        scoreText.text = score + "$";
    }

    public void shopPan_ShowAndHIde ()
    {
        shopPan.SetActive(!shopPan.activeSelf);
    }
    public void Settings_ShowAndHIde()
    {
        Settings .SetActive(!Settings.activeSelf);
    }
    public void shopBttn_addBonus(int index)
    {
        if  (score >= shopCosts[index])
        {
            bonus += shopBonuses[index];
            score -= shopBonuses[index];
            shopCosts[index] *= 2;
            shopBttnsText[index].text = "Buy upgrade" + shopCosts[index] + "$";
        }
        else
        {
            Debug.Log("No money");
        }
     
    }
   
    public void OnClick()
    {
        score += bonus;
        
    }
}