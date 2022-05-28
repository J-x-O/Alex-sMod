using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AlexMod.Utility {
    // --------------------------------------------------------------------------------------------------------------------
    /// <copyright file="HelpAttribute.cs">
    ///   <See cref="https://github.com/johnearnshaw/unity-inspector-help"></See>
    ///   Copyright (c) 2017, John Earnshaw, reblGreen Software Limited
    ///   <See cref="https://github.com/johnearnshaw/"></See>
    ///   <See cref="https://bitbucket.com/juanshaf/"></See>
    ///   <See cref="https://reblgreen.com/"></See>
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
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class HelpAttribute : PropertyAttribute {
        
        public readonly string Text;
        public readonly bool HideProperty;

        // MessageType exists in UnityEditor namespace and can throw an exception when used outside the editor.
        // We spoof MessageType at the bottom of this script to ensure that errors are not thrown when
        // MessageType is unavailable.
        public readonly MessageType Type;


        /// <summary> Adds a HelpBox to the Unity property inspector above this field. </summary>
        /// <param name="text">The help text to be displayed in the HelpBox.</param>
        /// <param name="hideProperty"> if true the original property will not be drawn, only the box </param>
        /// <param name="type">The icon to be displayed in the HelpBox.</param>
        public HelpAttribute(string text, bool hideProperty = false, MessageType type = MessageType.Info) {
            Text = text;
            HideProperty = hideProperty;
            Type = type;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(HelpAttribute))]
    public class HelpDrawer : PropertyDrawer {
        
        // Used for top and bottom padding between the text and the HelpBox border.
        const int paddingHeight = 8;

        // Used to add some margin between the the HelpBox and the property.
        const int marginHeight = 2;

        //  Global field to store the original (base) property height.
        float baseHeight = 0;

        // Custom added height for drawing text area which has the MultilineAttribute.
        float addedHeight = 0;

        /// <summary> A wrapper which returns the PropertyDrawer.attribute field as a HelpAttribute. </summary>
        HelpAttribute helpAttribute => (HelpAttribute)attribute;

        /// <summary> A helper property to check for RangeAttribute. </summary>
        RangeAttribute rangeAttribute {
            get {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(RangeAttribute), true);
                return attributes.Length > 0 ? (RangeAttribute)attributes[0] : null;
            }
        }

        /// <summary>
        /// A helper property to check for MultiLineAttribute.
        /// </summary>
        MultilineAttribute multilineAttribute {
            get {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(MultilineAttribute), true);
                return attributes.Length > 0 ? (MultilineAttribute)attributes[0] : null;
            }
        }


        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            // We store the original property height for later use...
            baseHeight = base.GetPropertyHeight(prop, label);

            // This stops icon shrinking if text content doesn't fill out the container enough.
            const float minHeight = paddingHeight * 5;

            // Calculate the height of the HelpBox using the GUIStyle on the current skin and the inspector
            // window's currentViewWidth.
            GUIContent content = new GUIContent(helpAttribute.Text);
            GUIStyle style = GUI.skin.GetStyle("helpbox");

            float height = style.CalcHeight(content, EditorGUIUtility.currentViewWidth);

            // We add tiny padding here to make sure the text is not overflowing the HelpBox from the top
            // and bottom.
            height += marginHeight * 2;

            // Since we draw a custom text area with the label above if our property contains the
            // MultilineAttribute, we need to add some extra height to compensate. This is stored in a
            // seperate global field so we can use it again later.
            if (multilineAttribute != null && prop.propertyType == SerializedPropertyType.String)
            {
                addedHeight = 48f;
            }

            // If the calculated HelpBox is less than our minimum height we use this to calculate the returned
            // height instead.
            return height > minHeight ? height + baseHeight + addedHeight : minHeight + baseHeight + addedHeight;
        }


        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            // We get a local reference to the MultilineAttribute as we use it twice in this method and it
            // saves calling the logic twice for minimal optimization, etc...
            var multiline = multilineAttribute;

            EditorGUI.BeginProperty(position, label, prop);

            // Copy the position out so we can calculate the position of our HelpBox without affecting the
            // original position.
            var helpPos = position;

            helpPos.height -= baseHeight + marginHeight;


            if (multiline != null)
            {
                helpPos.height -= addedHeight;
            }

            // Renders the HelpBox in the Unity inspector UI.
            EditorGUI.HelpBox(helpPos, helpAttribute.Text, helpAttribute.Type);

            position.y += helpPos.height + marginHeight;
            position.height = baseHeight;

            // if we dont draw the property exit here
            if (helpAttribute.HideProperty) {
                EditorGUI.EndProperty();
                return;
            }
            
            // If we have a RangeAttribute on our field, we need to handle the PropertyDrawer differently to
            // keep the same style as Unity's default.
            RangeAttribute range = rangeAttribute;

            bool handeled = false;
            if (range != null) {
                if (prop.propertyType == SerializedPropertyType.Float) {
                    EditorGUI.Slider(position, prop, range.min, range.max, label);
                    handeled = true;
                }
                else if (prop.propertyType == SerializedPropertyType.Integer) {
                    EditorGUI.IntSlider(position, prop, (int) range.min, (int) range.max, label);
                    handeled = true;
                }
            }
            
            // Here's where we handle the PropertyDrawer differently if we have a MultiLineAttribute, to try
            // and keep some kind of multiline text area. This is not identical to Unity's default but is
            // better than nothing...
            else if (multiline != null) {
                if (prop.propertyType == SerializedPropertyType.String) {
                    GUIStyle style = GUI.skin.label;
                    float size = style.CalcHeight(label, EditorGUIUtility.currentViewWidth);

                    EditorGUI.LabelField(position, label);

                    position.y += size;
                    position.height += addedHeight - size;

                    // Fixed text dissappearing thanks to: http://answers.unity3d.com/questions/244043/textarea-does-not-work-text-dissapears-solution-is.html
                    prop.stringValue = EditorGUI.TextArea(position, prop.stringValue);

                    handeled = true;
                }
            }
            
            // If we get to here it means we're drawing the default property field below the HelpBox
            if(!handeled) EditorGUI.PropertyField(position, prop, label);

            EditorGUI.EndProperty();
        }
    }
#else
    // Replicate MessageType Enum if we are not in editor as this enum exists in UnityEditor namespace.
    // This should stop errors being logged the same as Shawn Featherly's commit in the Github repo but I
    // feel is cleaner than having the conditional directive in the middle of the HelpAttribute constructor.
    public enum MessageType
    {
        None,
        Info,
        Warning,
        Error,
    }
#endif
}