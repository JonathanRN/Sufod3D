using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Npc : Unit
{
	private Tile closestReachable;
	private bool targetReached;

	private void Start()
	{
		Init();
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void BeginTurn()
	{
		base.BeginTurn();
		Tile playerTile = FindClosestTarget();
		closestReachable = GetClosestReachableToTarget(playerTile);

		MoveToTile(closestReachable);
	}

	public override void EndTurn()
	{
		base.EndTurn();
	}

	protected override void DoneMoving()
	{
		base.DoneMoving();

		//TODO find a way to know if the enemy is already next to the player
		if (CurrentTile.FindNeighborsTiles().Contains(closestReachable))
		{
			Debug.Log("CAN BE REACHED!");
		}

		TurnManager.Instance.EndTurn();
	}

	public Tile GetClosestReachableToTarget(Tile target)
	{
		Tile[] tiles = GetWalkableTiles();
		return tiles.OrderBy(x => Vector3.Distance(x.transform.position, target.transform.position)).First();
	}

	public Tile FindClosestTarget()
	{
		// todo teams???
		// for now find the player
		return GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>().CurrentTile;
	}
}
