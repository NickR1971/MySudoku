using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCellCheck : MonoBehaviour
{
    public Vector3 pos;
    private Vector3 startPosition;
    private RectTransform cellRT;
    private GameObject[] hints;
    private bool[] freeNums;
    private static int cellCounter = 0;
    private int cellNum;

    private void ViewHints()
    {
     int i;

        for (i = 0; i < 9; i++)
        {
            hints[i].SetActive(freeNums[i+1]);
        }
    }

    void Start()
    {
     int i;

        cellNum = cellCounter++;
        cellRT = GetComponent<RectTransform>();
        pos = startPosition = cellRT.position;

        freeNums = new bool[10];
        for (i = 0; i < 10; i++) freeNums[i] = false;

        hints = new GameObject[9];
        for (i = 0; i < 9; i++)
        {
            hints[i] = transform.GetChild(i).gameObject;
        }
        ViewHints();
    }

    void Update()
    {
        
    }
}
