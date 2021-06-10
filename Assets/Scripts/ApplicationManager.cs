using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour
{
	[SerializeField] private Image pointer;
	private Vector3 startPointerPosition;
	private RectTransform pointerRT;
	private int pointerX = 1;
	private int pointerY = 1;
    private int activeCellNum;
    private const float step = 42.5f;
    private int[] map;
	private CCellCheck[] cellList;
	private int refreshAction;


    private void Awake()
    {
     int i;

		cellList = new CCellCheck[81];
        map = new int[81];
        for (i = 0; i < 81; i++) map[i] = 0;
        activeCellNum = 0;
		refreshAction = 0;
    }
    /// /////////////////////////////
    /// Start initialization
	private void Start()
    {
		pointerRT = pointer.GetComponent<RectTransform>();
		startPointerPosition = pointerRT.position;
    }

	public void RegistryCell(CCellCheck _cell, int _index)
    {
		cellList[_index] = _cell;
    }

    public float GetStep() { return step; }

    public int GetMapValue(int _index) { return map[_index]; }

	public int GetRefresh() { return refreshAction; }
	public void DoneRefresh() { refreshAction--; }

    private void SetPointerPosition()
    {
		float px = pointerX-1;
		float py = pointerY-1;
		Vector3 pos = new Vector3(px * step, -py * step, 0);

		pointerRT.position = startPointerPosition + pos;
        activeCellNum = (pointerY - 1) * 9 + (pointerX - 1);
    }
	public void UpPointer()
    {
		if (pointerY == 1) return;
		pointerY--;
		SetPointerPosition();

	}
	public void DownPointer()
	{
		if (pointerY == 9) return;
		pointerY++;
		SetPointerPosition();
	}
	public void LeftPointer()
	{
		if (pointerX == 1) return;
		pointerX--;
		SetPointerPosition();
	}
	public void RightPointer()
	{
		if (pointerX == 9) return;
		pointerX++;
		SetPointerPosition();
	}

	public void PressNum(int _num)
    {
		//Debug.Log(_num);
		if (cellList[activeCellNum].SetValue(_num))
		{
			map[activeCellNum] = _num;
			refreshAction = 81;
		}
    }

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
