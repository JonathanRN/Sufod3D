using System.Linq;
using Boo.Lang;
using UnityEngine;

public class Player : TacticsMove
{
	private Ability currentAbility;
	
	private void Start()
	{
		Init();
	}

	protected override void Update()
	{
		base.Update();
		
		Debug.DrawRay(transform.position, transform.forward);

		if (!IsItsTurn) //bug if two player are side by side in the queue, the input gets called for both
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

			EndTurnOnInput(Input.GetKeyDown(KeyCode.Space));
		}
		else
		{
			Move();
		}
	}

	//todo rename that bitch
	private bool WantToAttack()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (currentAbility == Abilities.ElementAt(0))
			{
				IsAttacking = false;
				currentAbility = null;
				return false;
			}
			currentAbility = Abilities.ElementAt(0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if (currentAbility == Abilities.ElementAt(1))
			{
				IsAttacking = false;
				currentAbility = null;
				return false;
			}
			currentAbility = Abilities.ElementAt(1);
		}

		return currentAbility != null && CombatStats.CanUseAbility(currentAbility);
	}

	private void EndTurnOnInput(bool input)
	{
		if (input)
		{
			currentAbility = null;
			TurnManager.EndTurn();
			Debug.Log("End turn");
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
		if (!Input.GetMouseButtonUp(0))
			return;

		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.CompareTag("NPC"))
			{
				RaycastHit npcHit;

				// Check the tile under the unit to attack it
				if (Physics.Raycast(hit.collider.transform.position, -Vector3.up, out npcHit))
				{
					if (npcHit.collider.CompareTag("Tile"))
					{
						var tileUnder = npcHit.collider.GetComponent<Tile>();

						if (tileUnder.Attackable)
						{
							AttackTile(tileUnder, currentAbility);
							currentAbility = null;
						}
						else
							Debug.Log("Target too far away!");
					}
				}
			}
		}
	}
}