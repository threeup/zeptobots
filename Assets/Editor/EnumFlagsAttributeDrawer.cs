using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 



#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( EnumFlagsAttribute ) )]
public sealed class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI( 
        Rect position, 
        SerializedProperty prop, 
        GUIContent label 
    )
    {
        var buttonsIntValue = 0;
        var enumLength      = prop.enumNames.Length;
        var labelWidth      = EditorGUIUtility.labelWidth;
        var buttonPressed   = new bool[ enumLength ];
        var buttonWidth     = ( position.width - labelWidth ) / enumLength;
 
        var labelPos = new Rect( 
            position.x, 
            position.y, 
            labelWidth, 
            position.height 
        );
        EditorGUI.LabelField( labelPos, label );
        EditorGUI.BeginChangeCheck();
 
 		var buttonPos = new Rect(
                position.x + labelWidth, 
                position.y, 
                buttonWidth, 
                position.height
            );

 		buttonPressed[ 0 ] = prop.intValue == 0;
 		buttonPressed[ 0 ] = GUI.Toggle( 
                buttonPos, 
                buttonPressed[ 0 ], 
                prop.enumNames[ 0 ], 
                "Button" 
        );

        if ( buttonPressed[ 0 ] )
        {
            buttonsIntValue = 0;
        }
        for ( int i = 1; i < enumLength; i++ )
        {
        	int shift = 1 << (i-1);
            buttonPressed[ i ] = ( prop.intValue & shift ) == shift;
 
            buttonPos.x = position.x + labelWidth + buttonWidth * i;
                
 
            buttonPressed[ i ] = GUI.Toggle( 
                buttonPos, 
                buttonPressed[ i ], 
                prop.enumNames[ i ], 
                "Button" 
            );
 
            if ( buttonPressed[ i ] )
            {
                buttonsIntValue += shift;
            }
        }
 
        if ( EditorGUI.EndChangeCheck() )
        {
            prop.intValue = buttonsIntValue;
        }
    }
}
#endif