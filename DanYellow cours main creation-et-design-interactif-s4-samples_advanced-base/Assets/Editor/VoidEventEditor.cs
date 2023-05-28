using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoidEventChannelSO), editorForChildClasses: true)]
public class VoidEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        VoidEventChannelSO e = target as VoidEventChannelSO;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}
