using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapscroling : MonoBehaviour
{
    [Range(1, 50)]
    [Header("Controllers")]
    public int panCount;
    [Rande(0, 500)]
    public int panOffset;
    [Header("Other Objects")]
    public GameObject panPrefab;

    private GameObject[] instPans
    void Start()
    {
        
    }

