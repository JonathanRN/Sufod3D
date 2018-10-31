using System.Collections.Generic;
using UnityEngine;

public class NpcPathfinding : MonoBehaviour
{
	private TacticsMove tacticsMove;
	private CombatStats combatStats;
	private TurnManagerTeam turnManagerTeam;
	
	public Tile ActualTargetTile { get; set; }

	private void Awake()
	{
		tacticsMove = GetComponentInParent<TacticsMove>();
		turnManagerTeam = GameObject.FindWithTag("GameController").GetComponent<TurnManagerTeam>();
		combatStats = GetComponent<CombatStats>();
	}

	private Tile FindLowestF(List<Tile> list)
	{
		var lowest = list[0];

		foreach (var t in list)
			if (t.F < lowest.F)
				lowest = t;

		list.Remove(lowest);

		return lowest;
	}

	private Tile FindEndTile(Tile t)
	{
		var tempPath = new Stack<Tile>();

		var next = t.Parent;

		while (next != null)
		{
			tempPath.Push(next);
			next = next.Parent;
		}

		if (tempPath.Count <= combatStats.MovementPoints)
			return t.Parent;

		Tile endTile = null;

		for (var i = 0; i <= combatStats.MovementPoints; i++)
			endTile = tempPath.Pop();

		return endTile;
	}

	public void FindPath(Tile target)
	{
		tacticsMove.ComputeWalkableAdjacencyLists(target);
		tacticsMove.GetCurrentTile();

		var openList = new List<Tile>();
		var closedList = new List<Tile>();

		openList.Add(tacticsMove.CurrentTile);
		tacticsMove.CurrentTile.H = Vector3.Distance(tacticsMove.CurrentTile.transform.position, target.transform.position);
		tacticsMove.CurrentTile.F = tacticsMove.CurrentTile.H;

		while (openList.Count > 0)
		{
			var t = FindLowestF(openList);

			closedList.Add(t);

			if (t == target)
			{
				ActualTargetTile = FindEndTile(t);
				tacticsMove.MoveToTile(ActualTargetTile);
				return;
			}

			foreach (var tile in t.AdjacencyList)
				if (closedList.Contains(tile))
				{
					//Do nothing, already processed
				}
				else if (openList.Contains(tile))
				{
					var tempG = t.G + Vector3.Distance(tile.transform.position, t.transform.position);

					if (tempG < tile.G)
					{
						tile.Parent = t;

						tile.G = tempG;
						tile.F = tile.G + tile.H;
					}
				}
				else
				{
					tile.Parent = t;

					tile.G = t.G + Vector3.Distance(tile.transform.position, t.transform.position);
					tile.H = Vector3.Distance(tile.transform.position, target.transform.position);
					tile.F = tile.G + tile.H;

					openList.Add(tile);
				}
		}

		//todo - what do you do if there is no path to the target tile?
		Debug.Log("Path not found");
		//End turn for now
		turnManagerTeam.EndTurn();
	}
	
	public GameObject FindNearestTarget()
	{
		var targets = GameObject.FindGameObjectsWithTag("Player");

		GameObject nearest = null;
		var distance = Mathf.Infinity;

		foreach (var obj in targets)
		{
			var d = Vector3.Distance(transform.position, obj.transform.position);

			if (d < distance)
			{
				distance = d;
				nearest = obj;
			}
		}

		return nearest;
	}

}