using Cyberultimate.Unity;
using System.Collections.Generic;
using UnityEngine;
namespace LetterBattle
{
	public class CameraHelper : MonoSingleton<CameraHelper>
	{
		[SerializeField] private Camera mainCamera;

		private Vector2 basePos;
		private CyberCoroutine shaking = null;

		public Vector2 CameraSize { get; private set; }

		public const float CamOrthographicSize = 5f;
		public Camera MainCamera => mainCamera;
		protected override void Awake()
		{
			base.Awake();
			basePos = mainCamera.transform.Get2DPos();
			float h = 2 * CamOrthographicSize;
			float w = h * mainCamera.aspect;
			CameraSize = new Vector2(w, h);
		}

		public CyberCoroutine ShakeScreen(int shakingAmount = 10, float power = 0.05f, float delay = 0.01f)
		{
			shaking?.Stop();
			return shaking = CorController.Base.Start(ShakingScreen(shakingAmount, power, delay), this);
		}
		private IEnumerator<IWaitable> ShakingScreen(int shakingAmount = 10, float power = 0.05f, float delay = 0.01f)
		{
			float z = mainCamera.transform.position.z;
			for (int x = 0; x < shakingAmount; x++)
			{
				Vector2 nwPos = basePos + power * Randomer.Base.NextDirection();

				if (mainCamera == null)
					yield break;

				mainCamera.transform.position = new Vector3(nwPos.x, nwPos.y, z);

				yield return Yield.Wait(delay, ignoreTimeScale: true);
			}
			mainCamera.transform.position = new Vector3(basePos.x, basePos.y, z);

		}
	}
}