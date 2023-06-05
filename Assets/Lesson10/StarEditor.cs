using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarEditor : Editor
{
    private SerializedProperty _center;
    private SerializedProperty _points;
    private SerializedProperty _frequency;
    private void OnEnable()
    {
        _center = serializedObject.FindProperty("_center");
        _points = serializedObject.FindProperty("_points");
        _frequency = serializedObject.FindProperty("_frequency");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_center);
        EditorGUILayout.PropertyField(_points);
        EditorGUILayout.IntSlider(_frequency, 1, 20);
        int totalPoints = _frequency.intValue * _points.arraySize;
        if (totalPoints < 3)
        {
            EditorGUILayout.HelpBox("At least three points are needed.",
            MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox(totalPoints + " points in total.",
            MessageType.Info);
        }
        serializedObject.ApplyModifiedProperties();
    }
    private void OnSceneGUI()
    {
        if (!(target is Star star))
        {
            return;
        }
        Transform starTransform = star.transform;
        float angle = -360f / (star.Frequency * star.Points.Length);
        for (int i = 0; i < star.Points.Length; i++)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle * i);
            Vector3 oldPoint = starTransform.TransformPoint(rotation * star.Points[i].Position);
            Vector3 snap = Vector3.one * 0.5f;
            Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.02f, snap, Handles.DotHandleCap);
            if (oldPoint == newPoint)
            {
                continue;
            }
            star.Points[i].Position = Quaternion.Inverse(rotation) * starTransform.InverseTransformPoint(newPoint);
            star.UpdateMesh();
        }
    }
}