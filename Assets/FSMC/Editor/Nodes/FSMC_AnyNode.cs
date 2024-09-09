using FSMC.Editor.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace FSMC.Editor.Nodes
{
    public class FSMC_AnyNode : FSMC_BaseNode
    {
        public FSMC_AnyNode(Vector2 pos) : base("Any", pos)
        {
            
        }

        public override void Draw()
        {
            base.Draw();

            Port port = Port.Create<FSMC_Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            port.AddToClassList("InvisPort");
            Insert(0, port);

            AddContextTransition(port, IndexOf(port), "Create Transition");
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
