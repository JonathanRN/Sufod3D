using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Queue : MonoBehaviour
{
	[SerializeField]
	private GameObject itemPrefab;

	private Queue<Unit> queue;

	private void OnEnable()
	{
		TurnManager.OnQueueUpdated += OnQueueUpdate;
	}

	private void OnDisable()
	{
		TurnManager.OnQueueUpdated -= OnQueueUpdate;
	}

	private void OnQueueUpdate()
	{
		// TODO dont destroy/instantiate
		DestroyAllChildren();
		
		queue = TurnManager.Instance.TurnUnits;
		
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