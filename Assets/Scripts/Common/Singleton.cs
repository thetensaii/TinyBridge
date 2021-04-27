using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:Component {

	static T _instance;

	public static T Instance
	{
		get { return _instance; }
	}

	[Header("Singleton")]
	[SerializeField] private bool m_DoNotDestroyGameObjectOnLoad;

	protected virtual void Awake()
	{
        if (_instance != null)
        {
            Debug.Log("Destroying : " + gameObject.name + " -- " + _instance.name);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Creating Singleton : " + gameObject.name);
            _instance = this as T;
            if (m_DoNotDestroyGameObjectOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}
