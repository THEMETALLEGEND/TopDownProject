using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Configs
{
    [CustomEditor(typeof(SlotConfig))]
    public class SlotConfigEditor : Editor
    {
        private SlotConfig _slotConfig;
        private Dictionary<int, GameObject> _selectedObjects = new Dictionary<int, GameObject>();

        public override void OnInspectorGUI()
        {
            _slotConfig = (SlotConfig)target;

            EditorGUILayout.LabelField("Add Slot", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add to Interaction"))
                AddSlot(_slotConfig.interactionSlots);
            if (GUILayout.Button("Add to Queue"))
                AddSlot(_slotConfig.queueSlots);
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Interaction Slots", EditorStyles.boldLabel);
            DrawSlotList(_slotConfig.interactionSlots);

            EditorGUILayout.LabelField("Queue Slots", EditorStyles.boldLabel);
            DrawSlotList(_slotConfig.queueSlots);
            
            DrawPositionField("Spawn Position", ref _slotConfig.spawnPosition);
            DrawPositionField("Despawn Position", ref _slotConfig.despawnPosition);

            if (GUI.changed)
                EditorUtility.SetDirty(_slotConfig);
        }

        private void DrawPositionField(string label, ref Vector3 position)
        {
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            
            GameObject selectedObject = (GameObject)EditorGUILayout.ObjectField(
                null, 
                typeof(GameObject), 
                true, 
                GUILayout.Width(150)
            );
            
            if (selectedObject != null)
            {
                position = selectedObject.transform.position;
                EditorUtility.SetDirty(_slotConfig);
                AssetDatabase.SaveAssets();
            }
            
            position = EditorGUILayout.Vector3Field("", position);
            GUILayout.EndHorizontal();
        }

        private void AddSlot(List<Slot> list)
        {
            list.Add(new Slot { Position = Vector3.zero, IsBusy = false });
            EditorUtility.SetDirty(_slotConfig);
            AssetDatabase.SaveAssets();
        }

        private void DrawSlotList(List<Slot> list)
        {
            SyncSelectionDictionary(list.Count);

            for (var i = 0; i < list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Slot {i}", GUILayout.Width(50));

                GameObject newSelection = (GameObject)EditorGUILayout.ObjectField(
                    _selectedObjects[i], 
                    typeof(GameObject), 
                    true, 
                    GUILayout.Width(150)
                );

                if (newSelection != _selectedObjects[i])
                {
                    _selectedObjects[i] = newSelection;
                    if (newSelection != null)
                    {
                        list[i].Position = newSelection.transform.position;
                        EditorUtility.SetDirty(_slotConfig);
                        AssetDatabase.SaveAssets();
                    }
                }

                list[i].Position = EditorGUILayout.Vector3Field("", list[i].Position);
                list[i].IsBusy = EditorGUILayout.Toggle(list[i].IsBusy, GUILayout.Width(20));
                
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    list.RemoveAt(i);
                    _selectedObjects.Remove(i);
                    SyncSelectionDictionary(list.Count);
                    EditorUtility.SetDirty(_slotConfig);
                    AssetDatabase.SaveAssets();
                    return;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void SyncSelectionDictionary(int requiredCount)
        {
            for (int i = _selectedObjects.Count; i < requiredCount; i++)
            {
                _selectedObjects[i] = null;
            }
            
            List<int> keysToRemove = new List<int>();
            foreach (var key in _selectedObjects.Keys)
            {
                if (key >= requiredCount)
                    keysToRemove.Add(key);
            }
            foreach (var key in keysToRemove)
            {
                _selectedObjects.Remove(key);
            }
        }
    }
}