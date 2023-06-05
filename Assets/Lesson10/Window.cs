using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Window : EditorWindow
{
    private TestScript lookAtPoint;
    private Editor MainWindow;

    public TestScript Main { get; private set; }

    [MenuItem("Window/TestScript")]
    public static void Show() 
    {
        EditorWindow.GetWindow<Window>();
    }
    public void OnGUI()
    {
        Main = (TestScript)EditorGUILayout.ObjectField(Main, typeof(TestScript));

        if (Main != null) 
        {
            MainWindow ??= Editor.CreateEditor(Main);
            MainWindow.OnInspectorGUI();
        }
    }
}
