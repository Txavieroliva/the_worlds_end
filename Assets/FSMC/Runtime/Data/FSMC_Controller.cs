using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace FSMC.Runtime
{

    [CreateAssetMenu(fileName = "FSMC_controller", menuName = "FSMC/Controller")]
    public class FSMC_Controller : ScriptableObject
    {
#if UNITY_EDITOR
        public Vector2 StartPosition = new(200,200);
        public Vector2 AnyPosition = new(200,400);
#endif
        [SerializeReference] public List<FSMC_State> States = new();
        [SerializeReference] public List<FSMC_Transition> AnyTransitions = new();
        [SerializeReference] public FSMC_State StartingState = null;
        [SerializeReference] public List<FSMC_Parameter> Parameters = new();
        private Dictionary<int, FSMC_Parameter> _parametersLookup;

        private FSMC_State _currentState;
        private FSMC_State _transitioningTo;
        private List<FSMC_BoolParameter> _triggersActive;

        public void StartStateMachine(FSMC_Executer executer)
        {
            _parametersLookup = new Dictionary<int, FSMC_Parameter>();
            foreach(var par in Parameters)
            {
                _parametersLookup.Add(Animator.StringToHash(par.Name), par);
            }

            _triggersActive = new();
            foreach(var state in States)
            {
                state.StateInit(this, executer);
            }
            _transitioningTo = StartingState;
        }

        public void UpdateStateMachine(FSMC_Executer executer)
        {
            if(_transitioningTo != null)
            {
                _currentState = _transitioningTo;
                _transitioningTo = null;
                _currentState.OnStateEnter(this, executer);
            }

            _currentState.OnStateUpdate(this, executer);

            FSMC_State state = null;
            foreach(var transition in AnyTransitions)
            {
                state = transition.Evaluate();
                if (state != null) break;
            }

            if (state == null)
                state = _currentState.Evaluate();

            if(state != null)
            {
                _currentState.OnStateExit(this, executer);
                _transitioningTo = state;
            }

            for(int i = _triggersActive.Count - 1; i >= 0; i--)
            {
                _triggersActive[i].Value = false;
                _triggersActive.RemoveAt(i);
            }
        }




        public void SetFloat(string name, float value)
        {
            (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Float) as FSMC_FloatParameter).Value = value;
        }
        public void SetFloat(int id, float value)
        {
            (_parametersLookup[id] as FSMC_FloatParameter).Value = value;
        }
        public float GetFloat(string name)
        {
            return (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Float) as FSMC_FloatParameter).Value;
        }
        public float GetFloat(int id)
        {
            return (_parametersLookup[id] as FSMC_FloatParameter).Value;
        }


        public void SetInt(string name, int value)
        {
            (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Integer) as FSMC_IntegerParameter).Value = value;
        }
        public void SetInt(int id, int value)
        {
            (_parametersLookup[id] as FSMC_IntegerParameter).Value = value;
        }
        public int GetInt(string name)
        {
            return (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Integer) as FSMC_IntegerParameter).Value;
        }
        public int GetInt(int id)
        {
            return (_parametersLookup[id] as FSMC_IntegerParameter).Value;
        }


        public void SetBool(string name, bool value)
        {
            (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Bool) as FSMC_BoolParameter).Value = value;
        }
        public void SetBool(int id, bool value)
        {
            (_parametersLookup[id] as FSMC_BoolParameter).Value = value;
        }
        public bool GetBool(string name)
        {
            return (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Bool) as FSMC_BoolParameter).Value;
        }
        public bool GetBool(int id)
        {
            return (_parametersLookup[id] as FSMC_BoolParameter).Value;
        }


        public void SetTrigger(string name)
        {
            FSMC_BoolParameter trigger = (Parameters.SingleOrDefault(p => p.Name == name && p.Type == FSMC_ParameterType.Trigger) as FSMC_BoolParameter);
            trigger.Value = true;
            _triggersActive.Add(trigger);
        }
        public void SetTrigger(int id)
        {
            FSMC_BoolParameter trigger = (_parametersLookup[id] as FSMC_BoolParameter);
            trigger.Value = true;
            _triggersActive.Add(trigger);
        }

        public FSMC_State GetCurrentState()
        {
            return _currentState;
        }
        public void SetCurrentState(string name, FSMC_Executer executer)
        {
            FSMC_State state = States.SingleOrDefault(s => s.Name == name);
            if(state != null)
            {
                _currentState.OnStateExit(this, executer);
                _transitioningTo = state;
            }
        }
        public FSMC_State GetState(string name)
        {
            return States.SingleOrDefault(s => s.Name == name);
        }
        public static int StringToHash(string name) => Animator.StringToHash(name);

    }
}
