using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using FSMC.Runtime;

namespace FSMC.Editor.Windows
{
    public class FSMC_ParameterDisplay : VisualElement
    {
        public FSMC_ParameterType type = FSMC_ParameterType.Integer;
        private VisualElement valueContainer;
        public FSMC_ParameterDisplay()
        {
            TextField paramName = new TextField() { name = "paramName" };
            Label paramNameL = new Label() { name = "paramName" };
            paramNameL.style.display = DisplayStyle.None;
            Add(paramName);
            Add(paramNameL);

            valueContainer = new VisualElement() { name = "paramValue"};
            
            valueContainer.Add(new IntegerField() { name="IntegerValue"});
            valueContainer.Add(new FloatField() { name="FloatValue"});
            valueContainer.Add(new Toggle() { name="BoolValue"});
            valueContainer.Add(new Toggle() { name="TriggerValue"});

            Add(valueContainer);
        }
        public void RedrawValue()
        {
            valueContainer.Q<IntegerField>(name: "IntegerValue").style.display = DisplayStyle.None;
            valueContainer.Q<IntegerField>(name: "IntegerValue").Unbind();
            valueContainer.Q<FloatField>(name: "FloatValue").style.display = DisplayStyle.None;
            valueContainer.Q<FloatField>(name: "FloatValue").Unbind();
            valueContainer.Q<Toggle>(name: "BoolValue").style.display = DisplayStyle.None;
            valueContainer.Q<Toggle>(name: "BoolValue").Unbind();
            valueContainer.Q<Toggle>(name: "TriggerValue").style.display = DisplayStyle.None;
            valueContainer.Q<Toggle>(name: "TriggerValue").Unbind();

            switch (type)
            {
                case FSMC_ParameterType.Integer:
                    valueContainer.Q<IntegerField>(name: "IntegerValue").style.display = DisplayStyle.Flex;
                    break;
                case FSMC_ParameterType.Float:
                    valueContainer.Q<FloatField>(name: "FloatValue").style.display = DisplayStyle.Flex;
                    break;
                case FSMC_ParameterType.Bool:
                    valueContainer.Q<Toggle>(name: "BoolValue").style.display = DisplayStyle.Flex;
                    break;
                case FSMC_ParameterType.Trigger:
                    valueContainer.Q<Toggle>(name: "TriggerValue").style.display = DisplayStyle.Flex;
                    break;

            }
        }
        public void SetLabel(bool value)
        {
            if (value)
            {
                this.Q<TextField>(name: "paramName").style.display = DisplayStyle.None;
                this.Q<TextField>(name: "paramName").Unbind();
                this.Q<Label>(name: "paramName").style.display = DisplayStyle.Flex;
            }
            else
            {
                this.Q<Label>(name: "paramName").style.display = DisplayStyle.None;
                this.Q<TextField>(name: "paramName").style.display = DisplayStyle.Flex;
            }
        }

        
    }
}