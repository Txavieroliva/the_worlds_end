using FSMC.Editor.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using FSMC.Runtime;

namespace FSMC.Editor.Windows
{
    public partial class FSMC_GraphView : GraphView
    {
        private void DeserializeElements(string operationName, string data)
        {
            List<StateSerialized> states = JsonUtility.FromJson<SerWrapper>(data).ser;
            if (states == null) return;
            Undo.RecordObject(Controller, operationName + "FSMC elements");
            Dictionary<int, FSMC_StateNode> createdStates = new();
            foreach (var state in states)
            {
                var newState=
                CreateNode(contentViewContainer.WorldToLocal(mousePos) + state.Position + (operationName == "Duplicate" ? Vector2.one * 10 : Vector2.zero),
                    Controller.States.Any(s => s.Name == state.Name) ? state.Name + "_1" : state.Name);
                newState.State.AddBehaviours(BehavioursWrapper.DeserializeFromJson(state.Behaviours).ToArray());
                createdStates.Add(state.Hash, newState);
            }
            foreach (var state in states)
            {
                foreach (var t in state.Transitions)
                {
                    var edge = createdStates[state.Hash].Q<Port>(className: "output").ConnectTo<FSMC_Edge>(createdStates[t.Target].Q<Port>(className: "input"));

                    var transition = new FSMC_Transition(createdStates[state.Hash].NodeName + "->" + createdStates[t.Target].NodeName,
                            createdStates[state.Hash].State, createdStates[t.Target].State);
                    List<FSMC_ConditionWrapper> conditionWrappers = new();
                    foreach(var wrap in t.conditionsWrappers)
                    {
                        FSMC_ConditionWrapper conditionWrapper = new() { conditions = new()};
                        foreach( var con in wrap.Conditions)
                        {
                            try
                            {
                                FSMC_Condition condition;
                                switch (con.Type)
                                {
                                    case FSMC_ParameterType.Integer:
                                        condition = new FSMC_IntegerCondition() { comparison = con.ComparisonType, Value = con.IntValue, parameter = Controller.Parameters.Where(p => p.Name == con.ParamName).First() };
                                        break;
                                    case FSMC_ParameterType.Float:
                                        condition = new FSMC_FloatCondition() { comparison = con.ComparisonType, Value = con.FloatValue, parameter = Controller.Parameters.Where(p => p.Name == con.ParamName).First() };
                                        break;
                                    default:
                                        condition = new FSMC_BoolCondition() { comparison = con.ComparisonType, Value = con.BoolValue, parameter = Controller.Parameters.Where(p => p.Name == con.ParamName).First() };
                                        break;
                                }
                                conditionWrapper.conditions.Add(condition);
                            }
                            catch (Exception) { }
                        }
                        conditionWrappers.Add(conditionWrapper);
                    }
                    transition.conditions = conditionWrappers;
                    createdStates[state.Hash].State.TransitionsFrom.Add(transition);
                    createdStates[t.Target].State.TransitionsTo.Add(transition);
                    edge.transition = transition;

                    edge.transition = transition;
                    AddElement(edge);
                }
            }
        }
        private bool CanPaste(string data)
        {
            try
            {
                var des = JsonUtility.FromJson<SerWrapper>(data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string SerializeElements(IEnumerable<GraphElement> elements)
        {
            List <StateSerialized> clipboard = new();
            IEnumerable<FSMC_StateNode> states = elements.OfType<FSMC_StateNode>();
            foreach (var state in states)
            {
                clipboard.Add(new()
                {
                    Name = state.State.Name,
                    Hash = state.State.GetHashCode(),
                    Position = state.State.Position - contentViewContainer.WorldToLocal(mousePos),
                    Behaviours = BehavioursWrapper.SerializeToJson(state.State.GetBehaviours().ToList()) ,
                    Transitions = selection.OfType<FSMC_Edge>().Where(t => t.transition != null && t.transition.OriginState == state.State && states.Any(s => s.State == t.transition.DestinationState))
                    .Select(x => x.transition).Select(x=>new TransitionSerialized() { 
                        Target = x.DestinationState.GetHashCode(),
                        conditionsWrappers = x.conditions.Select(c=>new ConditionsWrapper()
                        {
                            Conditions = c.conditions.Select(con => {
                                if (con is FSMC_FloatCondition)
                                    return new ConditionSerialized() { ParamName = con.parameter.Name, Type = con.parameter.Type, FloatValue = (con as FSMC_FloatCondition).Value, ComparisonType = (con as FSMC_FloatCondition).comparison };
                                else if(con is FSMC_IntegerCondition)
                                    return new ConditionSerialized() { ParamName = con.parameter.Name, Type = con.parameter.Type, IntValue = (con as FSMC_IntegerCondition).Value, ComparisonType = (con as FSMC_IntegerCondition).comparison };
                                else
                                    return new ConditionSerialized() { ParamName = con.parameter.Name, Type = con.parameter.Type, BoolValue = (con as FSMC_BoolCondition).Value, ComparisonType = (con as FSMC_BoolCondition).comparison };
                            }).ToList()
                        }).ToList()
                    }).ToList()
                });
            }
            return JsonUtility.ToJson(new SerWrapper() { ser = clipboard });
        }
        [Serializable]
        private class SerWrapper
        {
            public List<StateSerialized> ser;
        }
        [Serializable]
        private class StateSerialized
        {
            public string Name;
            public int Hash;
            public Vector2 Position;
            public BehavioursWrapper Behaviours;
            public List<TransitionSerialized> Transitions;
        }
        [Serializable]
        private class BehavioursWrapper
        {
            public List<BehaviourSerialized> Behaviours;

            public static BehavioursWrapper SerializeToJson(List<FSMC_Behaviour> behaviours)
            {
                List<BehaviourSerialized> serializedBehaviours = new List<BehaviourSerialized>();
                foreach (FSMC_Behaviour behaviour in behaviours)
                {
                    BehaviourSerialized behaviourSerialized = new BehaviourSerialized
                    {
                        Type = behaviour.GetType().AssemblyQualifiedName,
                        Data = JsonUtility.ToJson(behaviour)
                    };
                    serializedBehaviours.Add(behaviourSerialized);
                }
                return new BehavioursWrapper() { Behaviours = serializedBehaviours };
            }

            public static List<FSMC_Behaviour> DeserializeFromJson(BehavioursWrapper wrapper)
            {
                List<FSMC_Behaviour> behaviours = new();
                foreach (BehaviourSerialized BehaviourSerialized in wrapper.Behaviours)
                {
                    try
                    {
                        Type behaviourType = Type.GetType(BehaviourSerialized.Type);
                        FSMC_Behaviour behaviour = (FSMC_Behaviour)JsonUtility.FromJson(BehaviourSerialized.Data, behaviourType);
                        behaviours.Add(behaviour);
                    }
                    catch (Exception) { }
                }

                return behaviours;
            }
        }
        [Serializable]
        private class BehaviourSerialized
        {
            public string Type;
            public string Data;
        }
        [Serializable]
        private class TransitionSerialized
        {
            public int Target;
            public List<ConditionsWrapper> conditionsWrappers;
        }
        [Serializable]
        private class ConditionsWrapper
        {
            public List<ConditionSerialized> Conditions;
        }
        [Serializable]
        private class ConditionSerialized
        {
            public FSMC_ParameterType Type;
            public string ParamName;
            public int IntValue;
            public float FloatValue;
            public bool BoolValue;
            public ComparisonType ComparisonType;
        }
    }
}
