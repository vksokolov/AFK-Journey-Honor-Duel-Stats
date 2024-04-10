using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private Dictionary<Type, Func<IService>> _ctors;
    private  Dictionary<Type, IService> _services;
    
    public static ServiceLocator Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        _ctors = new Dictionary<Type, Func<IService>>();
        _services = new Dictionary<Type, IService>();
    }
    
    public void Register<T>(Func<T> ctor) where T : IService
    {
        _ctors[typeof(T)] = () => ctor();
    }
    
    public T Get<T>() where T : IService
    {
        if (!_services.ContainsKey(typeof(T)))
        {
            _services[typeof(T)] = _ctors[typeof(T)]();
        }
        
        return (T) _services[typeof(T)];
    }
}
