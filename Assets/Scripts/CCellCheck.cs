using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCellCheck : MonoBehaviour
{
    private static Vector3 startPosition;
    private static int cellCounter = 0;
    private int cellNum;
    private int cellValue;
    private ApplicationManager appManager;
    private RectTransform cellRT;
    private Text txt;
    private GameObject[] hints;
    private bool[] freeNums;
    private int[] row;
    private int[] column;
    private int[] square;

    private void ViewHints()
    {
     int i;

        for (i = 0; i < 9; i++)
        {
            freeNums[appManager.GetMapValue(row[i])] = false;
            freeNums[appManager.GetMapValue(column[i])] = false;
            freeNums[appManager.GetMapValue(square[i])] = false;
        }
        freeNums[0] = true;
        for (i = 0; i < 9; i++)
        {
            hints[i].SetActive(freeNums[i+1] && cellValue==0);
        }
    }

    public void ResetNums()
    {
     int i;

        for (i = 0; i < 10; i++) freeNums[i] = true;
    }

    void Start()
    {
     int i, x, y;
     GameObject am;
     
        am = GameObject.Find("ApplicationManager");
        if (am == null) Debug.Log("ApplicationManager not found!");
        appManager=am.GetComponent<ApplicationManager>();
        cellRT = GetComponent<RectTransform>();
        txt = GetComponent<Text>();
        cellNum = cellCounter++;
        if (cellNum == 0)
        {
            startPosition = cellRT.position;
            x = y = 0;
        }
        else
        {
            float step = appManager.GetStep();
            x = cellNum % 9;
            y = cellNum / 9;
            Vector3 pos = new Vector3(step * x, -step * y, 0);
            cellRT.position = startPosition + pos;
        }

        freeNums = new bool[10];
        for (i = 0; i < 10; i++) freeNums[i] = true;

        appManager.RegistryCell(this, cellNum);

        row = new int[9];
        column = new int[9];
        square = new int[9];

        for (i = 0; i < 9; i++)
        {
            row[i] = y * 9 + i;
            column[i] = x + i * 9;
        }

        x = x / 3; y = y / 3;
        square[0] = y * 27 + x * 3;
        square[1] = square[0] + 1;
        square[2] = square[1] + 1;
        square[3] = square[0] + 9;
        square[4] = square[1] + 9;
        square[5] = square[2] + 9;
        square[6] = square[3] + 9;
        square[7] = square[4] + 9;
        square[8] = square[5] + 9;

        hints = new GameObject[9];
        for (i = 0; i < 9; i++)
        {
            hints[i] = transform.GetChild(i).gameObject;
        }
        SetValue(0);
        ViewHints();
    }

    public bool SetValue(int _num)
    {
        if (!freeNums[_num]) return false;
        cellValue = _num;
        if (_num != 0) txt.text = "" + _num;
        else txt.text = " ";
        return true;
    }

    void Update()
    {
        if (appManager.GetRefresh() > 0)
        {
            ViewHints();
            appManager.DoneRefresh();
        }
    }
}
