using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CCellCheck : MonoBehaviour, IPointerClickHandler
{
    private static Vector3 startPosition;
    private static int cellCounter = 0;
    private int cellNum;
    private int cellValue;
    private ApplicationManager appManager;
    private RectTransform cellRT;
    private Text txt;
    private GameObject[] hints;
    private CCellData cellData;
    private bool[] freeNums;

    private void ViewHints()
    {
        int i;

        cellData.CheckFree();
        
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

    public void Check1()
    {
        int n = cellData.Check1();
        if (n != 0) SetValue(n);
    }

    void Start()
    {
        int i, x, y;

        appManager = FindObjectOfType<ApplicationManager>();
        if (appManager == null) Debug.Log("ApplicationManager not found!");
        appManager.refresh += ViewHints;
        appManager.check1 += Check1;
        
        cellRT = GetComponent<RectTransform>();
        txt = GetComponent<Text>();
        cellNum = cellCounter++;
        if (cellNum == 0)
        {
            startPosition = cellRT.localPosition;
            x = y = 0;
        }
        else
        {
            float step = appManager.GetStep();
            x = cellNum % 9;
            y = cellNum / 9;
            Vector3 pos = new Vector3(step * x, -step * y, 0);
            cellRT.localPosition = startPosition + pos;
        }

        cellData = new CCellData(cellNum);

        freeNums = cellData.GetFreeNums();

        appManager.RegistryCell(this, cellNum);

        hints = new GameObject[9];
        for (i = 0; i < 9; i++)
        {
            hints[i] = transform.GetChild(i).gameObject;
        }
        SetValue(0);
    }

    public bool SetValue(int _num)
    {
        if (!freeNums[_num]) return false;
        cellValue = _num;
        cellData.SetValue(_num);
        if (_num != 0) txt.text = "" + _num;
        else txt.text = " ";
        return true;
    }

    public bool IsFreeNum(int _num)
    {
        return freeNums[_num];
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        appManager.SetPointerPosition(cellNum);
    }
}
