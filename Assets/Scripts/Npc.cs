using System.Collections.Generic;
using UnityEngine;
using WeSoLit;
using WeSoLit.Perso1;

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
		Abilities.Add(new Fireball());
		Abilities.Add(new Slash());
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

		if (CombatStats.IsOutOfActionPoints)
		{
			TurnManager.EndTurn();
		}
	}

	private void CheckIfCanAttack()
	{
		Ability abilityToCast = null;
		
		foreach (var ability in Abilities)
		{
			FindAttackableTiles(ability);

			if (CheckIfPlayerOnTiles(attackableTiles))
			{
				if (abilityToCast == null || abilityToCast.Priority < ability.Priority)
				{
					abilityToCast = ability;
				}
			}
		}

		if (abilityToCast != null)
			AttackTile(GetTargetTile(target), abilityToCast);
	}

	private bool CheckIfPlayerOnTiles(List<Tile> tiles) //bug last time i checked, 61 times were going in the loop
	{
		foreach (var tile in tiles)
		{
			if (tile.Attackable)
			{
				RaycastHit hit;
				if (tile.IsObjectOnTopOfTile(out hit))
				{
					if (hit.collider.CompareTag("Player"))
						return true;
				}
			}
		}

		return false;
	}

	protected override void OnFinishedMoving()
	{
		base.OnFinishedMoving();
		CheckIfCanAttack();
	}

	private void CalculatePath()
	{
		var targetTile = GetTargetTile(target);
		npcPathfinding.FindPath(targetTile);
	}
}