using UnityEngine;
using UnityEditor;
using System.Collections;
using Mechanics;

[CustomEditor(typeof(ProjectilePattern)), CanEditMultipleObjects]
public class ProjectilePatternEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        ProjectilePattern pp = (ProjectilePattern)target;

        serializedObject.Update();
        //base.OnInspectorGUI();
        //base.OnInspectorGUI();
        GUI.changed = false;
        //EditorGUILayout.FloatField("Arc", pp.arc);
        //pp.pMats = EditorGUILayout.ObjectField("Materials", pp.pMats, typeof(Material[]), false) as Material[];
        DisplayArray("ParticleMaterials");

        pp.pPrefab     = EditorGUILayout.ObjectField("Projectile Base", pp.pPrefab, typeof(GameObject), false) as GameObject;
        pp.arc         = (float)EditorGUILayout.Slider("Arc", pp.arc, 1f, 360f);
        pp.pCount      = (int)EditorGUILayout.IntSlider("Projectile Count ", pp.pCount, 1, 128);
        pp.RoF         = (float)EditorGUILayout.Slider("Rate of Fire", pp.RoF, 0.01f, 10);
        pp.pSpeed      = (float)EditorGUILayout.FloatField("Projectile Speed", pp.pSpeed);
        pp.enableBoost = (bool)EditorGUILayout.Toggle("Enable Boost", pp.enableBoost);

        if (pp.enableBoost)
        {
            pp.boostAmount = (float)EditorGUILayout.Slider("Boost Amount", pp.boostAmount, 1f, 4f);
            pp.boostDecay = (float)EditorGUILayout.Slider("Decay Rate", pp.boostDecay, 0.01f, 0.5f);
        }

        pp.continuous = (bool)EditorGUILayout.Toggle("Continues Fire ", pp.continuous);

        if (!pp.continuous)
        {
            if (GUILayout.Button("Fire Volley"))
            {
                pp.Fire();
            }
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(pp);
        }

        serializedObject.ApplyModifiedProperties();
    }


    private void DisplayArray(string property)
    {
        SerializedProperty sp = serializedObject.FindProperty(property);
        
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(sp, true);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

    }

}
