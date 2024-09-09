using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSMC.Runtime
{
    [System.Serializable]
    public class FSMC_State
    {
        [SerializeReference] private List<FSMC_Behaviour> _behaviours = new();

#if UNITY_EDITOR
        public Vector2 Position;
#endif
        [SerializeField] private string _name;
        public string Name { get => _name; private set => _name = value; }
        [SerializeReference] public List<FSMC_Transition> TransitionsFrom;
        [SerializeReference] public List<FSMC_Transition> TransitionsTo;

        public FSMC_State(string name)
        {
            _name = name;
            TransitionsFrom = new();
            TransitionsTo = new();
        }

        public void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
        {
            foreach (FSMC_Behaviour behaviour in _behaviours)
                behaviour.StateInit(stateMachine, executer);
        }
        public void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
        {
            foreach (FSMC_Behaviour behaviour in _behaviours)
                behaviour.OnStateEnter(stateMachine, executer);
        }
        public void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
        {
            foreach (FSMC_Behaviour behaviour in _behaviours)
                behaviour.OnStateUpdate(stateMachine, executer);
        }
        public void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
        {
            foreach (FSMC_Behaviour behaviour in _behaviours)
                behaviour.OnStateExit(stateMachine, executer);
        }
        public FSMC_State Evaluate()
        {
            foreach(FSMC_Transition transition in TransitionsFrom)
            {
                var state = transition.Evaluate();
                if (state != null) return state;
            }
            return null;
        }


        public T GetBehaviour<T>()
        {
            return _behaviours.OfType<T>().FirstOrDefault();
        }
        public FSMC_Behaviour GetBehaviour(Type type)
        {
            return _behaviours.Where(b => b.GetType().Equals(type)).FirstOrDefault();
        }
        public FSMC_Behaviour[] GetBehaviours()
        {
            return _behaviours.ToArray();
        }
        public T[] GetBehaviours<T>()
        {
            return _behaviours.OfType<T>().ToArray();
        }
        public FSMC_Behaviour[] GetBehaviours(Type type)
        {
            return _behaviours.Where(b => b.GetType().Equals(type)).ToArray();
        }
        public void AddBehaviour(FSMC_Behaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }
        public void AddBehaviours(FSMC_Behaviour[] behaviours)
        {
            _behaviours.AddRange(behaviours);
        }
        public void AddBehaviour<T>()
        {
            _behaviours.Add(Activator.CreateInstance<T>() as FSMC_Behaviour);
        }
        public void AddBehaviour(Type type)
        {
            _behaviours.Add(Activator.CreateInstance(type) as FSMC_Behaviour);
        }
    }
}
