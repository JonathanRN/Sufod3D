using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Queue : MonoBehaviour
{
	[SerializeField] private GameObject itemPrefab;

	private TurnManager turnManager;
	private Queue<GameObject> queue;

	private void Awake()
	{
		turnManager = GameObject.FindWithTag("GameController").GetComponent<TurnManager>();
	}

	private void OnEnable()
	{
		turnManager.OnQueueUpdated += OnQueueUpdate;
	}

	private void OnDisable()
	{
		turnManager.OnQueueUpdated -= OnQueueUpdate;
	}

	private void OnQueueUpdate()
	{
		DestroyAllChildren();
		
		queue = turnManager.TurnUnit;
		
		foreach (var obj in queue)
		{
			var instance = Instantiate(itemPrefab, transform);
			instance.GetComponentInChildren<Text>().text = obj.name;
		}
	}

	private void DestroyAllChildren()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}