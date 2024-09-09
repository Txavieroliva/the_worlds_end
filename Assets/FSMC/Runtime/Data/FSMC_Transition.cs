using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSMC.Runtime
{
    [Serializable]
    public class FSMC_Transition
    {
        [SerializeField] public List<FSMC_ConditionWrapper> conditions;


        [SerializeReference] private FSMC_State _originState = null;
        public FSMC_State OriginState { get => _originState; private set => _originState = value; }


        [SerializeReference] private FSMC_State _destinationState = null;
        public FSMC_State DestinationState { get => _destinationState; private set => _destinationState = value; }


        [SerializeField] private string _name;
        public string Name { get => _name; set => _name = value; }


        public FSMC_Transition(string name, FSMC_State origin, FSMC_State destination)
        {
            _name = name;
            _originState = origin;
            _destinationState = destination;
            var wrap = new FSMC_ConditionWrapper();
            wrap.conditions = new();
            conditions = new() { wrap };
        }

        public FSMC_State Evaluate()
        {
            if(conditions.Any(con=>con.conditions.All(c=>c.Check())))
                return DestinationState;
            return null;
        }
    }

    #region Conditions
    [Serializable]
    public class FSMC_ConditionWrapper
    {
        [SerializeReference]public List<FSMC_Condition> conditions = new();
    }
    [Serializable]
    public abstract class FSMC_Condition
    {
        public ComparisonType comparison;
        [SerializeReference] public FSMC_Parameter parameter;
        public abstract bool Check();
    }
    [Serializable]
    public class FSMC_FloatCondition : FSMC_Condition
    {
        public float Value;
        public override bool Check()
        {
            if (comparison == ComparisonType.NotEqual) return (parameter as FSMC_FloatParameter).CompareTo(Value) != (int)ComparisonType.Equal;
            return (parameter as FSMC_FloatParameter).CompareTo(Value) == (int)comparison;
        }
    }
    [Serializable]
    public class FSMC_IntegerCondition : FSMC_Condition
    {
        public int Value;
        public override bool Check()
        {
            if (comparison == ComparisonType.NotEqual) return (parameter as FSMC_IntegerParameter).CompareTo(Value) != (int)ComparisonType.Equal;
            return (parameter as FSMC_IntegerParameter).CompareTo(Value) == (int)comparison;
        }
    }
    [Serializable]
    public class FSMC_BoolCondition : FSMC_Condition
    {
        public bool Value;
        public override bool Check()
        {
            if (comparison == ComparisonType.NotEqual) return (parameter as FSMC_BoolParameter).CompareTo(Value) != (int)ComparisonType.Equal;
            return (parameter as FSMC_BoolParameter).CompareTo(Value) == (int)comparison;
        }
    }

    public enum ComparisonType
    {
        Lower=-1, Equal=0, NotEqual=2, Greater=1
    }
    #endregion
}
