using System.Collections.Generic;
using System.Linq;
using Characters;
using UnityEngine;


public delegate void OnQueueUpdatedEventHandler();
public class TurnManager : MonoBehaviour
{
	private readonly List<GameObject> units = new List<GameObject>();
	private Queue<GameObject> turnUnit;
	
	private bool firstTurn = true;

	public Queue<GameObject> TurnUnit => turnUnit;
	public event OnQueueUpdatedEventHandler OnQueueUpdated;

	private void Update()
	{
		if (turnUnit.Count > 0 && firstTurn)
		{
			StartTurn();
			firstTurn = false;
		}
	}

	private void StartTurn()
	{
		if (turnUnit.Count > 0)
			turnUnit.Peek().GetComponent<TacticsMove>().BeginTurn();
	}

	public void EndTurn()
	{
		var unit = turnUnit.Dequeue();
		unit.GetComponent<TacticsMove>().EndTurn();

		turnUnit.Enqueue(unit);
		
		OnQueueUpdated?.Invoke();
		StartTurn();
	}
	
	public void AddUnit(GameObject unit)
	{
		units.Add(unit);
		SortList();
	}

	public void RemoveUnit(GameObject unit)
	{
		if (units.Contains(unit))
		{
			units.Remove(unit);
			SortList();
		}
	}

	private void SortList()
	{
		units.Sort((x, y) => y.GetComponentInChildren<CombatStats>().Speed.CompareTo(x.GetComponentInChildren<CombatStats>().Speed));
		turnUnit = new Queue<GameObject>(units);
		OnQueueUpdated?.Invoke();
	}
}