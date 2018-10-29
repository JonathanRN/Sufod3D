using System.Linq;
using Boo.Lang;
using UnityEngine;

public class Player : TacticsMove
{
	private bool shouldMoveThisFrame;
	private Ability currentAbility;
	
	private void Start()
	{
		Init();
	}

	private void Update()
	{
		Debug.DrawRay(transform.position, transform.forward);
		shouldMoveThisFrame = false;

		if (!IsItsTurn)
			return;
		
		if (IsMoving == false)
		{
			if (!WantToAttack() && !IsAttacking)
			{
				FindSelectableTiles();
				MoveToTileOnMouseClick();
			}
			else
			{
				IsAttacking = true;
				FindAttackableTiles(currentAbility);
				AttackTileUnderMouse();
			}
			
			EndTurnOnInput(Input.GetKeyUp(KeyCode.Space));
		}
		else
		{
			shouldMoveThisFrame = true;
		}
	}

	//todo rename that bitch
	private bool WantToAttack()
	{	
		if (Input.GetKeyDown(KeyCode.Alpha1))
			currentAbility = abilities.ElementAt(0);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			currentAbility = currentAbility = abilities.ElementAt(1);

		return currentAbility != null && CombatStats.CanUseAbility(currentAbility);
	}

	private void EndTurnOnInput(bool input)
	{
		if (input)
		{
			currentAbility = null;
			TurnManager.EndTurn();
		}
	}

	// For Physics Updates
	private void FixedUpdate()
	{
		if (shouldMoveThisFrame)
		{
			Move();
		}
	}

	private void MoveToTileOnMouseClick()
	{
		if (Input.GetMouseButtonUp(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
				if (hit.collider.CompareTag("Tile"))
				{
					var t = hit.collider.GetComponent<Tile>();

					if (t.Selectable)
						MoveToTile(t);
				}
		}
	}

	private void AttackTileUnderMouse()
	{
		if (Input.GetMouseButtonUp(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
				if (hit.collider.CompareTag("Tile"))
				{
					var t = hit.collider.GetComponent<Tile>();

					if (t.Attackable)
					{
						AttackTile(t, currentAbility);
						currentAbility = null;
					}
				}
		}
	}
}