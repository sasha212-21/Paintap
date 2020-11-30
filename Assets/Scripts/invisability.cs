using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisability : MonoBehaviour
{
    public GameObject shopPan;

    public void shopPan_HideandShow()
    {
        shopPan.SetActive(!shopPan.activeSelf);
    }
}
