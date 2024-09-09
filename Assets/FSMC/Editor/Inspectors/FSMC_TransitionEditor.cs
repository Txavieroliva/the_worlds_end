using FSMC;
using FSMC.Editor.Windows;
using FSMC.Runtime;
using System;
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
        private List<FSMC_ConditionWrapper> alternatives;


        private VisualElement GenerateTransitionInspector()
        {
            alternatives = (currentProperty.managedReferenceValue as FSMC_Transition).conditions;

            var transitionContainer = new VisualElement();

            var directionBox = new VisualElement() { name = "DirectionBox" };
            var originLabel = new Label();
            originLabel.focusable = true;
            if (currentProperty.FindPropertyRelative("_originState").managedReferenceValue == null)
            {
                originLabel.text = "Any";
                originLabel.RegisterCallback<MouseDownEvent>(e => { if (e.clickCount == 2) OnSelectedPropChange(serializedObject.FindProperty("AnyTransitions")); });
            }
            else
            {
                originLabel.text = currentProperty.FindPropertyRelative("_originState").FindPropertyRelative("_name").stringValue;
                originLabel.RegisterCallback<MouseDownEvent>(e => { if (e.clickCount == 2) OnSelectedPropChange(currentProperty.FindPropertyRelative("_originState")); });
            }
            var arrowLabel = new Label("->");
            var destinationLabel = new Label(currentProperty.FindPropertyRelative("_destinationState").FindPropertyRelative("_name").stringValue);
            destinationLabel.RegisterCallback<MouseDownEvent>(e => { if (e.clickCount == 2) OnSelectedPropChange(currentProperty.FindPropertyRelative("_destinationState")); });

            directionBox.Add(originLabel);
            directionBox.Add(arrowLabel);
            directionBox.Add(destinationLabel);
            transitionContainer.Add(directionBox);

            for (int i = 0; i < alternatives.Count; i++)
            {
                int outerIndex = i;
                var alt = alternatives[outerIndex];
                var conditions = alt.conditions;

                var alternativeElement = new VisualElement();
                alternativeElement.AddToClassList("list-container");
                var listTopBar = new VisualElement();
                listTopBar.AddToClassList("list-top-bar");
                alternativeElement.Add(listTopBar);
                listTopBar.Add(new Label(i == 0 ? "" : "OR"));



                var listView = new ListView(conditions, 30, MakeItem, (el, ind) => this.BindItem(el, ind, conditions[ind], alternatives.IndexOf(alt)));
                listView.reorderable = true;
                listView.reorderMode = ListViewReorderMode.Animated;
                alternativeElement.Add(listView);

                var buttonsContainer = new VisualElement() { name = "ButtonsContainer" };
                var dropdownMenu = new GenericDropdownMenu();
                foreach (var param in (target as FSMC_Controller).Parameters)
                {
                    dropdownMenu.AddItem(param.Name, false, () => AddCondition(param));
                }
                var dropdownButton = new Button(() => dropdownMenu.DropDown(buttonsContainer.worldBound, buttonsContainer, true));
                dropdownButton.Add(new Image() { image = EditorGUIUtility.FindTexture("Toolbar Plus") });
                dropdownButton.Add(new Image() { image = EditorGUIUtility.FindTexture("Icon Dropdown") });
                var minusButton = new Button(() => RemoveCondition(listView.selectedIndex));
                minusButton.Add(new Image() { image = EditorGUIUtility.FindTexture("Toolbar Minus") });
                buttonsContainer.Add(dropdownButton);
                buttonsContainer.Add(minusButton);
                var optionsButton = new Button(() =>
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, () => RemoveAlternative(outerIndex));
                    menu.ShowAsContext();
                })
                { name = "OptionsButton" };
                buttonsContainer.Add(optionsButton);
                listTopBar.Add(buttonsContainer);

                listView.RegisterCallback<FocusOutEvent>(e => { if (e.relatedTarget != minusButton) listView.SetSelection(-1); });
                transitionContainer.Add(alternativeElement);


                void AddCondition(FSMC_Parameter param)
                {
                    Undo.RecordObject(serializedObject.targetObject as FSMC_Controller, "Add condition");
                    if (param.Type == FSMC_ParameterType.Integer)
                    {
                        conditions.Add(new FSMC_IntegerCondition() { parameter = param as FSMC_IntegerParameter });
                    }
                    else if (param.Type == FSMC_ParameterType.Float)
                    {
                        conditions.Add(new FSMC_FloatCondition() { parameter = param as FSMC_FloatParameter });
                    }
                    else if (param.Type == FSMC_ParameterType.Bool)
                    {
                        conditions.Add(new FSMC_BoolCondition() { parameter = param as FSMC_BoolParameter });
                    }
                    else if (param.Type == FSMC_ParameterType.Trigger)
                    {
                        conditions.Add(new FSMC_BoolCondition() { parameter = param as FSMC_BoolParameter, comparison = ComparisonType.Equal, Value = true });
                    }
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    listView.Rebuild();
                }
                void RemoveCondition(int index)
                {
                    if (index < 0) return;
                    Undo.RecordObject(serializedObject.targetObject as FSMC_Controller, "Remove condition");
                    conditions.RemoveAt(index);
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                    listView.Rebuild();
                }
            }

            var orButton = new Button(AddAlternative) { text = "ADD OR" };
            transitionContainer.Add(orButton);

            transitionContainer.AddToClassList("main-container");
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/FSMC/Editor/Editor Resources/FSMC_TransitionStyles.uss");
            transitionContainer.styleSheets.Add(styleSheet);

            return transitionContainer;
        }

        private void AddAlternative()
        {
            Undo.RecordObject(serializedObject.targetObject as FSMC_Controller, "Add alternative");
            var wrap = new FSMC_ConditionWrapper() { conditions = new() };
            alternatives.Add(wrap);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            root.Clear();
            root.Add(GenerateTransitionInspector());
        }
        private void RemoveAlternative(int index)
        {
            Undo.RecordObject(serializedObject.targetObject as FSMC_Controller, "Remove alternative");
            alternatives.RemoveAt(index);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            root.Clear();
            root.Add(GenerateTransitionInspector());
        }

        VisualElement MakeItem()
        {
            var item = new VisualElement();
            item.AddToClassList("list-item");
            return item;
        }

        void BindItem(VisualElement element, int index, FSMC_Condition con, int outerIndex)
        {
            element.Clear();
            SerializedProperty prop = currentProperty.FindPropertyRelative("conditions").
                    GetArrayElementAtIndex(outerIndex).FindPropertyRelative("conditions").GetArrayElementAtIndex(index);
            if (con is FSMC_IntegerCondition)
            {
                element.Add(new Label((con as FSMC_IntegerCondition).parameter.Name));
                var comparison = new EnumField(ComparisonType.Equal);
                element.Add(comparison);
                comparison.BindProperty(prop.FindPropertyRelative("comparison"));
                var input = new IntegerField();
                element.Add(input);
                input.BindProperty(prop.FindPropertyRelative("Value"));
            }
            else if (con is FSMC_FloatCondition)
            {
                element.Add(new Label((con as FSMC_FloatCondition).parameter.Name));
                var comparison = new EnumField(ComparisonType.Equal);
                element.Add(comparison);
                comparison.BindProperty(prop.FindPropertyRelative("comparison"));
                var input = new FloatField();
                element.Add(input);
                input.BindProperty(prop.FindPropertyRelative("Value"));
            }
            else if (con is FSMC_BoolCondition)
            {
                element.Add(new Label((con as FSMC_BoolCondition).parameter.Name));
                if ((con as FSMC_BoolCondition).parameter.Type != FSMC_ParameterType.Trigger)
                {
                    var comparison = new EnumField(ComparisonType.Equal);
                    element.Add(comparison);
                    comparison.BindProperty(prop.FindPropertyRelative("comparison"));
                    var input = new Toggle();
                    element.Add(input);
                    input.BindProperty(prop.FindPropertyRelative("Value"));
                }
            }
        }
    }
}