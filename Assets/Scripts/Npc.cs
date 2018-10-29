using UnityEngine;

public class Npc : TacticsMove
{
	private GameObject target;
	private NpcPathfinding npcPathfinding;

	protected override void Awake()
	{
		base.Awake();
		npcPathfinding = GetComponentInChildren<NpcPathfinding>();
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		Debug.DrawRay(transform.position, transform.forward);

		if (!IsItsTurn)
			return;

		if (!IsMoving)
		{
			target = npcPathfinding.FindNearestTarget();
			CalculatePath();
			FindSelectableTiles();
			npcPathfinding.ActualTargetTile.Target = true;
		}
		else
			Move();
	}

	protected override void OnFinishedMoving()
	{
		base.OnFinishedMoving();
		//if (CombatStats.IsOutOfMovementPoints)
			TurnManager.EndTurn();
	}

	private void CalculatePath()
	{
		var targetTile = GetTargetTile(target);
		npcPathfinding.FindPath(targetTile);
	}
}