using FSMC;
using FSMC.Editor.Windows;
using FSMC.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSMC.Editor.Inspectors
{
    public partial class FSMC_ControllerEditor
    {
        private SerializedProperty _behavioursProperty;
        private SerializedProperty _transitionsFrom;
        private SerializedProperty _transitionsTo;



        private VisualElement GenerateStateInspector()
        {
            var stateContainer = new VisualElement();

            _behavioursProperty = currentProperty.FindPropertyRelative("_behaviours");
            _transitionsFrom = currentProperty.FindPropertyRelative("TransitionsFrom");
            _transitionsTo = currentProperty.FindPropertyRelative("TransitionsTo");

            var stateName = new Label(currentProperty.FindPropertyRelative("_name").stringValue) { name = "StateName" };
            stateName.BindProperty(currentProperty.FindPropertyRelative("_name"));
            stateContainer.Add(stateName);

            var fromList = new ListView((currentProperty.managedReferenceValue as FSMC_State).TransitionsFrom, 25f, () => new VisualElement() { name = "TransitionInfo" }, (element, index) =>
            {
                element.Clear();
                element.Add(new Label(_transitionsFrom.GetArrayElementAtIndex(index).FindPropertyRelative("_name").stringValue));
                element.RegisterCallback<MouseDownEvent>(e =>
                {
                    if (e.clickCount == 2)
                    {
                        OnSelectedPropChange(_transitionsFrom.GetArrayElementAtIndex(index));
                    }
                });
            })
            {
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showFoldoutHeader = true,
                showBorder = true,
                showBoundCollectionSize = false,
                headerTitle = "Transitions from here",
                name = "TransitionsList"
            };
            fromList.itemIndexChanged += (i, j) => { serializedObject.ApplyModifiedProperties(); serializedObject.Update(); fromList.Rebuild(); };
            fromList.RegisterCallback<FocusOutEvent>(e => fromList.SetSelection(-1));
            stateContainer.Add(fromList);

            var toList = new ListView((currentProperty.managedReferenceValue as FSMC_State).TransitionsTo, 25f, () => new VisualElement() { name = "TransitionInfo" }, (element, index) =>
            {
                element.Clear();
                element.Add(new Label(_transitionsTo.GetArrayElementAtIndex(index).FindPropertyRelative("_name").stringValue));
                element.RegisterCallback<MouseDownEvent>(e =>
                {
                    if (e.clickCount == 2)
                    {
                        OnSelectedPropChange(_transitionsTo.GetArrayElementAtIndex(index));
                    }
                });
            })
            {
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showFoldoutHeader = true,
                showBorder = true,
                showBoundCollectionSize = false,
                headerTitle = "Transitions to here",
                name = "TransitionsList"
            };
            toList.itemIndexChanged += (i, j) => { serializedObject.ApplyModifiedProperties(); serializedObject.Update(); toList.Rebuild(); };
            toList.RegisterCallback<FocusOutEvent>(e => fromList.SetSelection(-1));
            stateContainer.Add(toList);

            // Create a VisualElement for each behaviour
            for (int i = 0; i < _behavioursProperty.arraySize; i++)
            {
                var behaviour = _behavioursProperty.GetArrayElementAtIndex(i);

                var foldout = new Foldout();
                var foldSpace = foldout.Q(className: "unity-base-field__input");
                foldSpace.name = "BehaviourFold";
                var enabled = new PropertyField(behaviour.FindPropertyRelative("enabled"), "");
                foldSpace.Add(enabled);
                enabled.BindProperty(behaviour.FindPropertyRelative("enabled"));
                foldSpace.Add(new Image() { image = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D });
                foldSpace.Add(new Label() { text = behaviour.managedReferenceValue.GetType().Name });
                int j = i;
                var opt = new Button(() => ShowMenuForBehaviour(j)) { name = "OptionsButton" };
                var optContainer = new VisualElement() { name = "OptionsContainer" };
                optContainer.Add(opt);
                foldSpace.Add(optContainer);

                foreach(var prop in behaviour.GetChildren(1))
                {
                    var p = new PropertyField(prop);
                    foldout.Add(p);
                    p.BindProperty(prop);
                }

                stateContainer.Add(foldout);
            }


            // Create a button to add new behaviours
            var dropdownMenu = new GenericDropdownMenu();
            var types = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(FSMC_Behaviour).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);
            foreach (var type in types)
            {
                dropdownMenu.AddItem(type.Name, false, () => AddBehaviourOfType(type));
            }

            var addButton = new Button(() => dropdownMenu.DropDown(root.worldBound, root, true)) { text = "Add Behaviour" };
            stateContainer.Add(addButton);

            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/FSMC/Editor/Editor Resources/FSMC_StateInspectorStyles.uss");
            stateContainer.styleSheets.Add(styleSheet);

            return stateContainer;
        }

        private void ShowMenuForBehaviour(int index)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () => RemoveBehaviour(index));
            menu.ShowAsContext();
        }
        private void RemoveBehaviour(int index)
        {
            Undo.RecordObject(serializedObject.targetObject as FSMC_Controller, "Remove FSMC Behaviour");
            var behaviour = _behavioursProperty.GetArrayElementAtIndex(index).managedReferenceValue as FSMC_Behaviour;
            _behavioursProperty.DeleteArrayElementAtIndex(index);
            serializedObject.ApplyModifiedProperties();
            root.Clear();
            root.Add(GenerateStateInspector());
        }

        private void AddBehaviourOfType(System.Type type)
        {
            Undo.RecordObject(serializedObject.targetObject as FSMC_Controller, "Add FSMC Behaviour");
            var behaviour = Activator.CreateInstance(type) as FSMC_Behaviour;

            _behavioursProperty.arraySize++;
            _behavioursProperty.GetArrayElementAtIndex(_behavioursProperty.arraySize - 1).managedReferenceValue = behaviour;

            serializedObject.ApplyModifiedProperties();
            root.Clear();
            root.Add(GenerateStateInspector());
        }

    }
}

