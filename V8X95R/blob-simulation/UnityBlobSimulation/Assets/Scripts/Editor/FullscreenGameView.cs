#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class FullscreenGameView
{
    static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
    static readonly PropertyInfo ShowToolbarProperty =
        GameViewType?.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);

    [MenuItem("Window/General/Game (Fullscreen) %G", priority = 2)]
    public static void Toggle()
    {
        if (GameViewType == null)
        {
            Debug.LogError("GameView type not found.");
            return;
        }

        EditorWindow gameView = EditorWindow.GetWindow(GameViewType);
        if (gameView == null)
            return;

        //ShowToolbarProperty?.SetValue(gameView, false);

        gameView.Show();
        gameView.maximized = !gameView.maximized;
        gameView.Focus();
    }
}

#endif