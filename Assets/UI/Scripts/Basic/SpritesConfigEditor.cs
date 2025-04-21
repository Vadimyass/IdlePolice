#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UI
{
    [CustomEditor(typeof(SpritesConfig))]
    public class SpritesConfigEditor : Editor
    {
        // Число колонок в сетке
        private const int columns = 4;

        public override void OnInspectorGUI()
        {
            SpritesConfig config = (SpritesConfig)target;

            // Получаем доступ к списку _sprites через рефлексию
            SerializedProperty spritesList = serializedObject.FindProperty("_sprites");

            // Кнопка для добавления нового элемента в список (растягивается на всю ширину)
            if (GUILayout.Button("Add sprite", GUILayout.ExpandWidth(true)))
            {
                spritesList.InsertArrayElementAtIndex(spritesList.arraySize);
            }

            // Кнопка для удаления последнего элемента из списка (растягивается на всю ширину)
            if (spritesList.arraySize > 0 && GUILayout.Button("Remove last sprite", GUILayout.ExpandWidth(true)))
            {
                spritesList.DeleteArrayElementAtIndex(spritesList.arraySize - 1);
            }

            
            // Устанавливаем количество колонок
            int count = 0;
            

            // Итерируем по элементам списка
            for (int i = 0; i < spritesList.arraySize; i++)
            {
                // Начинаем новую строку, если count равен 0
                if (count == 0)
                {
                    EditorGUILayout.BeginHorizontal(); // Начало строки
                }

                EditorGUILayout.BeginVertical("box", GUILayout.Width(120), GUILayout.Height(140)); // Один элемент сетки

                // Получаем текущий элемент структуры GameSprite
                SerializedProperty gameSprite = spritesList.GetArrayElementAtIndex(i);

                // Отображаем поле для выбора SpriteName
                SerializedProperty spriteName = gameSprite.FindPropertyRelative("SpriteName");
                EditorGUILayout.PropertyField(spriteName, GUIContent.none, GUILayout.ExpandWidth(true));

                // Отображаем спрайт с миниатюрой
                SerializedProperty sprite = gameSprite.FindPropertyRelative("Sprite");
                sprite.objectReferenceValue = (Sprite)EditorGUILayout.ObjectField(
                    sprite.objectReferenceValue,
                    typeof(Sprite),
                    false,
                    GUILayout.Width(120),
                    GUILayout.Height(120));

                EditorGUILayout.EndVertical();

                count++;

                // Если количество элементов в строке равно columns, закрываем строку
                if (count >= columns)
                {
                    EditorGUILayout.EndHorizontal(); // Конец строки
                    count = 0;
                }
            }

            // Если остались незавершённые строки, закрываем их
            if (count > 0)
            {
                EditorGUILayout.EndHorizontal(); // Конец последней строки
            }
            
            // Сохраняем изменения
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
