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
        _connectedObjects = target as cameraScenePreview; // sets target script
        if (_connectedObjects.FollowPoints == null)
        {
            return;
        }
    
        var numberOfPoints = _connectedObjects.FollowPoints.Count; // get size of points array
        for (var i = 0; i < numberOfPoints; i++) // for each point
        {
            var connectedObject = _connectedObjects.FollowPoints[i];
            Handles.color = Color.green; // sets the drawn line colour to green
            Handles.DrawLine( // draw line
                i == 0
                    ? _connectedObjects.transform.position // if primary point doesn't exist, set the position to the parent
                    : _connectedObjects.FollowPoints[i - 1].transform.position, connectedObject.transform.position); // if previous points exist, set the point to follow the last point in the array


            if (connectedObject.transform.localRotation.x != 0 || connectedObject.transform.localRotation.y != 0 || connectedObject.transform.localRotation.z != 0) // if point has any rotation
            {
                Handles.color = Color.red; // sets drawn line colour to red
                Handles.DrawLine(connectedObject.transform.position, connectedObject.transform.position + connectedObject.transform.forward * 5); // draws a line to show to the direction of rotation
            }
        }

        for (int i = 1; i < numberOfPoints; i+=2)
        {
            if (i + 2 < numberOfPoints)
            {
                var p0 = Handles.PositionHandle(_connectedObjects.FollowPoints[i].transform.position, Quaternion.identity);
                var p1 = Handles.PositionHandle(_connectedObjects.FollowPoints[i + 1].transform.position, Quaternion.identity);
                var p2 = Handles.PositionHandle(_connectedObjects.FollowPoints[i + 2].transform.position, Quaternion.identity);
                //var p0 = _connectedObjects.FollowPoints[i].transform.position;
                //var p1 = _connectedObjects.FollowPoints[i+1].transform.position;
                //var p2 = _connectedObjects.FollowPoints[i + 2].transform.position;
                //var endTangent = Handles.PositionHandle(_connectedObjects.FollowPoints[i].transform.position, Quaternion.identity);
                Handles.DrawBezier(p0, p2, p0, p1, Color.blue, null, 2f);
            }
        }
    }
    
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update(); // update the serialized object
        _points.DoLayoutList(); // show the re-ordrable list in the inspector
        serializedObject.ApplyModifiedProperties(); // apply properties to serialized object

        if (GUILayout.Button("Add new point")) // adds "add new point" button to inspector
        {
            var newObj = new GameObject(); // creates new game object
            if (_connectedObjects.FollowPoints.Count == 0) // if no points exist
            {
                newObj.transform.parent = GameObject.Find("scenePreview").transform; // set parent 
                newObj.transform.position = GameObject.Find("scenePreview").transform.position; // set pos to parent
            }
            else // if points exist
            {
                newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent; // set parent
                newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                            _connectedObjects.FollowPoints[^1].transform.forward * 5; // set position to in-front of the last point
            }
            _connectedObjects.FollowPoints.Add(newObj); // append to the list
        }
        if (GUILayout.Button("Add new point with inherited rotation")) // adds button to inspector
        {
            var newObj = new GameObject(); // creates new game object
            if (_connectedObjects.FollowPoints.Count == 0) // if no points exist
            {
                newObj.transform.parent = GameObject.Find("scenePreview").transform; // set parent
                newObj.transform.position = GameObject.Find("scenePreview").transform.position; // sets pos to parent
                newObj.transform.localRotation = GameObject.Find("scenePreview").transform.localRotation; // set rotation to the same as parent
            }
            else
            {
                newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent; // set parent
                newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                            _connectedObjects.FollowPoints[^1].transform.forward * 5; // set pos to in-front of the last point
                newObj.transform.rotation = _connectedObjects.FollowPoints[^1].transform.rotation; // set rotation to the same of the last point
            }
            _connectedObjects.FollowPoints.Add(newObj); // append to list
        }

        EditorGUILayout.PropertyField(_moveSpeed); // create field for move speed, custom editor doesn't show this by default
        EditorGUILayout.PropertyField(_rotateSpeed); // create field to rotate speed, same reason as above
        serializedObject.ApplyModifiedProperties(); // apply changes if made
    }
    
    private void OnEnable()
    {
        _listProperty = serializedObject.FindProperty("followPoints"); // find the followPoints list from the target script
        _moveSpeed = serializedObject.FindProperty("moveSpeed"); // finds the move speed from target script
        _rotateSpeed = serializedObject.FindProperty("rotateSpeed"); // finds the rotate speed from target script
        _points = new ReorderableList(serializedObject, _listProperty, true, true, false, false); // show list in inspector
        
        #region ReordableList Draw callback

        _points.drawElementCallback = // draw the list
            (rect, index, active, focused) =>
            {
                if (index > _points.serializedProperty.arraySize -1 ) return; // if array is empty do none of the following
                var element = _points.serializedProperty.GetArrayElementAtIndex(index); // store the array index as var
                
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight), element, GUIContent.none); // draw each entry in list in inspector
                }
                EditorGUI.EndDisabledGroup();
                
                if (GUI.Button(new Rect(rect.x + rect.width - 20, rect.y, 20, EditorGUIUtility.singleLineHeight), "X")) // delete button
                {
                    var obj = (GameObject) element.objectReferenceValue; // highlighted object is stored as var

                    if (obj) // if there is a highlighted object
                    {
                        DestroyImmediate(obj); // destroy the object
                    }

                    if(_points.serializedProperty.GetArrayElementAtIndex(index).objectReferenceValue != null) // if the index / reference is now null
                        _points.serializedProperty.DeleteArrayElementAtIndex(index); // remove from array 
                    _points.serializedProperty.DeleteArrayElementAtIndex(index);

                    serializedObject.ApplyModifiedProperties(); // update array 
                }
            };
        #endregion
    }
}




