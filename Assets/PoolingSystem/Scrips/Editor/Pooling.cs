using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace PoolingSystem.Editor
{
    public class CreatePoolingList : EditorWindow
    {
        private string m_ListName = string.Empty;

        [MenuItem("Pooling/Create Pooling List")]
        public static void AddPoolingList()
        {
            EditorWindow.GetWindow<CreatePoolingList>();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Create New Pool");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pool Name", GUILayout.Width(100));
            m_ListName = EditorGUILayout.TextField(m_ListName, GUILayout.Width(300));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create"))
            {
                Pooling.PoolingManager.CreatePoolingList(null, m_ListName, 100, true, 0, false);
                var activeScene = EditorSceneManager.GetActiveScene();
                if (!activeScene.isDirty) EditorSceneManager.MarkSceneDirty(activeScene);
            }
        }
    }
}


