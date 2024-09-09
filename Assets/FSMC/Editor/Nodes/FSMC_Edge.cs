using FSMC.Editor.Windows;
using FSMC.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace FSMC.Editor.Nodes
{
    public class FSMC_Edge : Edge
    {
        public FSMC_Transition transition;

        private FSMC_EdgeArrow arrow;

        public FSMC_Edge() : base()
        {
            arrow = new(this);
            Add(arrow);
            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeControlGeometryChanged);
        }

        private void OnEdgeControlGeometryChanged(GeometryChangedEvent evt)
        {
            PointsAndTangents[1] = PointsAndTangents[0];
            PointsAndTangents[2] = PointsAndTangents[3];

            if(input!=null && output!=null)
            {
                if (input.node.GetPosition().y > output.node.GetPosition().y)// UP
                {
                    PointsAndTangents[1].x += 8;
                    PointsAndTangents[2].x += 8;
                }
                else if (input.node.GetPosition().y < output.node.GetPosition().y)// Down
                {
                    PointsAndTangents[1].x -= 8;
                    PointsAndTangents[2].x -= 8;
                }
                else if (input.node.GetPosition().y == output.node.GetPosition().y) // Fix a wierd af bug in equally absurd way
                {
                    PointsAndTangents[1].x -= 1;
                    PointsAndTangents[1].y -= 1;
                }

                if (input.node.GetPosition().x > output.node.GetPosition().x)// Right
                {
                    PointsAndTangents[1].y -= 8;
                    PointsAndTangents[2].y -= 8;
                }
                else if(input.node.GetPosition().x < output.node.GetPosition().x)// Left
                {
                    PointsAndTangents[1].y += 8;
                    PointsAndTangents[2].y += 8;
                }
            }
    
            arrow.MarkDirtyRepaint();
        }
        public override void OnSelected()
        {
            base.OnSelected();
            arrow.MarkDirtyRepaint();
            if (FSMC_EditorWindow.Current == null) return;
            if (transition!=null)FSMC_EditorWindow.Current.currentSelection = this;
        }
        public override void OnUnselected()
        {
            base.OnUnselected();
            arrow.MarkDirtyRepaint();
            if (FSMC_EditorWindow.Current == null) return;
            if (FSMC_EditorWindow.Current.currentSelection == this) FSMC_EditorWindow.Current.currentSelection = null;
        }

        public Vector2[] GetPoints()
        {
            return PointsAndTangents;
        }

    }
}
