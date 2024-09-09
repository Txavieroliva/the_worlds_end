using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using FSMC.Editor.Nodes;
using FSMC.Runtime;

namespace FSMC.Editor.Windows
{
    public class FSMC_EditorWindow : EditorWindow
    {
        public static FSMC_EditorWindow Current;
        private GraphElement _currSel = null;
        public GraphElement currentSelection { get=>_currSel; set{
                //if (_currSel == value) return;
                if (graphView.selection.Count < 2)
                {
                    _currSel = value;
                    Selection.activeObject = controller; 
                    OnSelectionChange?.Invoke(value);
                }
            } }
        public static event Action<GraphElement> OnSelectionChange;

        public FSMC_Controller controller;
        public FSMC_GraphView graphView;

        private TextField search;
        private ListView listView;
        private List<FSMC_Parameter> filteredParams;
        private TwoPaneSplitView splitScreen;

        public void Refresh() => OnSelectionChange?.Invoke(currentSelection);
        private void OnEnable()
        {
            Current = this;
        }
        private void OnDisable()
        {
            Current = null;
        }

        public void CreateGUI()
        {
            #region SafeCheck
            VisualElement root = rootVisualElement;
            root.Clear();
            if (graphView == null)
            {
                if (controller != null) graphView = new FSMC_GraphView(controller);
                else return;
            }else if(controller == null)
            {
                if (graphView.Controller != null) controller = graphView.Controller;
                else return;
            }
            #endregion
            wantsMouseMove = true;
            splitScreen = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            root.Add(splitScreen);

            splitScreen.Add(CreateSideBar());
            splitScreen.Add(CreateGraphView());

            AddStyles();

            Undo.undoRedoPerformed += OnUndoRedo;
        }
        private GraphElement oldSel;
        private void OnUndoRedo()
        {
            oldSel = currentSelection;
            rootVisualElement.Q<VisualElement>(name:"GraphContainer").Clear();
            var oldPos = graphView.viewTransform;
            graphView = new FSMC_GraphView(controller);
            graphView.viewTransform.position = oldPos.position;
            graphView.viewTransform.scale = oldPos.scale;
            graphView.StretchToParentSize();
            rootVisualElement.Q<VisualElement>(name:"GraphContainer").Add(graphView);
            

            listView.Rebuild();
        }
        void OnGUI()
        {
            if (graphView != null) graphView.mousePos = Event.current.mousePosition;
            if(oldSel != null)
            {
                if (oldSel is FSMC_Edge) graphView.AddToSelection(graphView.edges.Where(e => (e as FSMC_Edge).transition == (oldSel as FSMC_Edge).transition).First());
                else if (oldSel is FSMC_StateNode) graphView.AddToSelection(graphView.nodes.Where(e=>e is FSMC_StateNode).Where(e => (e as FSMC_StateNode).State == (oldSel as FSMC_StateNode).State).First());
                oldSel = null;
            }
        }

        private VisualElement CreateSideBar()
        {

            Func<VisualElement> makeItem = () => new FSMC_ParameterDisplay();
            Action<VisualElement, int> bindItem = (e, i) => BindItem(e as FSMC_ParameterDisplay, i);

            listView = new ListView(controller.Parameters, 35, makeItem, bindItem);
            listView.reorderable = true;
            listView.reorderMode = ListViewReorderMode.Animated;
            
            listView.RegisterCallback<KeyDownEvent>(e => { if (e.keyCode == KeyCode.Delete) DeleteParameter(listView.selectedIndex); });
            listView.RegisterCallback<FocusOutEvent>(e => listView.SetSelection(-1));

            VisualElement listContainer = new();
            listContainer.style.width = 150;

            VisualElement topBar = new VisualElement() { name = "TopBar"};
            search = new() {name="SearchField"};
            search.SetPlaceholderText("search...");
            search.RegisterValueChangedCallback(e=>OnSearchChange(e));

            var dropdownMenu = new GenericDropdownMenu();
            dropdownMenu.AddItem("Add Int", false, () => AddParameter("Int", FSMC_ParameterType.Integer, 0));
            dropdownMenu.AddItem("Add Float", false, () => AddParameter("Float", FSMC_ParameterType.Float, 0f));
            dropdownMenu.AddItem("Add Bool", false, () => AddParameter("Bool", FSMC_ParameterType.Bool, false));
            dropdownMenu.AddItem("Add Trigger", false, () => AddParameter("Trigger", FSMC_ParameterType.Trigger, false));

            var dropdownButton = new VisualElement();
            dropdownButton.focusable = true;
            dropdownButton.AddToClassList("CustomButton");
            dropdownButton.RegisterCallback<PointerDownEvent>(e => dropdownMenu.DropDown(topBar.worldBound, topBar, true));
            dropdownButton.Add(new Image() { image = EditorGUIUtility.FindTexture("Toolbar Plus") });
            dropdownButton.Add(new Image() { image = EditorGUIUtility.FindTexture("Icon Dropdown") });

            topBar.Add(search);
            topBar.Add(dropdownButton);

            listContainer.Add(topBar);
            listContainer.Add(listView);

            return listContainer;
        }

        private void OnSearchChange(ChangeEvent<string> e)
        {
            string placeholderClass = TextField.ussClassName + "__placeholder";
            if(string.IsNullOrEmpty(e.newValue) || search.ClassListContains(placeholderClass))
            {
                filteredParams = null;
                listView.itemsSource = controller.Parameters;
                listView.reorderable = true;
                listView.Rebuild();
            }
            else
            {
                filteredParams = controller.Parameters.Where(p => p.Name.ToLower().Contains(e.newValue.ToLower())).ToList();
                listView.itemsSource = filteredParams;
                listView.reorderable = false;
                listView.Rebuild();
            }
        }

        private VisualElement CreateGraphView()
        {
            graphView.StretchToParentSize();
            VisualElement graphContainer = new VisualElement() { name = "GraphContainer"};
            graphContainer.Add(graphView);
            return graphContainer;
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/FSMC/Editor/Editor Resources/FSMC_WindowStyles.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        #region ListView delegates
        private void BindItem(FSMC_ParameterDisplay elem, int i)
        {
            FSMC_Parameter item;
            SerializedProperty prop;

            if (filteredParams == null)
            {
                item = controller.Parameters[i];
                prop = new SerializedObject(controller).FindProperty("Parameters").GetArrayElementAtIndex(i);
                elem.RemoveFromClassList("filtered");
                elem.SetLabel(false);
                elem.Q<TextField>(name: "paramName").focusable = true;
                elem.Q<TextField>(name: "paramName").BindProperty(prop.FindPropertyRelative("_name"));
            }
            else
            {
                item = filteredParams[i];
                prop = new SerializedObject(controller).FindProperty("Parameters").GetArrayElementAtIndex(controller.Parameters.IndexOf(item));
                elem.AddToClassList("filtered");
                elem.SetLabel(true);
                elem.Q<Label>(name: "paramName").focusable = false;
                int startIndex = item.Name.ToLower().IndexOf(search.text.ToLower());
                int endindex = startIndex+3+search.text.Length;
                elem.Q<Label>(name: "paramName").text = item.Name.Insert(startIndex, "<b>").Insert(endindex, "</b>");
            }


            elem.type = item.Type;
            elem.RedrawValue();
            switch (item.Type)
            {
                case FSMC_ParameterType.Integer:
                    elem.Q<IntegerField>(name: "IntegerValue").BindProperty(prop.FindPropertyRelative("_value"));
                    elem.Q<IntegerField>(name: "IntegerValue").value = (item as FSMC_IntegerParameter).Value;
                    break;
                case FSMC_ParameterType.Float:
                    elem.Q<FloatField>(name: "FloatValue").bindingPath = "_value";
                    elem.Q<FloatField>(name: "FloatValue").BindProperty(prop.FindPropertyRelative("_value"));
                    elem.Q<FloatField>(name: "FloatValue").value = (item as FSMC_FloatParameter).Value;
                    break;
                case FSMC_ParameterType.Bool:
                    elem.Q<Toggle>(name: "BoolValue").bindingPath = "_value";
                    elem.Q<Toggle>(name: "BoolValue").BindProperty(prop.FindPropertyRelative("_value"));
                    elem.Q<Toggle>(name: "BoolValue").value = (item as FSMC_BoolParameter).Value;
                    break;
                case FSMC_ParameterType.Trigger:
                    elem.Q<Toggle>(name: "TriggerValue").bindingPath = "_value";
                    elem.Q<Toggle>(name: "TriggerValue").BindProperty(prop.FindPropertyRelative("_value"));
                    elem.Q<Toggle>(name: "TriggerValue").value = (item as FSMC_BoolParameter).Value;
                    break;
            };
            elem.parent.AddManipulator(new ContextualMenuManipulator(
                e => e.menu.AppendAction("Delete", menuEvent => DeleteParameter(i)))
            );
        }
        private void DeleteParameter(int index)
        {
            IEnumerable<FSMC_ConditionWrapper> conditions = controller.States.SelectMany(s => s.TransitionsTo).SelectMany(t => t.conditions);
            int count = conditions.SelectMany(c => c.conditions).Where(c => c.parameter == (filteredParams == null ? controller.Parameters[index] : filteredParams[index])).Count();
            if(count > 0)
            {
                if(!EditorUtility.DisplayDialog("Remove parameter?", "Removing this parameter will also remove "+count+" transition conditions. Do you want to remove it?", 
                    "Yes", "No", DialogOptOutDecisionType.ForThisSession, "FSMC.RemoveParameterDecision")) return;
            }
            Undo.RecordObject(controller, "Remove parameter");
            if (count > 0) {
                foreach (FSMC_ConditionWrapper condition in conditions)
                {
                    for (int i = condition.conditions.Count - 1; i >= 0; i--)
                        if ((filteredParams == null && condition.conditions[i].parameter == controller.Parameters[index]) || (filteredParams != null && condition.conditions[i].parameter == filteredParams[index]))
                            condition.conditions.RemoveAt(i);
                }
            }
            if (filteredParams == null) controller.Parameters.RemoveAt(index);
            else { controller.Parameters.Remove(filteredParams[index]); filteredParams.RemoveAt(index); }
            listView.RefreshItems();
            EditorUtility.SetDirty(controller);
        }
        private void AddParameter(string name, FSMC_ParameterType type, object value)
        {
            Undo.RecordObject(controller, "Add parameter");
            controller.Parameters.Add(FSMC_Parameter.CreateParameter(name, type, value));
            listView.RefreshItems();
            EditorUtility.SetDirty(controller);
            OnSelectionChange?.Invoke(currentSelection);
        }
        #endregion

        #region Opening
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            FSMC_Controller controller = EditorUtility.InstanceIDToObject(instanceID) as FSMC_Controller;
            if (controller == null) return false;

            OpenWindow(controller);
            return true;
        }
        public static FSMC_EditorWindow OpenWindow(FSMC_Controller contr)
        {
            FSMC_EditorWindow wnd = GetWindow<FSMC_EditorWindow>(contr.name ,true, typeof(SceneView));
            wnd.titleContent.text = contr.name;
            Current = wnd;
            wnd.minSize = new Vector2(600, 400);
            wnd.graphView = null;
            wnd.controller = contr;
            wnd.CreateGUI();
            return wnd;
        }
            #endregion
    }

}
