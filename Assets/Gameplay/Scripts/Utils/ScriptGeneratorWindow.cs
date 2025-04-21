using UnityEditor;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
public class ScriptGeneratorWindow : EditorWindow
{
    private string scriptName = "NewUIScreen";  // Название по умолчанию

    [MenuItem("Tools/Script Generator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptGeneratorWindow>("Script Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Script Generator", EditorStyles.boldLabel);

        // Поле для ввода названия
        scriptName = EditorGUILayout.TextField("Script Name", scriptName);

        if (GUILayout.Button("Generate Scripts"))
        {
            GenerateScripts();
        }
    }

    private void GenerateScripts()
    {
        // Убедимся, что задано название
        if (string.IsNullOrEmpty(scriptName))
        {
            Debug.LogError("Script name must not be empty!");
            return;
        }

        // Путь к папке
        string folderPath = Path.Combine(Application.dataPath, "UI/Scripts", scriptName);

        // Если папка не существует, создаем её
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Folder '{scriptName}' created at {folderPath}");
        }
        else
        {
            Debug.LogWarning($"Folder '{scriptName}' already exists.");
        }

        // Создаем три скрипта
        CreateScript1(folderPath);
        CreateScript2(folderPath);
        CreateScript3(folderPath);

        // Обновляем окно проекта
        AssetDatabase.Refresh();
    }

    private void CreateScript1(string folderPath)
    {
        string scriptPath = Path.Combine(folderPath, scriptName + ".cs");
        string scriptContent =
$@"using UnityEngine;

namespace UI.Scripts.{scriptName}
{{
    public class {scriptName} : UIScreen
    {{
        
    }}
}}";

        File.WriteAllText(scriptPath, scriptContent);
        Debug.Log($"Script '{scriptName}' created at {scriptPath}");
    }

    private void CreateScript2(string folderPath)
    {
        string scriptPath = Path.Combine(folderPath, scriptName + "Controller.cs");
        string scriptContent =
$@"namespace UI.Scripts.{scriptName}
{{
    public class {scriptName}Controller : UIScreenController<{scriptName}>
    {{
        
    }}
}}";

        File.WriteAllText(scriptPath, scriptContent);
        Debug.Log($"Script '{scriptName}Controller' created at {scriptPath}");
    }

    private void CreateScript3(string folderPath)
    {
        string scriptPath = Path.Combine(folderPath, scriptName + "Animation.cs");
        string scriptContent =
$@"using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.{scriptName}
{{
    public class {scriptName}Animation : UIAnimation
    {{
        public override UniTask ShowAnimation()
        {{
            return UniTask.CompletedTask;
        }}

        public override UniTask HideAnimation()
        {{
            return UniTask.CompletedTask;
        }}
    }}
}}";

        File.WriteAllText(scriptPath, scriptContent);
        Debug.Log($"Script '{scriptName}Animation' created at {scriptPath}");
    }
}
#endif
