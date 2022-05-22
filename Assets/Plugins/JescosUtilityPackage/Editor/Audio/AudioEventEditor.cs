#if (UNITY_EDITOR) // exclude from build

using UnityEngine;
using UnityEditor;

namespace Beta.Audio {

	/// <summary> UnityUI-Code. Don't question it, it just works. </summary>
	[CustomEditor(typeof(AudioEvent), true)]
	public class AudioEventEditor : UnityEditor.Editor {

		[SerializeField] private AudioSource _previewer;

		public void OnEnable() {
			_previewer = EditorUtility
				.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource))
				.GetComponent<AudioSource>();
		}

		public void OnDisable() {
			DestroyImmediate(_previewer.gameObject);
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
			if (GUILayout.Button("Preview")) {
				((AudioEvent) target).Play(_previewer);
			}

			EditorGUI.EndDisabledGroup();
		}
	}
}

#endif