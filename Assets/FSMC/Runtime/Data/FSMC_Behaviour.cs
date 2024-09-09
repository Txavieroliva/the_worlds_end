using FSMC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMC.Runtime
{
    [Serializable]
    public abstract class FSMC_Behaviour
    {
        public bool enabled = true;

        public virtual void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
        {

        }
        public virtual void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
        {

        }

        public virtual void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
        {

        }

        public virtual void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
        {

        }
    }
}
