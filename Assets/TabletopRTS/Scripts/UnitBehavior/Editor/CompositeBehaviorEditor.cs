using UnityEditor;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    [CustomEditor(typeof(CompositeBehavior))]
    public class CompositeBehaviorEditor : Editor
    {
        static readonly int indexWidth = 20;
        static readonly int behaviorWidth = 200;
        static readonly int weightWidth = 50;

        public override void OnInspectorGUI()
        {
            CompositeBehavior cb = (CompositeBehavior)target;
            
            EditorGUILayout.BeginHorizontal();

            if (cb.Behaviors == null || cb.Behaviors.Length == 0)
            {
                EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                
                // Header
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(indexWidth));
                EditorGUILayout.LabelField("Behaviors", GUILayout.Width(behaviorWidth));
                EditorGUILayout.LabelField("Weights", GUILayout.Width(weightWidth));
                EditorGUILayout.EndHorizontal();
                
                EditorGUI.BeginChangeCheck();
                for (int i = 0; i < cb.Behaviors.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(indexWidth));
                    cb.Behaviors[i] = (FlockBehavior)EditorGUILayout.ObjectField(cb.Behaviors[i], typeof(FlockBehavior), false, GUILayout.Width(behaviorWidth));
                    cb.weights[i] = EditorGUILayout.FloatField(cb.weights[i], GUILayout.Width(weightWidth));
                    
                    EditorGUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(cb);
                }
                
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Add Behavior"))
            {
                AddBehavior(cb);
                EditorUtility.SetDirty(cb);
            }

            if (cb.Behaviors != null && cb.Behaviors.Length > 0)
            {
                if (GUILayout.Button("Remove Behavior"))
                {
                    RemoveBehavior(cb);
                    EditorUtility.SetDirty(cb);
                }
            }
        }

        private void AddBehavior(CompositeBehavior cb)
        {
            int oldCount = (cb.Behaviors != null) ? cb.Behaviors.Length : 0;
            FlockBehavior[] newBehaviors = new FlockBehavior[oldCount + 1];
            float[] newWeights = new float[oldCount + 1];
            for (int i = 0; i < oldCount; i++)
            {
                newBehaviors[i] = cb.Behaviors[i];
                newWeights[i] = cb.weights[i];
            }
            newWeights[oldCount] = 1f;
            cb.Behaviors = newBehaviors;
            cb.weights = newWeights;
        }

        private void RemoveBehavior(CompositeBehavior cb)
        {
            int oldCount = cb.Behaviors.Length;
            if (oldCount == 1)
            {
                cb.Behaviors = null;
                cb.weights = null;
                return;
            }

            FlockBehavior[] newBehaviors = new FlockBehavior[oldCount - 1];
            float[] newWeights = new float[oldCount - 1];
            for (int i = 0; i < oldCount - 1; i++)
            {
                newBehaviors[i] = cb.Behaviors[i];
                newWeights[i] = cb.weights[i];
            }

            cb.Behaviors = newBehaviors;
            cb.weights = newWeights;
        }
    }    
}

