using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(MoveBetweenPoints)), CanEditMultipleObjects]
public class MoveBetweenPointsEditor : Editor 
{
    private MoveBetweenPoints mbp;

    public override void OnInspectorGUI()
    {
        mbp = (MoveBetweenPoints)target;

        GUI.changed = false;

        mbp.axis     = (MoveBetweenPoints.Axis)EditorGUILayout.EnumPopup("Move Axis", mbp.axis);
        mbp.moveType = (MoveBetweenPoints.MoveType)EditorGUILayout.EnumPopup("Move Type", mbp.moveType);

        DisplayOptions();

        // base.OnInspectorGUI();

        mbp.pauseAtEnd = (bool)EditorGUILayout.Toggle("Pause at end", mbp.pauseAtEnd);

        if(mbp.pauseAtEnd)
        {
            mbp.pauseSeconds = (float)EditorGUILayout.FloatField("Pause Amount", mbp.pauseSeconds);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(mbp);
        }
    }


    private void DisplayOptions()
    {
        switch (mbp.moveType)
        {
            // Point to Point
            case MoveBetweenPoints.MoveType.Point2Point:

                mbp.startPoint = (Vector3)EditorGUILayout.Vector3Field("Start Point", mbp.startPoint);
                mbp.endPoint   = (Vector3)EditorGUILayout.Vector3Field("End Point", mbp.endPoint);
                mbp.speed      = (float)EditorGUILayout.Slider("Move Speed", mbp.speed, 0f, 25f);

                break;
            // Initial to Point
            case MoveBetweenPoints.MoveType.Initial2Point:

                mbp.endPoint = (Vector3)EditorGUILayout.Vector3Field("End Point", mbp.endPoint);
                mbp.speed    = (float)EditorGUILayout.Slider("Move Speed", mbp.speed, 0f, 25f);

                break;
            // Distance Based 
            case MoveBetweenPoints.MoveType.DistanceBased:

                mbp.distance = (float)EditorGUILayout.FloatField("Distance", mbp.distance);
                mbp.speed    = (float)EditorGUILayout.Slider("Move Speed", mbp.speed, 0f, 25f);

                break;
        }
    }
	
}
