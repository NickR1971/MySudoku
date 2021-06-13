using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCellData
{
    private int value;
    private bool[] freeNums;
    private int[] row;
    private int[] column;
    private int[] square;

    private static CCellData[] cellList = new CCellData[81];

    public CCellData(int _cellNum)
    {
        int i, x, y;

        value = 0;
        cellList[_cellNum] = this;

        freeNums = new bool[10];
        for (i = 0; i < 10; i++) freeNums[i] = true;

        x = _cellNum % 9;
        y = _cellNum / 9;

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
    }

    public ref bool[] GetFreeNums() { return ref freeNums; }

    public ref int[] GetRow() { return ref row; }

    public ref int[] GetColumn() { return ref column; }

    public ref int[] GetSquare() { return ref square; }

    public void SetValue(int _value)
    {
        int i;

        value = _value;
        if (value == 0)
        {
            for (i = 1; i < 10; i++) freeNums[i] = true;
            return;
        }

        for (i = 1; i < 10; i++)
        {
            freeNums[i] = (i == _value);
        }
    }

    public void CheckFree()
    {
        int i;
        bool b;

        b = freeNums[0];

        for (i = 0; i < 9; i++)
        {
            freeNums[cellList[row[i]].value] = false;
            freeNums[cellList[column[i]].value] = false;
            freeNums[cellList[square[i]].value] = false;
        }

        freeNums[0] = b;
    }

    private int CheckGroup(int[] a)
    {
        int i, n, cnt, rt;

        rt = 0;
        for(n=1;n<10;n++)
        {
            if(freeNums[n])
            {
                cnt = 0;
                for (i = 0; i < 9; i++) if (cellList[a[i]].freeNums[n]) cnt++;
                if (cnt == 1) rt = n;
            }
            if (rt != 0) break;
        }

        return rt;
    }

    public int Check1()
    {
        int i, n;

        n = 0;
        if (value != 0) return n;

        for (i = 1; i < 10; i++)
        {
            if (freeNums[i]) n++;
        }
        
        if (n == 1)
        {
            for (i = 1; i < 10; i++)
            {
                if(freeNums[i])
                {
                    n = i;
                    break;
                }
            }
            return n;
        }

        n = CheckGroup(row);
        if (n != 0) return n;
        n = CheckGroup(column);
        if (n != 0) return n;
        n = CheckGroup(square);
        return n;
    }
}
