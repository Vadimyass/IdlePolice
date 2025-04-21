using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Forwards Method Execution to run on a different Thread
/// <para>
/// Use this to run Methods on the Main Thread from another thread,
/// or to easily Asynchronously Exectue a method on a different thread
/// </para>
/// </summary>
public class Dispatcher : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// Singleton-Instance
    /// </summary>
    private static Dispatcher _instance;
    /// <summary>
    /// Bool used to track Queue between threads.
    /// </summary>
    private static volatile bool _queued = false;
    /// <summary>
    /// Backlog of Actions to run on next Update
    /// </summary>
    private static readonly Dictionary<Action<object[]>, object[]> _backlog = new Dictionary<Action<object[]>, object[]>(8);
    /// <summary>
    /// Backlog of Actions to run on next Update
    /// </summary>
    private static readonly List<Action> _noParamActionBacklog = new List<Action>();
    #endregion
 
    #region Methods
    #region Public
    /// <summary>
    /// Asynchronously Runs Action (using ThreadPool)
    /// </summary>
    /// <param name="action">Action to Run</param>
    public static void RunAsync(Action action)
    {
        try
        {
            ThreadPool.QueueUserWorkItem(o => action());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
    /// <summary>
    /// Asynchronously Runs Action With Parameter (using ThreadPool)
    /// </summary>
    /// <param name="action">Action to Run</param>
    /// <param name="state">Object-Parameter for Action</param>
    public static void RunAsync(Action<object> action, object state)
    {
        ThreadPool.QueueUserWorkItem(o => action(o), state);
    }
    /// <summary>
    /// Runs an Action on the Main Unity Thread (in the next Update)
    /// </summary>
    /// <param name="action">Action to Run</param>
    public static void RunOnMainThread(Action action)
    {
        lock (_noParamActionBacklog)
        {
            _noParamActionBacklog.Add(action);
            _queued = true;
        }
    }
    /// <summary>
    /// Runs an Action (with Parameters) on the Main Unity Thread (in the next Update)
    /// </summary>
    /// <param name="action">Action to Run</param>
    /// <param name="parameters">Parameters for Action</param>
    public static void RunOnMainThread(Action<object[]> action, object[] parameters)
    {
        lock (_backlog)
        {
            _backlog.Add(action, parameters);
            _queued = true;
        }
    }
    #endregion
    #region Private
    /// <summary>
    /// Automatically generates a Dispatcher-Object when the App is Started
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
            DontDestroyOnLoad(_instance.gameObject);
        }
    }
    /// <summary>
    /// Runs queued actions on Main Unity Thread
    /// </summary>
    private void Update()
    {
        if (_queued)
        {
            // Actions without Params
            List<Action> toPerform;
            lock (_noParamActionBacklog)
            {
                toPerform = new List<Action>(_noParamActionBacklog);
                _noParamActionBacklog.Clear();
            }
            for (int i = 0; i < toPerform.Count; i++)
                toPerform[i].Invoke();
             // Actions with Params
            Dictionary<Action<object[]>, object[]> _actions;
            lock (_backlog)
            {
                _actions = new Dictionary<Action<object[]>, object[]>(_backlog);
                _backlog.Clear();
            }
            foreach (KeyValuePair<Action<object[]>, object[]> action in _actions)
                action.Key.Invoke(action.Value);
            // Finished Queue
          _queued = false;
        }
    }
    #endregion
    #endregion
}