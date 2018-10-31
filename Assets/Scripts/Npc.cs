using UnityEngine;

public delegate Event OnNpcMouseOver();
public class Npc : TacticsMove
{
	private GameObject target;
	private NpcPathfinding npcPathfinding;

	public event OnNpcMouseOver onNpcMouseOver;

	protected override void Awake()
	{
		base.Awake();
		npcPathfinding = GetComponentInChildren<NpcPathfinding>();
	}

	private void Start()
	{
		Init();
	}

	protected override void Update()
	{
		base.Update();
		
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
		//if (CombatStats.IsOutOfMovementPoints) todo endturn after enemy is done attacking or something else
			TurnManager.EndTurn();
	}

	private void CalculatePath()
	{
		var targetTile = GetTargetTile(target);
		npcPathfinding.FindPath(targetTile);
	}

	private void OnMouseOver()
	{
		NotifyOnMouseOver();
	}

	private void NotifyOnMouseOver()
	{
		if (onNpcMouseOver != null)
			onNpcMouseOver();
	}
}