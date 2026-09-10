using Game.Core.LevelBase;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class PlayButton : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI label;

		private void Start()
		{
			label.text = $"Level {(int)LevelProgress.Current + 1}";

			GetComponent<Button>().onClick.AddListener(LoadLevelScene);
		}

		private void LoadLevelScene()
		{
			SceneManager.LoadScene(SceneNames.Level);
		}
	}
}
