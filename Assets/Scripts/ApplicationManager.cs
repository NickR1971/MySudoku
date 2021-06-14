using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class SaveData
{
	public int[] map = new int[81];
}

public class ApplicationManager : MonoBehaviour
{
	[SerializeField] private Image pointer;
	[SerializeField] private GameObject[] numButtons = new GameObject[9];
	private Vector3 startPointerPosition;
	private RectTransform pointerRT;
	private int pointerX = 1;
	private int pointerY = 1;
    private int activeCellNum;
    private const float step = 32.0f;
    private int[] map;
	private CCellCheck[] cellList;
	public event Action refresh;
	public event Action check1;


    private void Awake()
    {
     int i;

		cellList = new CCellCheck[81];
        map = new int[81];
        for (i = 0; i < 81; i++) map[i] = 0;
        activeCellNum = 0;
    }

	private void Start()
    {
		pointerRT = pointer.GetComponent<RectTransform>();
		startPointerPosition = pointerRT.localPosition;
    }

	public void RegistryCell(CCellCheck _cell, int _index)
    {
		cellList[_index] = _cell;
    }

	public CCellCheck GetCell(int _index) { return cellList[_index]; }

    public float GetStep() { return step; }

    public int GetMapValue(int _index) { return map[_index]; }

    private void SetPointerPosition()
    {
		int i;
		float px = pointerX-1;
		float py = pointerY-1;
		Vector3 pos = new Vector3(px * step, -py * step, 0);

		pointerRT.localPosition = startPointerPosition + pos;
        activeCellNum = (pointerY - 1) * 9 + (pointerX - 1);

		for (i = 0; i < 9; i++)
        {
			numButtons[i].GetComponent<Button>().interactable = cellList[activeCellNum].IsFreeNum(i + 1);
		}
    }
	
	public void SetPointerPosition(int _cell)
    {
		pointerX = _cell % 9;
		pointerY = _cell / 9;
		pointerX++;
		pointerY++;
		SetPointerPosition();
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
		if (cellList[activeCellNum].SetValue(_num))
		{
			map[activeCellNum] = _num;
			refresh?.Invoke();
		}
    }

	public void ResetHints()
    {
	 int i;

		for (i = 0; i < 81; i++)
        {
			cellList[i].ResetNums();
        }
    }

	public void Check1()
    {
		check1?.Invoke();
		refresh?.Invoke();
    }

	public void Save()
    {
	 int i;

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath
					 + "/MySaveData.dat");
		SaveData data = new SaveData();
		for (i = 0; i < 81; i++)
        {
			data.map[i] = map[i];
        }
		bf.Serialize(file, data);
		file.Close();
		Debug.Log("Save to " + Application.persistentDataPath + "/MySaveData.dat");
    }

	public void Load()
	{
	 int i;

		if (File.Exists(Application.persistentDataPath
					   + "/MySaveData.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =
					   File.Open(Application.persistentDataPath
					   + "/MySaveData.dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			for (i = 0; i < 81; i++)
            {
				if (cellList[i].SetValue(data.map[i]))
				{
					map[i] = data.map[i];
				}
			}
			file.Close();
			refresh?.Invoke();
			Debug.Log("Game data loaded!");
		}
		else
			Debug.LogError("There is no save data!");
	}

	public void ResetData()
    {
		if (File.Exists(Application.persistentDataPath
			  + "/MySaveData.dat"))
		{
			File.Delete(Application.persistentDataPath
							  + "/MySaveData.dat");
			Debug.Log("Data reset complete!");
		}
		else
			Debug.LogError("No save data to delete.");

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
