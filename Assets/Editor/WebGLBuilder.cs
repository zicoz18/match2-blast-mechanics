using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameEditor
{
	/// <summary>
	/// Headless WebGL build entry point, invoked from the command line:
	///   Unity -quit -batchmode -projectPath . -executeMethod GameEditor.WebGLBuilder.Build -logFile -
	/// </summary>
	public static class WebGLBuilder
	{
		// MainScene first: the scene at index 0 is what the build boots into.
		private static readonly string[] ScenePaths =
		{
			"Assets/Scenes/MainScene.unity",
			"Assets/Scenes/LevelScene.unity",
		};
		private const string OutputPath = "Build/WebGL";

		public static void Build()
		{
			// The project's build settings still point at a SampleScene that no longer
			// exists, so the scene list is set explicitly rather than read from there.
			var buildScenes = new EditorBuildSettingsScene[ScenePaths.Length];
			for (int i = 0; i < ScenePaths.Length; i++)
			{
				buildScenes[i] = new EditorBuildSettingsScene(ScenePaths[i], true);
			}
			EditorBuildSettings.scenes = buildScenes;

			// Custom template: the stock one pins the canvas at 960x600 inside a page frame.
			PlayerSettings.WebGL.template = "PROJECT:Responsive";

			// GitHub Pages serves pre-compressed files without a Content-Encoding header
			// and cannot be configured to send one, so the browser never un-compresses
			// them and the loader aborts. decompressionFallback embeds a decompressor in
			// the loader instead, which costs a little startup time and works on any
			// static host. Gzip rather than Brotli because the JavaScript Brotli
			// decompressor is far slower, which is exactly what the fallback exercises.
			PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
			PlayerSettings.WebGL.decompressionFallback = true;
			PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.None;
			PlayerSettings.WebGL.dataCaching = true;
			PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL, ManagedStrippingLevel.High);

			var options = new BuildPlayerOptions
			{
				scenes = ScenePaths,
				locationPathName = OutputPath,
				target = BuildTarget.WebGL,
				targetGroup = BuildTargetGroup.WebGL,
				options = BuildOptions.None
			};

			Debug.Log($"[WebGLBuilder] Building {ScenePaths.Length} scene(s) -> {OutputPath}");
			BuildReport report = BuildPipeline.BuildPlayer(options);
			var summary = report.summary;

			Debug.Log($"[WebGLBuilder] Result: {summary.result}  " +
			          $"size: {summary.totalSize / (1024 * 1024)} MB  " +
			          $"time: {summary.totalTime}");

			if (summary.result != BuildResult.Succeeded)
			{
				EditorApplication.Exit(1);
			}
		}
	}
}
