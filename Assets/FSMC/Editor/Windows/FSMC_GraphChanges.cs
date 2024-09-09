using FSMC.Editor.Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;
using FSMC.Runtime;

namespace FSMC.Editor.Windows
{
    public partial class FSMC_GraphView
    {
        private GraphViewChange OnGraphViewChange(GraphViewChange change)
        {
            Undo.RecordObject(Controller, "State Machine changed");

            //EDGES CREATION
            if (change.edgesToCreate != null)
            {
                foreach (Edge edge in change.edgesToCreate)
                {
                    //CREATING START EDGE
                    if (edge.output.node is FSMC_StartNode)
                    {
                        Controller.StartingState = (edge.input.node as FSMC_StateNode).State;
                        if (this.Query<Edge>().Where(e => e.output.node is FSMC_StartNode && !e.Equals(edge)).ToList().Count() > 0)
                        {
                            this.RemoveElement(this.Query<Edge>().Where(e => e.output.node is FSMC_StartNode && !e.Equals(edge)).First());
                        }
                    }
                    //CREATING ANY EDGE
                    else if (edge.output.node is FSMC_AnyNode)
                    {
                        var transition = new FSMC_Transition("Any->" + (edge.input.node as FSMC_BaseNode).NodeName, null, (edge.input.node as FSMC_StateNode).State);

                        Controller.AnyTransitions.Add(transition);
                        (edge.input.node as FSMC_StateNode).State.TransitionsTo.Add(transition);
                        (edge as FSMC_Edge).transition = transition;
                    }
                    //CREATING EDGE BETWEEN STATES
                    else
                    {
                        var transition = new FSMC_Transition((edge.output.node as FSMC_BaseNode).NodeName + "->" + (edge.input.node as FSMC_BaseNode).NodeName,
                            (edge.output.node as FSMC_StateNode).State, (edge.input.node as FSMC_StateNode).State);
                        (edge.output.node as FSMC_StateNode).State.TransitionsFrom.Add(transition);
                        (edge.input.node as FSMC_StateNode).State.TransitionsTo.Add(transition);
                        (edge as FSMC_Edge).transition = transition;
                    }
                }
            }

            //MOVING NODES


            if (change.movedElements != null)
            {
                foreach (var element in change.movedElements)
                {
                    if (element is FSMC_StartNode)
                    {
                        Controller.StartPosition = new Vector2((element as Node).GetPosition().x, (element as Node).GetPosition().y);
                    }
                    else if (element is FSMC_AnyNode)
                    {
                        Controller.AnyPosition = new Vector2((element as Node).GetPosition().x, (element as Node).GetPosition().y);
                    }
                    else if (element is FSMC_StateNode)
                    {
                        FSMC_StateNode node = element as FSMC_StateNode;
                        node.State.Position = new Vector2(node.GetPosition().x, node.GetPosition().y);
                    }
                }
            }

            //REMOVING ELEMENTS

            if (change.elementsToRemove != null)
            {
                //SAFE CHECK & TRANSITIONS CASCADE
                for (int i = change.elementsToRemove.Count - 1; i >= 0; i--)
                {
                    //ADD ALL TRANSITIONS TO REMOVAL ON STATE DELETION
                    if (change.elementsToRemove[i] is FSMC_StateNode)
                    {
                        var node = change.elementsToRemove[i] as FSMC_StateNode;
                        node.Query<Port>().ForEach(p => change.elementsToRemove.AddRange(p.connections));
                    }
                    //PREVENT DELETING START AND ANY NODE
                    else if (change.elementsToRemove[i] is FSMC_AnyNode || change.elementsToRemove[i] is FSMC_StartNode)
                    {
                        change.elementsToRemove.RemoveAt(i);
                    }
                    //PREVENT DELETING START TRANSITION
                    else if (change.elementsToRemove[i] is FSMC_Edge)
                    {
                        var edge = change.elementsToRemove[i] as FSMC_Edge;
                        if (edge.transition == null) //That means it is a start transition
                        {
                            change.elementsToRemove.RemoveAt(i);
                        }
                    }
                }

                //DISTINCT FOR SAFETY. USER COULD SELECT MULTIPLE ELEMENTS TO DELETE AT ONCE, SOME OF THEM COULD BE ADDED SECOND TIME IN PREVIOUS OPERATIONS
                change.elementsToRemove = change.elementsToRemove.Distinct().ToList();

                foreach (var e in change.elementsToRemove)
                {
                    if (e is FSMC_StateNode)
                    {
                        Controller.States.Remove((e as FSMC_StateNode).State);
                    }
                    else if (e is FSMC_Edge)
                    {
                        var edge = e as FSMC_Edge;
                        if (edge.transition == null) //That means it is a start transition
                        {
                            Controller.StartingState = null;
                            this.Query<FSMC_StateNode>().Where(n => !change.elementsToRemove.Contains(n)).First()?.SetAsStart(Controller);
                        }
                        else if (edge.transition != null && edge.transition.OriginState == null) //That means it is an any transition
                        {
                            Controller.AnyTransitions.Remove(edge.transition);
                            edge.transition.DestinationState.TransitionsTo.Remove(edge.transition);
                        }
                        else if (edge.transition != null) //Simple transition
                        {
                            edge.transition.OriginState.TransitionsFrom.Remove(edge.transition);
                            edge.transition.DestinationState.TransitionsTo.Remove(edge.transition);
                        }
                    }
                }
            }
            if (FSMC_EditorWindow.Current.currentSelection != null && (change.elementsToRemove == null || !change.elementsToRemove.Contains(FSMC_EditorWindow.Current.currentSelection)))
            {
                FSMC_EditorWindow.Current.Refresh();

            }
            return change;
        }
    }
}
