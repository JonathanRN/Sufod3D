using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T instance;
	public static T Instance => instance;

	protected virtual void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Error while trying to Awake a singleton");
		}
		else
		{
			instance = (T)this;
		}
	}

	protected virtual void OnDestroy()
	{
		if (instance != null)
		{
			instance = null;
		}
	}
}
