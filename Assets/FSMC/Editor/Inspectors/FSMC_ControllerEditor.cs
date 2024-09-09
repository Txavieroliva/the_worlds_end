using FSMC;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using FSMC.Editor.Windows;
using FSMC.Editor.Nodes;
using FSMC.Runtime;
using System.Linq;

namespace FSMC.Editor.Inspectors
{
    [CustomEditor(typeof(FSMC_Controller))]
    public partial class FSMC_ControllerEditor : UnityEditor.Editor
    {
        public static bool IsOpen = false;
        VisualElement root;
        SerializedProperty currentProperty;

        private void OnEnable()
        {
            IsOpen = true;
            if (root == null) root = new VisualElement();
            if (currentProperty == null)
            {
                if (FSMC_EditorWindow.Current != null && FSMC_EditorWindow.Current.currentSelection != null && FSMC_EditorWindow.Current.controller == target as FSMC_Controller)
                {
                    OnSelectionChange(FSMC_EditorWindow.Current.currentSelection);
                }
                else
                {
                    OnSelectionChange(null);
                }
            }

            FSMC_EditorWindow.OnSelectionChange += OnSelectionChange;
        }
        private void OnDisable()
        {
            IsOpen = false;
            FSMC_EditorWindow.OnSelectionChange -= OnSelectionChange;
        }

        public override VisualElement CreateInspectorGUI()
        {
            if (root == null) root = new VisualElement();
            return root;
        }

        private VisualElement GenerateControllerInspector()
        {
            var controllerContainer = new VisualElement();

            if (FSMC_EditorWindow.Current == null || FSMC_EditorWindow.Current.controller != serializedObject.targetObject as FSMC_Controller)
            {
                var openButton = new Button(() => AssetDatabase.OpenAsset((serializedObject.targetObject as FSMC_Controller).GetInstanceID()))
                { text = "Open Editor", name = "OpenButton" };
                openButton.style.height = 40;
                controllerContainer.Add(openButton);
            }
            return controllerContainer;
        }
        private VisualElement GenerateAnyInspector()
        {
            var controllerContainer = new VisualElement();

            var _anyListProperty = serializedObject.FindProperty("AnyTransitions");
            var anyList = new ListView((serializedObject.targetObject as FSMC_Controller).AnyTransitions, 25f, () => new VisualElement() { name = "TransitionInfo" }, (element, index) =>
            {
                element.Clear();
                element.Add(new Label(_anyListProperty.GetArrayElementAtIndex(index).FindPropertyRelative("_name").stringValue));
                element.RegisterCallback<MouseDownEvent>(e =>
                {
                    if (e.clickCount == 2)
                    {
                        OnSelectedPropChange(_anyListProperty.GetArrayElementAtIndex(index));
                    }
                });
            })
            {
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showFoldoutHeader = true,
                showBorder = true,
                showBoundCollectionSize = false,
                headerTitle = "Transitions from any state",
                name = "TransitionsList"
            };
            anyList.itemIndexChanged += (i, j) => { serializedObject.ApplyModifiedProperties(); serializedObject.Update(); anyList.Rebuild(); };
            anyList.RegisterCallback<FocusOutEvent>(e => anyList.SetSelection(-1));
            controllerContainer.Add(anyList);
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/FSMC/Editor/Editor Resources/FSMC_StateInspectorStyles.uss");
            controllerContainer.styleSheets.Add(styleSheet);

            return controllerContainer;
        }

        private void OnSelectionChange(GraphElement element)
        {
            serializedObject.Update();
            if (element == null)
            {
                root.Clear();
                root.Add(GenerateControllerInspector());
            }
            else if (element is FSMC_StateNode)
            {
                currentProperty = serializedObject.FindProperty("States")
                    .GetArrayElementAtIndex((serializedObject.targetObject as FSMC_Controller).States.IndexOf((element as FSMC_StateNode).State));
                root.Clear();
                root.Add(GenerateStateInspector());
            }
            else if (element is FSMC_Edge)
            {
                var edge = element as FSMC_Edge;
                currentProperty = serializedObject.FindProperty("States")
                    .GetArrayElementAtIndex((serializedObject.targetObject as FSMC_Controller).States.IndexOf(edge.transition.DestinationState))
                    .FindPropertyRelative("TransitionsTo").GetArrayElementAtIndex(edge.transition.DestinationState.TransitionsTo.IndexOf(edge.transition));
                root.Clear();
                root.Add(GenerateTransitionInspector());
            }
            else if (element is FSMC_AnyNode)
            {
                currentProperty = null;
                root.Clear();
                root.Add(GenerateAnyInspector());
            }
        }
        private void OnSelectedPropChange(SerializedProperty prop)
        {
            serializedObject.Update();
            if (prop == null)
            {
                if (FSMC_EditorWindow.Current != null)
                {
                    FSMC_EditorWindow.Current.graphView.ClearSelection();
                }
                else
                {
                    root.Clear();
                    root.Add(GenerateControllerInspector());
                }
            }
            else if (prop.name == "AnyTransitions")
            {
                if (FSMC_EditorWindow.Current != null)
                {
                    FSMC_EditorWindow.Current.graphView.ClearSelection();
                    FSMC_EditorWindow.Current.graphView.AddToSelection(
                        FSMC_EditorWindow.Current.graphView.nodes.Where(e => e is FSMC_AnyNode).FirstOrDefault()
                    );
                }
                else
                {
                    currentProperty = null;
                    root.Clear();
                    root.Add(GenerateAnyInspector());
                }
            }
            else if (prop.managedReferenceValue is FSMC_State)
            {
                if (FSMC_EditorWindow.Current != null)
                {
                    FSMC_EditorWindow.Current.graphView.ClearSelection();
                    FSMC_EditorWindow.Current.graphView.AddToSelection(
                        FSMC_EditorWindow.Current.graphView.nodes.Where(e => e is FSMC_StateNode).Where(e => (e as FSMC_StateNode).State == prop.managedReferenceValue as FSMC_State).FirstOrDefault()
                    );
                }
                else
                {
                    currentProperty = prop;
                    root.Clear();
                    root.Add(GenerateStateInspector());
                }
            }
            else if (prop.managedReferenceValue is FSMC_Transition)
            {
                if (FSMC_EditorWindow.Current != null)
                {
                    FSMC_EditorWindow.Current.graphView.ClearSelection();
                    FSMC_EditorWindow.Current.graphView.AddToSelection(
                        FSMC_EditorWindow.Current.graphView.edges.Where(e => (e as FSMC_Edge).transition == prop.managedReferenceValue as FSMC_Transition).FirstOrDefault()
                    );
                }
                else
                {
                    currentProperty = prop;
                    root.Clear();
                    root.Add(GenerateTransitionInspector());
                }
            }
        }
    }
}
