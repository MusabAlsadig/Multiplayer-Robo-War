using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Data;
using InputManagement;

/// <summary>
/// This class will handle calling only one method when a key is pressed <br/>
/// it work together with <see cref="PlayerInput"/>
/// </summary>
public static class EventManager
{
    private static Dictionary<Controls, Action> Actions = new Dictionary<Controls, Action>();
    private static Dictionary<Controls, Action> ClickStartedActions = new Dictionary<Controls, Action>();
    private static Dictionary<Controls, Action> ClickCancledActions = new Dictionary<Controls, Action>();
    private static Dictionary<Controls, Action> ImportantActions = new Dictionary<Controls, Action>();
    private static Dictionary<Rebindable, InputAction> rebindableInputs = new Dictionary<Rebindable, InputAction>();
    private static InputControls controls;


    public static void Setup(InputControls _controls)
    {
        Actions[Controls.Fire] = () => { };
        ImportantActions[Controls.Fire] = () => { };
        ClickStartedActions[Controls.Fire] = () => { };
        ClickCancledActions[Controls.Fire] = () => { };

        Actions[Controls.Reload] = () => { };
        ImportantActions[Controls.Reload] = () => { };
        ClickStartedActions[Controls.Reload] = () => { };
        ClickCancledActions[Controls.Reload] = () => { };

        Actions[Controls.Thruster] = () => { };
        ImportantActions[Controls.Thruster] = () => { };
        ClickStartedActions[Controls.Thruster] = () => { };
        ClickCancledActions[Controls.Thruster] = () => { };
    }

    #region Input Events


    public enum Controls 
    {
        Fire, Reload, Thruster
    }
    public static void ActionStartHandler(Controls controls)
    {
		//if (settings.logActions)
        	Debug.Log($"preforming action of <color=blue>{controls}</color>");

        var a = ClickStartedActions[controls];
        if (a == null)
            return;

        var list = a.GetInvocationList();
        if (list.Length == 0)
            return;
        var lastMethod = list[list.Length - 1];
		
		//if (settings.logActions)
        	Debug.Log($"invoking method called {lastMethod.Method.Name}");
			
        lastMethod.DynamicInvoke();
        
    }
    
    public static void ActionCancledHandler(Controls controls)
    {
		//if (settings.logActions)
        	Debug.Log($"preforming action of <color=blue>{controls}</color>");

        var a = ClickCancledActions[controls];
        if (a == null)
            return;

        var list = a.GetInvocationList();
        if (list.Length == 0)
            return;
        var lastMethod = list[list.Length - 1];
		
		//if (settings.logActions)
        	Debug.Log($"invoking method called {lastMethod.Method.Name}");
			
        lastMethod.DynamicInvoke();
        
    }
    
    public static void ActionHandler(Controls controls)
    {
		//if (settings.logActions)
        	Debug.Log($"preforming action of <color=blue>{controls}</color>");

        //#region Check for Important actions first
        //var important = ImportantActions[controls];
        //if (important != null)
        //{
        //    var allMethods = important.GetInvocationList();

        //    if (allMethods.Length != 0)
        //    {

        //        var _lastMethod = allMethods[allMethods.Length - 1];
        //        //if (settings.logActions)
        //            Debug.Log($"invoking <color=green>Important</color> method called {_lastMethod.Method.Name}");
        //        _lastMethod.DynamicInvoke();

        //        return;
        //    }
        //}
        //#endregion

        var a = Actions[controls];
        if (a == null)
            return;

        var list = a.GetInvocationList();
        if (list.Length == 0)
            return;
        var lastMethod = list[list.Length - 1];
		
		//if (settings.logActions)
        	Debug.Log($"invoking method called {lastMethod.Method.Name}");
			
        lastMethod.DynamicInvoke();
        
    }

    /// <summary>
    /// <paramref name="method"/> will be called if it's the last one assigned
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="method"></param>
    /// <param name="isImportant">should this method ignore all not important ones after it?</param>
    public static void Assign(Controls controls, Action method)
    {
        if (!Actions.ContainsKey(controls))
            Actions[controls] = method;
        else
            Actions[controls] += method;
    }
    
    public static void AssignOnClick(Controls controls, Action method)
    {
        if (!ClickStartedActions.ContainsKey(controls))
            ClickStartedActions[controls] = method;
        else
            ClickStartedActions[controls] += method;
    }
    
    public static void AssignOnCancle(Controls controls, Action method)
    {
        if (!ClickCancledActions.ContainsKey(controls))
            ClickCancledActions[controls] = method;
        else
            ClickCancledActions[controls] += method;
    }


    /// <summary>
    /// <paramref name="method"/> will be called instead of any other method of same <see cref="controls"/>
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="method"></param>
    /// <param name="isImportant">should this method ignore all not important ones after it?</param>
    public static void AssignImportant(Controls controls, Action method)
    {
        if (!ImportantActions.ContainsKey(controls))
            ImportantActions[controls] = method;
        else
            ImportantActions[controls] += method;
    }

    /// <summary>
    /// Remove <paramref name="method"/> from invocation list
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="method"></param>
    /// <param name="isImportant"></param>
    public static void Remove(Controls controls, Action method)
    {
        Actions[controls] -= method;
    }

    /// <summary>
    /// Remove <paramref name="method"/> from invocation list
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="method"></param>
    /// <param name="isImportant"></param>
    public static void RemoveOnClick(Controls controls, Action method)
    {
        ClickStartedActions[controls] -= method;
    }

    /// <summary>
    /// Remove <paramref name="method"/> from invocation list
    /// </summary>
    /// <param name="controls"></param>
    /// <param name="method"></param>
    /// <param name="isImportant"></param>
    public static void RemoveOnCancle(Controls controls, Action method)
    {
        ClickCancledActions[controls] -= method;
    }

    public static void RemoveImportant(Controls controls, Action method)
    {
        ImportantActions[controls] -= method;
    }


    public static void Disable(Controls controls)
    {
        AssignImportant(controls, DoNothing);
    }

    public static void Enable(Controls controls)
    {
        RemoveImportant(controls, DoNothing);
    }

    private static void DoNothing() => Debug.LogWarning("Doing nothing with this input");

    #endregion

    #region Rebinding methods

    public enum Rebindable 
    { Fire, Reload, Thruster }

    /// <summary>
    /// change the key that used to activate this type of methods
    /// </summary>
    /// <param name="rebindable"></param>
    /// <param name="onComplete"></param>
    /// <param name="onCancle"></param>
    /// <param name="bindingIndex"></param>
    public static void Rebind(Rebindable rebindable, Action<string> onComplete, Action onCancle = null, int bindingIndex = 0)
    {
        InputAction inputAction = rebindableInputs[rebindable];
        if (inputAction == null)
        {
            //InfoShower.instance.QuickMessage("no action found for " + rebindable);
            return;
        }

        controls.Player.Disable();

        var oldBinding = inputAction.bindings[bindingIndex];

        // setup the rebinding before starting it
        var rebind = new InputActionRebindingExtensions.RebindingOperation().WithAction(inputAction).
                WithCancelingThrough("<Keyboard>/escape").
                OnComplete(callback =>
               {
                   
                   callback.Dispose();
                   controls.Player.Enable();

                   if (CheckDublicatedBindings(inputAction, bindingIndex))
                   {
                       inputAction.ApplyBindingOverride(bindingIndex, oldBinding);
                       Rebind(rebindable, onComplete);
                   }
                   else
                       onComplete?.Invoke(InputActionRebindingExtensions.GetBindingDisplayString(inputAction));
               }).
               OnCancel(callback =>
               {
                   callback.Dispose();
                   controls.Player.Enable();

                   inputAction.ApplyBindingOverride(0, oldBinding);
                   //InfoShower.instance.QuickMessage("<color=red>Cancled</color>");
                   onCancle?.Invoke();
               }
               );

        // start rebinding
        rebind.Start();

    }

    

    public static void RemoveBindingOverride(Rebindable rebindable, int bindingIndex = 0)
    {
        InputAction inputAction = rebindableInputs[rebindable];
        inputAction.RemoveBindingOverride(bindingIndex);
    }

    private static bool CheckDublicatedBindings(InputAction _inputAction, int bindingIndex = 0, bool allCompositeParts = false)
    {
        InputBinding newBinding = _inputAction.bindings[bindingIndex];
        foreach (InputBinding binding in _inputAction.actionMap.bindings)
        {
            if (binding.action == newBinding.action)
                continue;

            if (binding.effectivePath == newBinding.effectivePath)
            {
                //InfoShower.instance.Console("DublicatedBinding",Color.red);
                return true;
            }
        }
        return false;
    }

    public static string GetBindingDisplayString(Rebindable rebindable) =>
        InputActionRebindingExtensions.GetBindingDisplayString(rebindableInputs[rebindable]);

    #endregion

    //#region Save && Load
    //public static SaveableDictionary<Rebindable, string> GetBindings()
    //{
    //    SaveableDictionary<Rebindable, string> result = new SaveableDictionary<Rebindable, string>();
    //    Rebindable type;
    //    InputAction action;
    //    foreach (var pair in rebindableInputs)
    //    {
    //        type = pair.Key;
    //        action = pair.Value;
    //        result.Add(type, action.bindings[0].effectivePath);
    //    }

    //    return result;
    //}

    //public static void ApplyBindings(SaveableDictionary<Rebindable, string> newBindings)
    //{
    //    Rebindable type;
    //    string path;
    //    InputAction action;
    //    foreach (var key in newBindings.keys)
    //    {
    //        type = key;
    //        path = newBindings.GetValue(key);

    //        action = rebindableInputs[type];

    //        action.ApplyBindingOverride(0, path);
    //    }
    //}
    //#endregion
}
