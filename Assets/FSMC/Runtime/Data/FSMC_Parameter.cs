using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FSMC.Runtime
{
    [System.Serializable]
    public enum FSMC_ParameterType
    {
        Integer,
        Float,
        Bool,
        Trigger
    }

    [System.Serializable]
    public abstract class FSMC_Parameter
    {

        [SerializeField] private FSMC_ParameterType _type;
        public FSMC_ParameterType Type { get => _type; private set => _type = value; }

        [SerializeField] private string _name;
        public string Name { get => _name; set => _name = value; }

        protected FSMC_Parameter(FSMC_ParameterType type, string name)
        {
            Type = type;
            Name = name;
        }

        public static FSMC_Parameter CreateParameter(string name, FSMC_ParameterType type, object value)
        {
            FSMC_Parameter param = null;

            switch (type)
            {
                case FSMC_ParameterType.Integer:
                    if (value is not int) return null;
                    param = new FSMC_IntegerParameter((int)value, type, name);
                    break;
                case FSMC_ParameterType.Float:
                    if (value is not float) return null;
                    param = new FSMC_FloatParameter((float)value, type, name);
                    break;
                case FSMC_ParameterType.Bool:
                    if (value is not bool) return null;
                    param = new FSMC_BoolParameter((bool)value, type, name);
                    break;
                case FSMC_ParameterType.Trigger:
                    if (value is not bool) return null;
                    param = new FSMC_BoolParameter(false, type, name);
                    break;

            }
            return param;
        }
    }
}
