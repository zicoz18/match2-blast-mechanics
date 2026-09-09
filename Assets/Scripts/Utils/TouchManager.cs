using Game.Core.BoardBase;
using UnityEngine;

namespace Utils
{
	public class TouchManager : MonoBehaviour
	{
		private const string CellCollider = "CellCollider";

		// A browser fires synthetic mouse events a few hundred milliseconds after a touch
		// ends, for compatibility with pages that only listen for mouse. Touch handling
		// must stay latched for longer than that gap or every tap is delivered twice.
		private const float MouseSuppressSecondsAfterTouch = 0.5f;

		[SerializeField] private Camera Camera;
		[SerializeField] private Board Board;

		private float _lastTouchTime = float.NegativeInfinity;

		private void Awake()
		{
			// Stops Unity adding its own simulated mouse events on top of the browser's.
			Input.simulateMouseWithTouches = false;
		}

		private void Update()
		{
			// Runtime detection rather than a compile-time branch: one build serves the
			// editor, desktop browsers (mouse) and mobile browsers (touch).
			if (Input.touchCount > 0)
			{
				_lastTouchTime = Time.unscaledTime;
				HandleTouchInput();
				return;
			}

			// touchCount is already 0 by the time the synthetic mouse event lands, so a
			// plain else-branch is not enough to suppress it.
			if (Time.unscaledTime - _lastTouchTime > MouseSuppressSecondsAfterTouch)
			{
				HandleMouseInput();
			}
		}

		private void HandleTouchInput()
		{
			var touch = Input.GetTouch(0);
			switch (touch.phase)
			{
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					ExecuteTouch(touch.position);
					break;
			}
		}

		private void HandleMouseInput()
		{
			if (Input.GetMouseButtonUp(0))
			{
				ExecuteTouch(Input.mousePosition);
			}
		}

		private void ExecuteTouch(Vector3 pos)
		{
			var hit = Physics2D.OverlapPoint(Camera.ScreenToWorldPoint(pos)) as BoxCollider2D;

			if (hit != null && hit.CompareTag(CellCollider))
			{
				Board.CellTapped(hit.gameObject.GetComponent<Cell>());
			}
		}
	}
}
