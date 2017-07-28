using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(MoveBetweenPoints))]
public class MoveBetweenPointsEditor : Editor 
{
    private MoveBetweenPoints mbp;

    public override void OnInspectorGUI()
    {
        mbp = (MoveBetweenPoints)target;

        mbp.axis     = (MoveBetweenPoints.Axis)EditorGUILayout.EnumPopup("Move Axis", mbp.axis);
        mbp.moveType = (MoveBetweenPoints.MoveType)EditorGUILayout.EnumPopup("Move Type", mbp.moveType);

        DisplayOptions();

        // base.OnInspectorGUI();
    }


    private void DisplayOptions()
    {
        switch (mbp.moveType)
        {
            // Point to Point
            case MoveBetweenPoints.MoveType.Point2Point:

                mbp.startPoint = EditorGUILayout.Vector3Field("Start Point", mbp.startPoint);
                mbp.endPoint   = EditorGUILayout.Vector3Field("End Point", mbp.endPoint);
                mbp.speed      = EditorGUILayout.Slider("Move Speed", mbp.speed, 0f, 25f);

                break;
            // Initial to Point
            case MoveBetweenPoints.MoveType.Initial2Point:

                mbp.endPoint = EditorGUILayout.Vector3Field("End Point", mbp.endPoint);
                mbp.speed    = EditorGUILayout.Slider("Move Speed", mbp.speed, 0f, 25f);

                break;
            // Distance Based 
            case MoveBetweenPoints.MoveType.DistanceBased:

                mbp.distance = EditorGUILayout.FloatField("Distance", mbp.distance);
                mbp.speed    = EditorGUILayout.Slider("Move Speed", mbp.speed, 0f, 25f);

                break;
        }
    }
	
}
