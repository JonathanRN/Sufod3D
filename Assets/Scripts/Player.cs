using UnityEngine;

public class Player : TacticsMove
{
	private bool shouldMoveThisFrame;
	
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
			if (!Input.GetKeyDown(KeyCode.Alpha1) && !IsAttacking)
			{
				FindSelectableTiles();
				MoveToTileOnMouseClick();
			}
			else
			{
				IsAttacking = true;
				FindAttackableTiles();
				AttackTileUnderMouse();
			}
			
			EndTurnOnInput(Input.GetKeyUp(KeyCode.Space));
		}
		else
		{
			shouldMoveThisFrame = true;
		}
	}

	private void EndTurnOnInput(bool input)
	{
		if (input)
			TurnManager.EndTurn();
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
						AttackTile(t);
				}
		}
	}
}