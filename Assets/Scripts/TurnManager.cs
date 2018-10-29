using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	private static readonly Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
	private static readonly Queue<string> turnKey = new Queue<string>();
	private static readonly Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();
	
	private void Update()
	{
		if (turnTeam.Count == 0)
			InitTeamTurnQueue();
	}

	private void InitTeamTurnQueue()
	{
		var teamList = units[turnKey.Peek()];

		foreach (var unit in teamList)
			turnTeam.Enqueue(unit);

		StartTurn();
	}

	private void StartTurn()
	{
		if (turnTeam.Count > 0)
			turnTeam.Peek().BeginTurn();
	}

	public void EndTurn()
	{
		var unit = turnTeam.Dequeue();
		unit.EndTurn();

		if (turnTeam.Count > 0)
		{
			StartTurn();
		}
		else
		{
			var team = turnKey.Dequeue();
			turnKey.Enqueue(team);
			InitTeamTurnQueue();
		}
	}

	public void AddUnit(TacticsMove unit)
	{
		List<TacticsMove> list;

		if (!units.ContainsKey(unit.tag))
		{
			list = new List<TacticsMove>();
			units[unit.tag] = list;

			if (!turnKey.Contains(unit.tag))
				turnKey.Enqueue(unit.tag);
		}
		else
		{
			list = units[unit.tag];
		}

		list.Add(unit);
	}

	//TODO remove units when they die
	public void RemoveUnit(TacticsMove unit)
	{
		
	}
}