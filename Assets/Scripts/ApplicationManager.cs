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
	private const float stepX = 0.54f;
	private const float stepY = -0.54f;

	private void Start()
    {
		pointerRT = pointer.GetComponent<RectTransform>();
		startPointerPosition = pointerRT.position;
    }

    private void SetPointerPosition()
    {
		float px = pointerX-1;
		float py = pointerY-1;
		Vector3 pos = new Vector3(px * stepX, py * stepY, 0);

		pointerRT.position = startPointerPosition + pos;
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
	public void pressNum(int _num)
    {
		Debug.Log(_num);
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
