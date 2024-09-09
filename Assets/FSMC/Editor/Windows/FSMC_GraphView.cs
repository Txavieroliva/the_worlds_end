using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using FSMC.Runtime;

namespace FSMC.Editor.Windows
{
    using Nodes;
    using System.Linq;

    public partial class FSMC_GraphView : GraphView
    {
        public FSMC_Controller Controller { get; private set; }
        public FSMC_StartNode Start { get; private set; }
        public Vector2 mousePos;

        public FSMC_GraphView(FSMC_Controller controller)
        {
            Controller = controller;

            AddManipulators();

            AddBackgound();

            InitializeNodes();

            AddStyles();

            graphViewChanged += OnGraphViewChange;

            serializeGraphElements += SerializeElements;

            canPasteSerializedData += CanPaste;

            unserializeAndPaste += DeserializeElements;

        }

        private void InitializeNodes()
        {
            Start = new FSMC_StartNode(Controller.StartPosition);
            var any = new FSMC_AnyNode(Controller.AnyPosition);
            AddElement(Start);
            AddElement(any);

            List<FSMC_StateNode> states = new List<FSMC_StateNode>();

            foreach(FSMC_State state in Controller.States)
            {
                var node = new FSMC_StateNode(state, Controller);
                states.Add(node);
                AddElement(node);
            }

            foreach(FSMC_Transition transition in Controller.AnyTransitions)
            {
                var edge = any.Q<Port>(className: "output").ConnectTo<FSMC_Edge>(states.Single(s => s.NodeName == transition.DestinationState.Name).Q<Port>(className: "input"));
                edge.transition = transition;
                AddElement(edge);
            }
            foreach(var state in states)
            {
                foreach(var transition in state.State.TransitionsTo)
                {
                    if(transition.OriginState == null && transition.Name.StartsWith("Any->"))
                    {
                        var edge = any.Q<Port>(className: "output").ConnectTo<FSMC_Edge>(state.Q<Port>(className: "input"));
                        edge.transition = transition;
                        AddElement(edge);
                    }
                    else
                    {
                        var edge = states.Single(s => s.NodeName == transition.OriginState.Name).Q<Port>(className: "output").ConnectTo<FSMC_Edge>(state.Q<Port>(className: "input"));
                        edge.transition = transition;
                        AddElement(edge);
                    }
                }
            }
            if (Controller.StartingState != null)
            {
                AddElement(Start.Q<Port>(className: "output").ConnectTo<FSMC_Edge>(states.Single(s=>s.NodeName==Controller.StartingState.Name).Q<Port>(className:"input")));
            }
        }
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(CreateContextMenu());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentDragger());
        }
        private void AddBackgound()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("Assets/FSMC/Editor/Editor Resources/FSMC_GraphViewStyles.uss");
            styleSheets.Add(styleSheet);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port) return;
                if (startPort.node == port.node) return;
                if (startPort.direction == port.direction) return;
                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }

        private IManipulator CreateContextMenu()
        {
            ContextualMenuManipulator menu = new(
                menuEvent => {
                    menuEvent.menu.AppendAction(
                    "Create State", actionEvent =>
                    {
                        var window = new CreateStatePopup(s => CreateNode(contentViewContainer.WorldToLocal(actionEvent.eventInfo.mousePosition), s));
                        UnityEditor.PopupWindow.Show(new Rect(actionEvent.eventInfo.mousePosition, Vector2.zero), window);
                        window.editorWindow.rootVisualElement.Q<TextField>().Q<VisualElement>(name: "unity-text-input").Focus();
                    });
                }
                );
            return menu;
        }

        private FSMC_StateNode CreateNode(Vector2 pos, string name)
        {
            var node = new FSMC_StateNode(name, Controller, pos);
            AddElement(node);
            if (Controller.States.Count == 1)
                node.SetAsStart(Controller);
            return node;
        }

    }

}
