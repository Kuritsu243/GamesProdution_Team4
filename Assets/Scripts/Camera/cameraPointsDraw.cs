using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(cameraScenePreview))]
public class cameraPointsDraw : Editor
{
    private cameraScenePreview _connectedObjects;
    private GameObject _previousPos;
    private ReorderableList _points;
    private SerializedProperty _listProperty;
    private SerializedProperty _moveSpeed;
    private SerializedProperty _rotateSpeed;
    private void OnSceneGUI()
    {
        _connectedObjects = target as cameraScenePreview;
        if (_connectedObjects.FollowPoints == null)
        {
            return;
        }
    
        var numberOfPoints = _connectedObjects.FollowPoints.Count;
        for (var i = 0; i < numberOfPoints; i++)
        {
            var connectedObject = _connectedObjects.FollowPoints[i];
            Handles.color = Color.green;
            Handles.DrawLine(
                i == 0
                    ? _connectedObjects.transform.position
                    : _connectedObjects.FollowPoints[i - 1].transform.position, connectedObject.transform.position);
            if (connectedObject.transform.localRotation.x != 0 || connectedObject.transform.localRotation.y != 0 || connectedObject.transform.localRotation.z != 0)
            {
                Handles.color = Color.red;
                Handles.DrawLine(connectedObject.transform.position, connectedObject.transform.position + connectedObject.transform.forward * 5);
            }
        }
    }
    
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();
        _points.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Add new point"))
        {
            var newObj = new GameObject();
            if (_connectedObjects.FollowPoints.Count == 0)
            {
                newObj.transform.parent = GameObject.Find("scenePreview").transform;
                newObj.transform.position = GameObject.Find("scenePreview").transform.position;
            }
            else
            {
                newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent;
                newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                            _connectedObjects.FollowPoints[^1].transform.forward * 5;
            }
            _connectedObjects.FollowPoints.Add(newObj);
        }
        if (GUILayout.Button("Add new point with inherited rotation"))
        {
            var newObj = new GameObject();
            if (_connectedObjects.FollowPoints.Count == 0)
            {
                newObj.transform.parent = GameObject.Find("scenePreview").transform;
                newObj.transform.position = GameObject.Find("scenePreview").transform.position;
                newObj.transform.localRotation = GameObject.Find("scenePreview").transform.localRotation;
            }
            else
            {
                newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent;
                newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                            _connectedObjects.FollowPoints[^1].transform.forward * 5;
                newObj.transform.rotation = _connectedObjects.FollowPoints[^1].transform.rotation;
            }
            _connectedObjects.FollowPoints.Add(newObj);
        }

        EditorGUILayout.PropertyField(_moveSpeed);
        EditorGUILayout.PropertyField(_rotateSpeed);
        serializedObject.ApplyModifiedProperties();
    }
    
    private void OnEnable()
    {
        _listProperty = serializedObject.FindProperty("followPoints");
        _moveSpeed = serializedObject.FindProperty("moveSpeed");
        _rotateSpeed = serializedObject.FindProperty("rotateSpeed");
        _points = new ReorderableList(serializedObject, _listProperty, true, true, false, false);
        
        #region ReordableList Draw callback

        _points.drawElementCallback =
            (rect, index, active, focused) =>
            {
                if (index > _points.serializedProperty.arraySize -1 ) return;
                var element = _points.serializedProperty.GetArrayElementAtIndex(index);
                
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
                }
                EditorGUI.EndDisabledGroup();
                
                if (GUI.Button(new Rect(rect.x + rect.width - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), "X"))
                {
                    var obj = (GameObject) element.objectReferenceValue;

                    if (obj)
                    {
                        DestroyImmediate(obj);
                    }

                    if(_points.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue != null)
                        _points.serializedProperty.DeleteArrayElementAtIndex(index);
                    _points.serializedProperty.DeleteArrayElementAtIndex(index);

                    serializedObject.ApplyModifiedProperties();
                }
            };
        #endregion
    }
}




