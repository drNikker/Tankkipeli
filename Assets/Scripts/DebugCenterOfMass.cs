using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Rigidbody))]

public class DebugCenterOfMass : Editor
{
    void OnSceneGUI()
    {
        Rigidbody rb = target as Rigidbody;
        Handles.color = Color.red;
        Handles.SphereCap(2, rb.transform.TransformPoint(rb.centerOfMass), rb.rotation, 0.1f);
    }
    public override void OnInspectorGUI()
    {
        GUI.skin = EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Inspector);
        DrawDefaultInspector();
    }
}
