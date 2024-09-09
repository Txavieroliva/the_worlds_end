using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FSMC.Runtime
{
    [System.Serializable]
    public class FSMC_BoolParameter : FSMC_Parameter, IComparable<bool>
    {
        [SerializeField] private bool _value = false;
        public bool Value { get => _value; set => _value = value; }

        public FSMC_BoolParameter(bool value, FSMC_ParameterType type, string name) : base(type, name)
        {
            Value = value;
        }

        public int CompareTo(bool other)
        {
            int result = Value.CompareTo(other);
            if (result > 0) return 1;
            else if (result < 0) return -1;
            return 0;
        }
    }
}
