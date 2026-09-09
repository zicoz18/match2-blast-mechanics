using UnityEngine;

namespace Utils
{
	public class ScreenManager : MonoBehaviour
	{
		// World units the camera must keep visible on both axes, so the 9x9 board and
		// its border stay fully on screen at any aspect ratio.
		private const float VisibleUnits = 10f;

		private Camera _camera;
		private float _lastAspect;

		private void Awake()
		{
			_camera = GetComponent<Camera>();
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
			_lastAspect = _camera.aspect;

			// Taking the larger of the two requirements letterboxes rather than crops:
			// the previous version only satisfied the width constraint, which cropped the
			// board vertically on any landscape aspect.
			var sizeToFitHeight = VisibleUnits / 2f;
			var sizeToFitWidth = (VisibleUnits / _camera.aspect) / 2f;

			_camera.orthographicSize = Mathf.Max(sizeToFitHeight, sizeToFitWidth);
		}
	}
}
