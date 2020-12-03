using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisability : MonoBehaviour
{
    public GameObject shopPan;
    public GameObject setings;
    public GameObject Palettes;
    public GameObject Scrolshop;

    public void shopPan_HideandShow()
    {
        shopPan.SetActive(!shopPan.activeSelf);
    }

    public void setings_HideandShow()
    {
        setings.SetActive(!setings.activeSelf);
    }

    public void Palettes_HideandShow()
    {
        Palettes.SetActive(!Palettes.activeSelf);
    }

    public void Scrolshop_HideandShow()
    {
        Scrolshop.SetActive(!Scrolshop.activeSelf);
    }
}
