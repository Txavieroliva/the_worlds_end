using FSMC.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSMC.Editor
{
    public static class FSMC_Utils 
    {
        public static void SetPlaceholderText(this TextField textField, string placeholder)
        {
            string placeholderClass = TextField.ussClassName + "__placeholder";

            onFocusOut();
            textField.RegisterCallback<FocusInEvent>(evt => onFocusIn());
            textField.RegisterCallback<FocusOutEvent>(evt => onFocusOut());

            void onFocusIn()
            {
                if (textField.ClassListContains(placeholderClass))
                {
                    textField.value = string.Empty;
                    textField.RemoveFromClassList(placeholderClass);
                }
            }

            void onFocusOut()
            {
                if (string.IsNullOrEmpty(textField.text))
                {
                    textField.SetValueWithoutNotify(placeholder);
                    textField.AddToClassList(placeholderClass);
                }
            }
        }

        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty property, int skip=0)
        {
            var nextElement = property.Copy();
            bool hasNextElement = nextElement.NextVisible(false);
            if (!hasNextElement)
            {
                nextElement = null;
            }

            property.NextVisible(true);
            for(int i=0; i<skip; i++)
            {
                if ((SerializedProperty.EqualContents(property, nextElement)))
                {
                    yield break;
                }
                else
                    property.NextVisible(false);
            }
            while (true)
            {
                if ((SerializedProperty.EqualContents(property, nextElement)))
                {
                    yield break;
                }

                yield return property;

                bool hasNext = property.NextVisible(false);
                if (!hasNext)
                {
                    break;
                }
            }
        }
        
    }
}
