using System;
using System.Linq;
using Boo.Lang;
using UnityEngine;
using WeSoLit;
using WeSoLit.Perso1;

public class Player : TacticsMove
{
	private DamagePreviewCreator damagePreviewCreator;
	private Ability currentAbility;

	protected override void Awake()
	{
		base.Awake();
		damagePreviewCreator = GameObject.FindWithTag("GameController").GetComponent<DamagePreviewCreator>();
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
				PreviewAOETiles();
				AttackAOEUnderMouse();
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

	private void PreviewAOETiles()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
			if (hit.collider.CompareTag("Tile"))
			{
				var t = hit.collider.GetComponent<Tile>();

				if (t.Attackable)
					FindAOETiles(currentAbility,t);
					
			}
	}

	private void AttackTileUnderMouse()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (!Physics.Raycast(ray, out hit))
			return;

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
						CreateAndConfigureDamagePreview(hit.collider.gameObject);
						if (Input.GetMouseButtonUp(0))
						{
							AttackTile(tileUnder,currentAbility);
							currentAbility = null;
							damagePreviewCreator.DestroyIfExists();
						}
					}
				}
			}
		}
		else
		{
			damagePreviewCreator.DestroyIfExists();
		}
	}
	private void AttackAOEUnderMouse()
	{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit npcHit;
			// Check the tile under the unit to attack it
			if (Physics.Raycast(ray, out npcHit))
			{
				if (npcHit.collider.CompareTag("Tile"))
				{
					var tileUnder = npcHit.collider.GetComponent<Tile>();

					if (tileUnder.AOETouched)
					{
						if (Input.GetMouseButtonUp(0))
						{
							AttackAOETile(currentAbility);
							currentAbility = null;
							damagePreviewCreator.DestroyIfExists();
						}
					}
				}
			}
		

	}

	private void CreateAndConfigureDamagePreview(GameObject target)
	{
		var instance = damagePreviewCreator.InstantiateDamagePreview();

		if (instance != null)
		{
			var damagePreview = instance.GetComponent<DamagePreview>();
			damagePreview.SetDamageText($"- {currentAbility.Damage}");
			damagePreview.SetNameAndHealthText($"{target.name} ({target.GetComponentInChildren<Health>().HealthPoints})"); //todo Temporary values
		}
	}
}