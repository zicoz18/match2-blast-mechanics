using Game.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
	public class ResultPanel : MonoBehaviour
	{
		private const string CompletedMessage = "Level Complete!";
		private const string FailedMessage = "Out of Moves";

		[SerializeField] private GameObject root;
		[SerializeField] private TextMeshProUGUI messageText;
		[SerializeField] private Button continueButton;
		[SerializeField] private Button retryButton;

		private void Awake()
		{
			root.SetActive(false);
			continueButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.Main));

			retryButton.onClick.AddListener(() => SceneManager.LoadScene(SceneNames.Level));
		}

		public void Show(LevelPlayState state)
		{
			bool completed = state == LevelPlayState.Completed;

			messageText.text = completed ? CompletedMessage : FailedMessage;
			continueButton.gameObject.SetActive(completed);
			retryButton.gameObject.SetActive(!completed);

			root.SetActive(true);
		}
	}
}
