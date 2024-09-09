using FSMC.Editor.Windows;
using FSMC.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSMC.Editor.Nodes
{
    public class FSMC_StateNode : FSMC_BaseNode
    {
        public FSMC_State State;
        private Port inputPort;
        private Port outputPort;

        public FSMC_StateNode(FSMC_State state, FSMC_Controller controller) : base(state.Name, state.Position, controller)
        {
            State = state;
            AddRenameButton();
        }
        public FSMC_StateNode(string stateName, FSMC_Controller controller, Vector2 pos) : base(stateName, pos, controller)
        {
            State = new(stateName);
            Undo.RecordObject(controller, "Add state");
            controller.States.Add(State);
            State.Position = pos;
            AddRenameButton();
        }

        public void SetAsStart(FSMC_Controller controller)
        {
            controller.StartingState = State;
            var graph = GetFirstAncestorOfType<FSMC_GraphView>();
            graph.AddElement(graph.Q<FSMC_StartNode>().Q<Port>(className: "output").ConnectTo<FSMC_Edge>(inputPort));
        }

        public override void Draw()
        {
            base.Draw();

            outputPort = Port.Create<FSMC_Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            outputPort.AddToClassList("InvisPort");
            inputPort = Port.Create<FSMC_Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.AddToClassList("InvisPort");
            Insert(0,outputPort);
            Insert(0,inputPort);
            AddContextTransition(outputPort, IndexOf(outputPort), "Create Transition");
        }
        private void AddRenameButton()
        {
            this.Q<TextField>(name: "NodeName").RegisterValueChangedCallback(UpdateNames);
            this.Q<TextField>(name: "NodeName")
                .BindProperty(new SerializedObject(_controller).FindProperty("States").GetArrayElementAtIndex(_controller.States.IndexOf(State)).FindPropertyRelative("_name"));
            this.AddManipulator(new ContextualMenuManipulator(
                menuEvent => {
                    menuEvent.menu.AppendAction("Rename",
                    (e) =>
                    {
                        this.Q<TextField>(name: "NodeName").SetEnabled(true);
                        this.Q<TextField>(name: "NodeName").Focus();
                        this.Q<TextField>(name: "NodeName").RegisterCallback<FocusOutEvent>(DefocusText);
                    }, DropdownMenuAction.AlwaysEnabled);
                }));
            this.RegisterCallback<MouseDownEvent>(e => { 
                if(e.clickCount == 2)
                {
                    this.Q<TextField>(name: "NodeName").SetEnabled(true);
                    this.Q<TextField>(name: "NodeName").Focus();
                    this.Q<TextField>(name: "NodeName").RegisterCallback<FocusOutEvent>(DefocusText);
                }
            });
        }
        private void UpdateNames(ChangeEvent<string> v)
        {
            State.TransitionsFrom.ForEach(t => t.Name = t.Name.Replace(v.previousValue+"->", v.newValue+"->"));
            State.TransitionsTo.ForEach(t => t.Name = t.Name.Replace("->"+v.previousValue, "->"+v.newValue));
        }
        private void DefocusText(FocusOutEvent e)
        {
            this.Q<TextField>(name: "NodeName").SelectAll();
            this.Q<TextField>(name: "NodeName").SetEnabled(false);
            this.Q<TextField>(name: "NodeName").UnregisterCallback<FocusOutEvent>(DefocusText);
        }
        public override void OnSelected()
        {
            base.OnSelected();
            if (FSMC_EditorWindow.Current == null) return;
            FSMC_EditorWindow.Current.currentSelection = this;
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            if (FSMC_EditorWindow.Current == null) return;
            if (FSMC_EditorWindow.Current.currentSelection == this) FSMC_EditorWindow.Current.currentSelection = null;
        }
    }
}
