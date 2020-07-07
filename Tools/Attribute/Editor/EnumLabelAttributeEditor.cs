/**
EnumLabel.cs
Copyright (c) 2016 Hassaku
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
https://gist.github.com/HassakuTb/30dceff6fcfeca73fa5ea6a5ecbbc08f
*/
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace CovaTech.EditorTools
{
    [CustomPropertyDrawer(typeof(EnumLabelAttribute))]
    public class EnumLabelDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            EnumLabelAttribute attr = attribute as EnumLabelAttribute;
            Match match = new Regex(@"Element (?<index>\d+)").Match(label.text);
            if(!match.Success){
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            int index = int.Parse(match.Groups["index"].Value);

            if (index < attr.EnumNames.Length) {
                EditorGUI.PropertyField(position, property, new GUIContent(attr.EnumNames[index]), true);
            }
            else {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}