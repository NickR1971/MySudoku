using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCellCheck : MonoBehaviour
{
    private RectTransform cellRT;
    private GameObject[] hints;
    private bool[] freeNums;
    private static int cellCounter = 0;
    private int cellNum;
    private ApplicationManager appManager;
    private static Vector3 startPosition;

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
     GameObject am;
     
        am = GameObject.Find("ApplicationManager");
        if (am == null) Debug.Log("ApplicationManager not found!");
        appManager=am.GetComponent<ApplicationManager>();
        cellRT = GetComponent<RectTransform>();
        cellNum = cellCounter++;
        if (cellNum == 0) startPosition = cellRT.position;
        else
        {
            float step = appManager.GetStep();
            int x = cellNum % 9;
            int y = cellNum / 9;
            Vector3 pos = new Vector3(step*x, -step*y, 0);
            cellRT.position = startPosition + pos;
        }

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
        if (appManager.GetChangedCell() == cellNum)
        {
            Debug.Log("Enter cell: " + cellNum);
            appManager.ResetChangeCell();
        }
    }
}
