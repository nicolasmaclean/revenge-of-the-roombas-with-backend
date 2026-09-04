using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Player
{
    //[CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : UnityEditor.Editor
    {
        SerializedProperty _moveSpeedProperty;
        SerializedProperty _horizontalAxisProperty;
        SerializedProperty _verticalAxisProperty;
        SerializedProperty _OnMoveProperty;

        SerializedProperty _dashDistanceProperty;
        SerializedProperty _dashCooldownProperty;
        SerializedProperty _dashAxisProperty;
        SerializedProperty _OnDashProperty;

        SerializedProperty _punchColliderProperty;
        SerializedProperty _punchAxisProperty;
        SerializedProperty _OnPunchProperty;

        SerializedProperty _counterAxisProperty;
        SerializedProperty _OnCounterProperty;

        SerializedProperty _alignMovementToCameraProperty;
        SerializedProperty _followCameraProperty;

        SerializedProperty _animatorProperty;

        public void OnEnable()
        {
            _moveSpeedProperty = serializedObject.FindProperty(nameof(PlayerController.moveSpeed));
            _horizontalAxisProperty = serializedObject.FindProperty(nameof(PlayerController.horizontalAxis));
            _verticalAxisProperty = serializedObject.FindProperty(nameof(PlayerController.verticalAxis));
            _OnMoveProperty = serializedObject.FindProperty(nameof(PlayerController.OnMove));

            _dashDistanceProperty = serializedObject.FindProperty(nameof(PlayerController.dashForce));
            _dashCooldownProperty = serializedObject.FindProperty(nameof(PlayerController.dashCooldown));
            _dashAxisProperty = serializedObject.FindProperty(nameof(PlayerController.dashAxis));
            _OnDashProperty = serializedObject.FindProperty(nameof(PlayerController.OnDash));

            _punchColliderProperty = serializedObject.FindProperty(nameof(PlayerController.punchCollider));
            _punchAxisProperty = serializedObject.FindProperty(nameof(PlayerController.punchAxis));
            _OnPunchProperty = serializedObject.FindProperty(nameof(PlayerController.OnPunch));

            _counterAxisProperty = serializedObject.FindProperty(nameof(PlayerController.counterAxis));
            _OnCounterProperty = serializedObject.FindProperty(nameof(PlayerController.OnCounter));

            _alignMovementToCameraProperty = serializedObject.FindProperty(nameof(PlayerController.alignMovementToCamera));
            _followCameraProperty = serializedObject.FindProperty(nameof(PlayerController.followCamera));

            _animatorProperty = serializedObject.FindProperty(nameof(PlayerController.animator));
        }

        public override void OnInspectorGUI()
        {
            // controls
            EditorGUILayout.PropertyField(_moveSpeedProperty);
            EditorGUILayout.PropertyField(_horizontalAxisProperty);
            EditorGUILayout.PropertyField(_verticalAxisProperty);
            EditorGUILayout.PropertyField(_OnMoveProperty);

            EditorGUILayout.PropertyField(_dashDistanceProperty);
            EditorGUILayout.PropertyField(_dashCooldownProperty);
            EditorGUILayout.PropertyField(_dashAxisProperty);
            EditorGUILayout.PropertyField(_OnDashProperty);

            EditorGUILayout.PropertyField(_punchColliderProperty);
            EditorGUILayout.PropertyField(_punchAxisProperty);
            EditorGUILayout.PropertyField(_OnPunchProperty);

            EditorGUILayout.PropertyField(_counterAxisProperty);
            EditorGUILayout.PropertyField(_OnCounterProperty);

            // camera
            EditorGUILayout.PropertyField(_alignMovementToCameraProperty);
            if (_alignMovementToCameraProperty.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(_followCameraProperty);
                EditorGUI.indentLevel -= 1;
            }

            //EditorGUILayout.PropertyField(_animatorProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}