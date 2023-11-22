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

    private SerializedProperty minRunningVolumnRange; //running
    private SerializedProperty maxRunningVolumnRange;
    private SerializedProperty minRunningPitchRange;
    private SerializedProperty maxRunningPitchRange; 

    private SerializedProperty steppingAudioClipsGrass;
    private SerializedProperty steppingAudioClipConcrete;
    private SerializedProperty steppingAudioClipGrassWeed;
    private SerializedProperty steppingAudioClipStone;

    private float minWalkingVolumnRangeFloat = 0f;//to adjust the volumn of the walk
    private float maxWalkingVolumnRangeFloat = 1f;

    private float maxWalkingPitchRangeFloat = 1f; //to adjust the pitch of the walk
    private float minWalkingPitchRangeFloat = 0f;

    private float minRunningVolumnRangeFloat = 0f;
    private float maxRunningVolumnRangeFloat = 1f;

    private float minRunningPitchRangeFloat = 0f;
    private float maxRunningPitchRangeFloat = 1f;

    private bool walkingSoundFoldoutGroup = false; //used for the fold out group
    private bool runningSoundFoldoutGroup = false;
    #endregion 

    private void OnEnable()
    {
        transform = serializedObject.FindProperty("playerTransform");
        minWalkPitchRange = serializedObject.FindProperty("minWalkPitchRange");
        maxWalkPitchRange = serializedObject.FindProperty("maxWalkPitchRange");
        minWalkVolumnRange = serializedObject.FindProperty("minWalkVolumnRange");
        maxWalkVolumnRange = serializedObject.FindProperty("maxWalkVolumnRange");

        minRunningVolumnRange = serializedObject.FindProperty("minRunningVolumnRange");
        maxRunningVolumnRange = serializedObject.FindProperty("maxRunningVolumnRange");
        minRunningPitchRange = serializedObject.FindProperty("minRunningPitchRange");
        maxRunningPitchRange = serializedObject.FindProperty("maxRunningPitchRange");

        steppingAudioClipsGrass = serializedObject.FindProperty("steppingAudioClipGrass");
        steppingAudioClipConcrete = serializedObject.FindProperty("steppingAudioClipConcrete");
        steppingAudioClipGrassWeed = serializedObject.FindProperty("steppingAudioClipGrassWeed");
        steppingAudioClipStone = serializedObject.FindProperty("steppingAudioClipStone");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(transform);

        walkingSoundFoldoutGroup = EditorGUILayout.BeginFoldoutHeaderGroup(walkingSoundFoldoutGroup, "Walking sound");
        if(walkingSoundFoldoutGroup)
        {
            minWalkingVolumnRangeFloat = minWalkVolumnRange.floatValue;
            maxWalkingVolumnRangeFloat = maxWalkVolumnRange.floatValue;
            minWalkingPitchRangeFloat = minWalkPitchRange.floatValue;
            maxWalkingPitchRangeFloat = maxWalkPitchRange.floatValue;

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
            maxWalkPitchRange.floatValue = maxWalkingPitchRangeFloat;

            minWalkVolumnRange.floatValue = minWalkingVolumnRangeFloat;
            maxWalkVolumnRange.floatValue = maxWalkingVolumnRangeFloat;

        } //for walking
        EditorGUILayout.EndFoldoutHeaderGroup();

        runningSoundFoldoutGroup = EditorGUILayout.BeginFoldoutHeaderGroup(runningSoundFoldoutGroup, "Running sound");
        if (runningSoundFoldoutGroup)
        {
            minRunningPitchRangeFloat = minRunningPitchRange.floatValue;
            maxRunningPitchRangeFloat = maxRunningPitchRange.floatValue;
            minRunningVolumnRangeFloat = minRunningVolumnRange.floatValue;
            maxRunningVolumnRangeFloat = maxRunningVolumnRange.floatValue;

            EditorGUILayout.LabelField($"min pitch: {minRunningPitchRangeFloat}");
            EditorGUILayout.LabelField($"max pitch: {maxRunningPitchRangeFloat}");
            EditorGUILayout.MinMaxSlider("Range of Pitch",
                ref minRunningPitchRangeFloat,
                ref maxRunningPitchRangeFloat,
                0f,
                1f
                );
            EditorGUILayout.LabelField($"min volumn: {minRunningVolumnRangeFloat}");
            EditorGUILayout.LabelField($"max volumn: {maxRunningVolumnRangeFloat}");
            EditorGUILayout.MinMaxSlider("Range of Volumn",
                ref minRunningVolumnRangeFloat,
                ref maxRunningVolumnRangeFloat,
                0f,
                1f
                );


            minRunningPitchRange.floatValue = minRunningPitchRangeFloat;
            maxRunningPitchRange.floatValue = maxRunningPitchRangeFloat;

            minRunningVolumnRange.floatValue = minRunningVolumnRangeFloat;
            maxRunningVolumnRange.floatValue = maxRunningVolumnRangeFloat;
        } //for running

        EditorGUILayout.EndFoldoutHeaderGroup() ;
        EditorGUILayout.PropertyField(steppingAudioClipsGrass);
        EditorGUILayout.PropertyField(steppingAudioClipConcrete);
        EditorGUILayout.PropertyField(steppingAudioClipGrassWeed);
        EditorGUILayout.PropertyField(steppingAudioClipStone);


        serializedObject.ApplyModifiedProperties();
    }
}
