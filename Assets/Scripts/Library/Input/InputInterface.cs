using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Lib.Input
{
    /// <summary>
    /// Enum used to define current input entry type
    /// </summary>
    [System.Serializable]
    public enum InputInterfaceEntryType
    {
        Key,
        Axis,
        MouseButton,
        MouseDeltaX,
        MouseDeltaY,
        MousePosX,
        MousePosY,
        MouseScroll,
    }

    /// <summary>
    /// Enum used to define current input entry value return mode
    /// </summary>
    [System.Serializable]
    public enum InputInterfaceEntryMode
    {
        Default,
        Raw,
        Down,
        Up,
    }
    
    /// <summary>
    /// Enum used to define an input entry
    /// </summary>
    [System.Serializable]
    public class InputInterfaceEntry
    {
        public string key;
        public InputInterfaceEntryType type;
        public KeyCode keyCode;
        public int mouseButton;
        public string axis;

        public float this[InputInterfaceEntryMode mode] => GetValue(mode);
        public float floatValue => GetValue();
        public float floatRawValue => GetValue(InputInterfaceEntryMode.Raw);
        public bool boolValue => GetValue() > 0;
        public bool boolDownValue => GetValue(InputInterfaceEntryMode.Down) > 0;
        public bool boolUpValue => GetValue(InputInterfaceEntryMode.Up) > 0;

        /// <summary>
        /// Get current input value in the desired mode
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public float GetValue(InputInterfaceEntryMode mode = InputInterfaceEntryMode.Default)
        {
            switch (type)
            {
                case InputInterfaceEntryType.Key:
                    switch (mode)
                    {
                        case InputInterfaceEntryMode.Default:
                            return UnityEngine.Input.GetKey(keyCode) ? 1 : 0;
                        case InputInterfaceEntryMode.Down:
                            return UnityEngine.Input.GetKeyDown(keyCode) ? 1 : 0;
                        case InputInterfaceEntryMode.Up:
                            return UnityEngine.Input.GetKeyUp(keyCode) ? 1 : 0;
                    }
                    break;
                case InputInterfaceEntryType.Axis:
                    switch (mode)
                    {
                        case InputInterfaceEntryMode.Default:
                            return UnityEngine.Input.GetAxis(axis);
                        case InputInterfaceEntryMode.Raw:
                            return UnityEngine.Input.GetAxisRaw(axis);
                    }
                    break;
                case InputInterfaceEntryType.MouseButton:
                    switch (mode)
                    {
                        case InputInterfaceEntryMode.Default:
                            return UnityEngine.Input.GetMouseButton(mouseButton) ? 1 : 0;
                        case InputInterfaceEntryMode.Down:
                            return UnityEngine.Input.GetMouseButtonDown(mouseButton) ? 1 : 0;
                        case InputInterfaceEntryMode.Up:
                            return UnityEngine.Input.GetMouseButtonUp(mouseButton) ? 1 : 0;
                    }
                    break;
                case InputInterfaceEntryType.MousePosX:
                    return UnityEngine.Input.mousePosition.x;
                case InputInterfaceEntryType.MousePosY:
                    return UnityEngine.Input.mousePosition.y;
                case InputInterfaceEntryType.MouseDeltaX:
                    return UnityEngine.Input.GetAxis("Mouse X");
                case InputInterfaceEntryType.MouseDeltaY:
                    return UnityEngine.Input.GetAxis("Mouse Y");
                case InputInterfaceEntryType.MouseScroll:
                    return UnityEngine.Input.mouseScrollDelta.y;
            }
            return 0;
        }

        /// <summary>
        /// Cast current entry to a float value
        /// </summary>
        /// <param name="entry"></param>
        public static implicit operator float(InputInterfaceEntry entry)
        {
            return entry.floatValue;
        }

        /// <summary>
        /// Cast current entry to a boolean value
        /// </summary>
        /// <param name="entry"></param>
        public static implicit operator bool(InputInterfaceEntry entry)
        {
            return entry.boolValue;
        }
    }
    
    /// <summary>
    /// Main input interface class. Holds an array of input entries
    /// </summary>
    [CreateAssetMenu(fileName = "InputInterface", menuName = "GameLib/InputInterface", order = 1)]
    public class InputInterface : ScriptableObject 
    {
        public InputInterfaceEntry[] entries;

        [System.NonSerialized]
        private bool _optimized = false;

        /// <summary>
        /// Get a input interface entry using the key
        /// </summary>
        /// <value></value>
        public InputInterfaceEntry this[string key]
        {
            get
            {
                if (!_optimized)
                    _Optimize();

                return FindByKey(key);
            }
        }

        private void _Optimize()
        {
            _optimized = true;
            entries = entries.OrderBy(x => x.key).ToArray();
        }
        
        /// <summary>
        /// Finds an input interface entry using the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public InputInterfaceEntry FindByKey(string key)
        {
            int left = 0;
            int right = entries.Length - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (entries[mid].key == key)
                {
                    return entries[mid];
                }
                else if (entries[mid].key.CompareTo(key) < 0)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return null;
        }
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InputInterfaceEntry))]
    public class InputInterfaceEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label); 
            int type = property.FindPropertyRelative("type").enumValueIndex;

            #region Key
            var prop = property.FindPropertyRelative("key");
            Rect keyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            prop.stringValue = EditorGUI.TextField(keyRect, prop.stringValue);
            #endregion

            #region Type
            prop = property.FindPropertyRelative("type");
            Rect typeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width / 2 - 2, EditorGUIUtility.singleLineHeight);
            type = EditorGUI.Popup(typeRect, type, prop.enumDisplayNames);
            prop.enumValueIndex = type;
            #endregion

            #region Value
            Rect valueRect = new Rect(position.x + position.width / 2, position.y + EditorGUIUtility.singleLineHeight + 2, position.width / 2, EditorGUIUtility.singleLineHeight);
            switch ((InputInterfaceEntryType)type)
            {
                case InputInterfaceEntryType.Key:
                    prop = property.FindPropertyRelative("keyCode");
                    prop.enumValueIndex = EditorGUI.Popup(valueRect,prop.enumValueIndex, property.FindPropertyRelative("keyCode").enumDisplayNames);
                    break;
                case InputInterfaceEntryType.Axis:
                    prop = property.FindPropertyRelative("axis");
                    prop.stringValue = EditorGUI.TextField(valueRect, prop.stringValue);
                    break;
                case InputInterfaceEntryType.MouseButton:
                    prop = property.FindPropertyRelative("mouseButton");
                    prop.intValue = EditorGUI.IntField(valueRect, prop.intValue);
                    break;
            }
            #endregion

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 6;
        }
    }
    #endif
}

