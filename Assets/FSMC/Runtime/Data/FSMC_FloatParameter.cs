using FSMC;
using System;
using UnityEngine;

namespace FSMC.Runtime
{
    [System.Serializable]
    public class FSMC_FloatParameter : FSMC_Parameter, IComparable<float>
    {
        [SerializeField] private float _value = 0;
        public float Value { get => _value; set => _value = value; }

        public FSMC_FloatParameter(float value, FSMC_ParameterType type, string name) : base(type, name)
        {
            Value = value;
        }

        public int CompareTo(float other)
        {
            int result = Value.CompareTo(other);
            if (result > 0) return 1;
            else if (result < 0) return -1;
            return 0;
        }

    }
}