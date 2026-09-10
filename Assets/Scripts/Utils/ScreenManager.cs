using UnityEngine;

namespace Utils
{
	public class ScreenManager : MonoBehaviour
	{
		// World units the camera must keep visible on both axes, so the 9x9 board and
		// its border stay fully on screen at any aspect ratio.
		private const float VisibleUnits = 10f;

		// Fraction of the screen height the board camera is allowed to draw into. The
		// remainder at the top belongs to the HUD, which is why the board can never
		// overlap it: the camera physically cannot render there.
		private const float BoardViewportHeight = 0.84f;

		private Camera _camera;
		private float _lastAspect;

		private void Awake()
		{
			_camera = GetComponent<Camera>();
			_camera.rect = new Rect(0f, 0f, 1f, BoardViewportHeight);
			PrepareCamera();
		}

		private void Update()
		{
			// A browser canvas can be resized at any moment, so aspect is not fixed after
			// Awake the way it is in a fixed-resolution player.
			if (!Mathf.Approximately(_camera.aspect, _lastAspect))
			{
				PrepareCamera();
			}
		}

		private void PrepareCamera()
		{
			// Camera.aspect already accounts for the viewport rect, so this is the aspect
			// of the board's slice of the screen rather than of the whole window.
			_lastAspect = _camera.aspect;

			// Taking the larger of the two requirements letterboxes rather than crops:
			// satisfying only the width constraint cropped the board vertically on any
			// landscape aspect.
			var sizeToFitHeight = VisibleUnits / 2f;
			var sizeToFitWidth = (VisibleUnits / _camera.aspect) / 2f;

			_camera.orthographicSize = Mathf.Max(sizeToFitHeight, sizeToFitWidth);
		}
	}
}
