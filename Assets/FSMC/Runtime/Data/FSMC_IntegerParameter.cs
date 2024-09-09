using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FSMC.Runtime
{
    [System.Serializable]
    public class FSMC_IntegerParameter : FSMC_Parameter, IComparable<int>
    {
        [SerializeField] private int _value = 0;
        public int Value { get => _value; set => _value = value; }

        public FSMC_IntegerParameter(int value, FSMC_ParameterType type, string name) : base(type, name)
        {
            Value = value;
        }

        public int CompareTo(int other)
        {
            int result = Value.CompareTo(other);
            if (result > 0) return 1;
            else if (result < 0) return -1;
            return 0;
        }
    }
}
