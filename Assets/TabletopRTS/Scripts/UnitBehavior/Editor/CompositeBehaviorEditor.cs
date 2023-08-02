using UnityEditor;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    [CustomEditor(typeof(CompositeBehavior))]
    public class CompositeBehaviorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CompositeBehavior cb = (CompositeBehavior)target;
            
            Rect r = EditorGUILayout.BeginHorizontal();
            r.height = EditorGUIUtility.singleLineHeight;

            if (cb.Behaviors == null || cb.Behaviors.Length == 0)
            {
                Debug.Log("No behaviors in array");
                EditorGUILayout.HelpBox("No behaviors in array.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
                r = EditorGUILayout.BeginHorizontal();
                r.height = EditorGUIUtility.singleLineHeight;
            }
            else
            {
                Debug.Log("In the Else");
                r.x = 30f;
                r.width = EditorGUIUtility.currentViewWidth - 95f;
                EditorGUI.LabelField(r, "Behaviors");
                r.x = EditorGUIUtility.currentViewWidth - 65f;
                r.width = 60f;
                EditorGUI.LabelField(r, "Weights");
                r.y += EditorGUIUtility.singleLineHeight * 1.2f;
                
                EditorGUI.BeginChangeCheck();
                for (int i = 0; i < cb.Behaviors.Length; i++)
                {
                    Debug.Log($"Adding Behavior {i} to list.");
                    r.x = 5f;
                    r.width = 20f;
                    EditorGUI.LabelField(r, i.ToString());
                    r.x = 30f;
                    r.width = EditorGUIUtility.currentViewWidth - 95f;
                    cb.Behaviors[i] = (FlockBehavior)EditorGUI.ObjectField(r, cb.Behaviors[i], typeof(FlockBehavior), false);
                    r.x = EditorGUIUtility.currentViewWidth - 65f;
                    r.width = 60f;
                    cb.weights[i] = EditorGUI.FloatField(r, cb.weights[i]);
                    r.y += EditorGUIUtility.singleLineHeight * 1.1f;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(cb);
                }
            }
            Debug.Log("just passed the Else");
            EditorGUILayout.EndHorizontal();
            r.x = 5f;
            r.width = EditorGUIUtility.currentViewWidth - 10f;
            r.y += EditorGUIUtility.singleLineHeight * .5f;
            if (GUI.Button(r, "Add Behavior"))
            {
                Debug.Log("Hit Add Behavior Button");
                AddBehavior(cb);
                EditorUtility.SetDirty(cb);
            }

            r.y += EditorGUIUtility.singleLineHeight * 1.5f;
            if (cb.Behaviors != null && cb.Behaviors.Length > 0)
            {
                if (GUI.Button(r, "Remove Behavior"))
                {
                    Debug.Log("Hit Remove Behavior Button");
                    RemoveBehavior(cb);
                    EditorUtility.SetDirty(cb);
                }
            }
            Debug.Log("End of Update");
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

