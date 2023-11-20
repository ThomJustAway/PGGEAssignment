using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundProducer))]
public class SoundProducerEditor : Editor
{
    private SerializedProperty transform;

    #region walking clips
    private SerializedProperty minWalkVolumnRange; //getting the values from the sound Producer
    private SerializedProperty maxWalkVolumnRange;
    private SerializedProperty minWalkPitchRange;
    private SerializedProperty maxWalkPitchRange;
    private SerializedProperty walkingAudioClipsGrass;

    private float minWalkingVolumnRangeFloat = 0f;//to adjust the volumn of the walk
    private float maxWalkingVolumnRangeFloat = 1f;

    private float maxWalkingPitchRangeFloat = 1f; //to adjust the pitch of the walk
    private float minWalkingPitchRangeFloat = 0f;

    private bool walkingSoundFoldoutGroup = false; //used for the fold out group
    #endregion 

    private void OnEnable()
    {
        transform = serializedObject.FindProperty("playerTransform");
        minWalkPitchRange = serializedObject.FindProperty("minWalkPitchRange");
        maxWalkPitchRange = serializedObject.FindProperty("maxWalkPitchRange");
        minWalkVolumnRange = serializedObject.FindProperty("minWalkVolumnRange");
        maxWalkVolumnRange = serializedObject.FindProperty("maxWalkVolumnRange");
        walkingAudioClipsGrass = serializedObject.FindProperty("walkingAudioClipGrass");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(transform);

        walkingSoundFoldoutGroup = EditorGUILayout.BeginFoldoutHeaderGroup(walkingSoundFoldoutGroup, "Walking sound");
        if(walkingSoundFoldoutGroup)
        {
            EditorGUILayout.LabelField($"min pitch: {minWalkingPitchRangeFloat}");
            EditorGUILayout.LabelField($"max pitch: {maxWalkingPitchRangeFloat}");
            EditorGUILayout.MinMaxSlider("Range of Pitch",
                ref minWalkingPitchRangeFloat,
                ref maxWalkingPitchRangeFloat,
                0f,
                1f
                );
            EditorGUILayout.LabelField($"min volumn: {minWalkingVolumnRangeFloat}");
            EditorGUILayout.LabelField($"max volumn: {maxWalkingVolumnRangeFloat}");
            EditorGUILayout.MinMaxSlider("Range of Volumn",
                ref minWalkingVolumnRangeFloat,
                ref maxWalkingVolumnRangeFloat,
                0f,
                1f
                );
            minWalkPitchRange.floatValue = minWalkingPitchRangeFloat;
            maxWalkPitchRange.floatValue= maxWalkingPitchRangeFloat;

            minWalkVolumnRange.floatValue = minWalkingVolumnRangeFloat;
            maxWalkVolumnRange.floatValue = maxWalkingVolumnRangeFloat;

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.PropertyField(walkingAudioClipsGrass);


        serializedObject.ApplyModifiedProperties();
    }
}
