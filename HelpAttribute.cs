// --------------------------------------------------------------------------------------------------------------------
/// <copyright file="HelpAttribute.cs">
///   <See cref="https://github.com/johnearnshaw/unity-inspector-help"></See>
///   Copyright (c) 2017, John Earnshaw
///   <See cref="https://github.com/johnearnshaw/"></See>
///   <See cref="https://bitbucket.com/juanshaf/"></See>
///   All rights reserved.
///   Redistribution and use in source and binary forms, with or without modification, are
///   permitted provided that the following conditions are met:
///      1. Redistributions of source code must retain the above copyright notice, this list of
///         conditions and the following disclaimer.
///      2. Redistributions in binary form must reproduce the above copyright notice, this list
///         of conditions and the following disclaimer in the documentation and/or other materials
///         provided with the distribution.
///   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY
///   EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
///   MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE
///   COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
///   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
///   SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
///   HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
///   TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
///   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEditor;

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class HelpAttribute : PropertyAttribute
{
    public readonly string help;
    public readonly MessageType type;

    public HelpAttribute(string help, MessageType type = MessageType.Info)
    {
        this.help = help;
        this.type = type;
    }
}

[CustomPropertyDrawer(typeof(HelpAttribute))]
public class HelpDrawer : PropertyDrawer
{
    const int lineHeight = 6;
    const int baseHeight = 16;
    const int paddingHeight = 8;
    const int marginHeight = 3;

    HelpAttribute helpAttribute { get { return (HelpAttribute)attribute; } }

    RangeAttribute rangeAttribute
    {
        get
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(RangeAttribute), true);
            return attributes != null && attributes.Length > 0 ? (RangeAttribute)attributes[0] : null;
        }
    }

    MultilineAttribute multilineAttribute
    {
        get
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(MultilineAttribute), true);
            return attributes != null && attributes.Length > 0 ? (MultilineAttribute)attributes[0] : null;
        }
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        float multiline = baseHeight * 2;

        foreach (var c in helpAttribute.help.ToCharArray())
        {
            if (c == '\n')
            {
                multiline += lineHeight + marginHeight + 1;
            }
        }

        if (multilineAttribute != null && prop.propertyType == SerializedPropertyType.String)
        {
            multiline *= 2.5f;
        }

        return multiline + baseHeight + paddingHeight + marginHeight;
    }


    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        var multiline = multilineAttribute;

        EditorGUI.BeginProperty(position, label, prop);

        var helpPos = position;
        helpPos.height -= baseHeight + marginHeight;

        if (multiline != null)
        {
            helpPos.height -= 48;
        }

        EditorGUI.HelpBox(helpPos, helpAttribute.help, helpAttribute.type);

        position.y += helpPos.height + marginHeight;
        position.height = baseHeight;

        var range = rangeAttribute;
        
        if (range != null)
        {
            if (prop.propertyType == SerializedPropertyType.Float)
            {
                EditorGUI.Slider(position, prop, range.min, range.max, label);
            }
            else if (prop.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.IntSlider(position, prop, (int)range.min, (int)range.max, label);
            }
            else
            {
                EditorGUI.PropertyField(position, prop, label);
            }
        }
        else if (multiline != null)
        {
            if (prop.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label);

                position.y += baseHeight;
                position.height += 31;

                // Fixed text dissappearing thanks to: http://answers.unity3d.com/questions/244043/textarea-does-not-work-text-dissapears-solution-is.html
                prop.stringValue = EditorGUI.TextArea(position, prop.stringValue);
            }
            else
            {
                EditorGUI.PropertyField(position, prop, label);
            }
        }
        else
        {
            EditorGUI.PropertyField(position, prop, label);
        }

        EditorGUI.EndProperty();
    }
}