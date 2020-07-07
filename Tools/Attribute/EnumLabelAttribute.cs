
/**
EnumLabel.cs
Copyright (c) 2016 Hassaku
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
https://gist.github.com/HassakuTb/30dceff6fcfeca73fa5ea6a5ecbbc08f
*/
using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace CovaTech.EditorTools
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumLabelAttribute : PropertyAttribute{

        public string[] EnumNames { get; private set; }

        public EnumLabelAttribute(Type enumType) {
            Assert.IsTrue(enumType.IsEnum, "[EnumLabel] type of attribute parameter is not enum.");
            EnumNames = Enum.GetNames(enumType);
        }
    }
}