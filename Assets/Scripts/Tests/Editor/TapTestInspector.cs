using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TapTest))]
public class TapTestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var tester = target as TapTest;
        if(tester.Playing)
        {
            GUILayout.Label("playing ...");
            return;
        }

        if(tester.Recoding)
        {
            if (GUILayout.Button("Stop"))
            {
                tester.Stop();
            }
        }
        else
        {
            if (GUILayout.Button("Start Recoding"))
            {
                tester.Record();
            }
            if (GUILayout.Button("Play"))
            {
                tester.Play();
            }
        }
    }
}
