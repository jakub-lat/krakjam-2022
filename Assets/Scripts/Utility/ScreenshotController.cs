using Cyberultimate.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotController : MonoSingleton<ScreenshotController>
{
    public void TakeScreenshot()
    {
		string time = System.DateTime.UtcNow.ToLocalTime().ToString("HH-mm-ss");
		string target = "Screenshots";
		if (!Directory.Exists(target))
		{
			Directory.CreateDirectory(target);
		}

		time = $"{Directory.GetCurrentDirectory()}/Screenshots/Screenshot{time}.png";
		ScreenCapture.CaptureScreenshot(time, 2);
	}

	public void OnScreenshot()
    {
		TakeScreenshot();
    }
}
