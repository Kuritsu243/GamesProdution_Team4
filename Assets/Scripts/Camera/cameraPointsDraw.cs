#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    private SerializedProperty _lineDistanceMultiplier;
    private const string CurvePattern = @"\bCurve\b\([0]\)";
    private const string EndOfCurvePattern = @"\bCurve\b\([3]\)";
    private const string MiddleOf1CurvePattern = @"\bCurve\b\([1]\)";
    private const string MiddleOf2CurvePattern = @"\bCurve\b\([2]\)";
  
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
            var pointName = connectedObject.name;
            if (CheckIfStartOfCurve(pointName) && i+3 < numberOfPoints)
            {
                DrawCurve(_connectedObjects.FollowPoints[i].transform.position, _connectedObjects.FollowPoints[i+1].transform.position, _connectedObjects.FollowPoints[i+2].transform.position, _connectedObjects.FollowPoints[i+3].transform.position);
            }

            if (CheckIfEndOfCurve(pointName) && i+1 < numberOfPoints)
            {
                DrawLine(_connectedObjects.FollowPoints[i].transform, _connectedObjects.FollowPoints[i+1].transform);
            }
            switch (i)
            {
                case 0:
                    DrawLine(_connectedObjects.transform, _connectedObjects.transform);
                    break;
                case > 0 when !CheckIfMiddleOfCurve(pointName) && !CheckIfEndOfCurve(pointName):
                {
                    DrawLine(_connectedObjects.FollowPoints[i-1].transform, connectedObject.transform);
                    if (connectedObject.transform.localRotation.x == 0 && connectedObject.transform.localRotation.y == 0 &&
                        connectedObject.transform.localRotation.z == 0) continue;
                    DrawRotation(connectedObject.transform);
                    break;
                }
            }
 
        }
    }

    private static void DrawCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var startPos = Handles.PositionHandle(p0, quaternion.identity);
        var startTangent = Handles.PositionHandle(p1, quaternion.identity);
        var endPos = Handles.PositionHandle(p3, quaternion.identity);
        var endTangent = Handles.PositionHandle(p2, quaternion.identity);
        
        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.blue, null, 2f);
    }

    private static void DrawLine(Transform p0, Transform p1)
    {
        Handles.color = Color.green;
        var startPos = p0.position;
        var endPos = p1.position;
        Handles.DrawLine(startPos, endPos);
    }

    private static void DrawRotation(Transform p0)
    {
        Handles.color = Color.red;
        var startPos = p0.position;
        var endPos = startPos + p0.forward * 5;
        Handles.DrawLine(startPos, endPos);
    }

    private static bool CheckIfStartOfCurve(string pointName)
    {
        return Regex.Match(pointName, CurvePattern).Value == "Curve(0)";
    }

    private static bool CheckIfEndOfCurve(string pointName)
    {
        return Regex.Match(pointName, EndOfCurvePattern).Value == "Curve(3)";
    }

    private static bool CheckIfMiddleOfCurve(string pointName)
    {
        return Regex.Match(pointName, MiddleOf1CurvePattern).Value == "Curve(1)" || Regex.Match(pointName, MiddleOf2CurvePattern).Value == "Curve(2)";
    }
    
    
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update(); // update the serialized object
        _points.DoLayoutList(); // show the ordable list in the inspector
        serializedObject.ApplyModifiedProperties(); // apply properties to serialized object

        if (GUILayout.Button("Add new point")) // adds "add new point" button to inspector
        {
            var newObj = new GameObject(); // creates new game object
            if (_connectedObjects.FollowPoints.Count == 0) // if no points exist
            {
                newObj.transform.parent = GameObject.Find("scenePreview").transform; // set parent 
                newObj.transform.position = GameObject.Find("scenePreview").transform.position; // set pos to parent
                newObj.name = "Point(0)";
            }
            else // if points exist
            {
                newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent; // set parent
                newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                            _connectedObjects.FollowPoints[^1].transform.forward * (_connectedObjects.transform.localScale.x * _connectedObjects.lineDistanceMultiplier); // set position to in-front of the last point
                newObj.name = $"Point({_connectedObjects.FollowPoints.Count + 1})";
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
                newObj.name = "Point(0)";
            }
            else
            {
                newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent; // set parent
                newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                            _connectedObjects.FollowPoints[^1].transform.forward * (_connectedObjects.transform.localScale.x * _connectedObjects.lineDistanceMultiplier); // set pos to in-front of the last point
                newObj.transform.rotation = _connectedObjects.FollowPoints[^1].transform.rotation; // set rotation to the same of the last point
                newObj.name = $"Point({_connectedObjects.FollowPoints.Count + 1})";
            }
            _connectedObjects.FollowPoints.Add(newObj); // append to list
        }

        if (GUILayout.Button("Add new curve"))
        {
            for (var i = 0; i < 4; i++)
            {
                var newObj = new GameObject();
                if (_connectedObjects.FollowPoints.Count == 0 && i == 0)
                {
                    newObj.transform.parent = GameObject.Find("scenePreview").transform; // set parent
                    newObj.transform.position = GameObject.Find("scenePreview").transform.position; // sets pos to parent
                    newObj.transform.localRotation = GameObject.Find("scenePreview").transform.localRotation; // set rotation to the same as parent
                    newObj.name = $"Point(0),Curve({i})";
                }
                else
                {
                    newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent; // set parent
                    newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                                _connectedObjects.FollowPoints[^1].transform.forward * (_connectedObjects.transform.localScale.x * _connectedObjects.lineDistanceMultiplier); // set pos to in-front of the last point
                    newObj.name = $"Point({_connectedObjects.FollowPoints.Count + 1}),Curve({i})";
                }
                _connectedObjects.FollowPoints.Add(newObj);
            }
        }

        if (GUILayout.Button("Add new curve with inherited rotation"))
        {
            for (var i = 0; i < 4; i++)
            {
                var newObj = new GameObject();
                if (_connectedObjects.FollowPoints.Count == 0 && i == 0)
                {
                    newObj.transform.parent = GameObject.Find("scenePreview").transform; // set parent
                    newObj.transform.position = GameObject.Find("scenePreview").transform.position; // sets pos to parent
                    newObj.transform.localRotation = GameObject.Find("scenePreview").transform.localRotation; // set rotation to the same as parent
                    newObj.name = $"Point(0),Curve({i})";
                }
                else
                {
                    newObj.transform.parent = _connectedObjects.FollowPoints[^1].transform.parent; // set parent
                    newObj.transform.position = _connectedObjects.FollowPoints[^1].transform.position +
                                                _connectedObjects.FollowPoints[^1].transform.forward * (_connectedObjects.transform.localScale.x * _connectedObjects.lineDistanceMultiplier); // set pos to in-front of the last point
                    newObj.transform.localRotation = _connectedObjects.FollowPoints[^1].transform.rotation;
                    newObj.name = $"Point({_connectedObjects.FollowPoints.Count + 1}),Curve({i})";
                }
                _connectedObjects.FollowPoints.Add(newObj);
            }
        }

        EditorGUILayout.PropertyField(_moveSpeed); // create field for move speed, custom editor doesn't show this by default
        EditorGUILayout.PropertyField(_rotateSpeed); // create field to rotate speed, same reason as above
        EditorGUILayout.PropertyField(_lineDistanceMultiplier); // see above
        serializedObject.ApplyModifiedProperties(); // apply changes if made
    }
    
    private void OnEnable()
    {
        _listProperty = serializedObject.FindProperty("followPoints"); // find the followPoints list from the target script
        _moveSpeed = serializedObject.FindProperty("moveSpeed"); // finds the move speed from target script
        _rotateSpeed = serializedObject.FindProperty("rotateSpeed"); // finds the rotate speed from target script
        _lineDistanceMultiplier = serializedObject.FindProperty("lineDistanceMultiplier");
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


#endif

