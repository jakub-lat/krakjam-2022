using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LetterboxManager : MonoBehaviour
{
	[SerializeField]
	private Color letterBoxColor = new Color(0, 0, 0, 1);

	[SerializeField]
	[Range(1, 100)]
	private float aspectRatioX = 16;

	[SerializeField]
	[Range(1, 100)]
	private float aspectRatioY = 9;

	private Camera cam = null;
	private Camera letterboxCam = null;

	[SerializeField]
	[Tooltip("Make sure that any of your camera is not using this depth!")]
	private int minimalPossibleDepth = -100;

	private const float FULL_HEIGHT = 1;
	

	protected void Awake()
	{
		cam = GetComponent<Camera>();

		AddLetterboxCamera();

		RefreshSize();
	}

	private void AddLetterboxCamera()
	{
		letterboxCam = new GameObject().AddComponent<Camera>();
		letterboxCam.backgroundColor = letterBoxColor;
		letterboxCam.cullingMask = 0;
		letterboxCam.depth = minimalPossibleDepth;
		letterboxCam.useOcclusionCulling = false;
		letterboxCam.allowHDR = false;
		letterboxCam.allowMSAA = false;
		letterboxCam.clearFlags = CameraClearFlags.Color;
		letterboxCam.name = "Letter Box Camera";
	}

	public void RefreshSize()
	{
		float chosenRatio = (aspectRatioX / aspectRatioY);
		float windowRatio = (float)Screen.width / (float)Screen.height;
		float scaleHeight = windowRatio / chosenRatio;
		Rect rect = cam.rect;

		if (scaleHeight < FULL_HEIGHT)
		{
			rect.width = FULL_HEIGHT;
			rect.height = scaleHeight;
			rect.x = 0;
			rect.y = (FULL_HEIGHT - scaleHeight) / 2;
		}

		else
		{
			float scaleWidth = FULL_HEIGHT / scaleHeight;

			rect.width = scaleWidth;
			rect.height = FULL_HEIGHT;
			rect.x = (FULL_HEIGHT - scaleWidth) / 2;
			rect.y = 0;
		}

		cam.rect = rect;
	}
}
