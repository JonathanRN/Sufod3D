using UnityEngine;

public class CombatInput : MonoBehaviour
{
	[SerializeField]
	private Player player;

	private PlayerPathPreview playerPathPreview;

	private Tile tileUnderMouse;
	private Tile lastFrameTile;

	private void Awake()
	{
		playerPathPreview = player.GetComponentInChildren<PlayerPathPreview>();
	}

	private void Update()
	{
		tileUnderMouse = GetTileUnderMouse();
		if (player.CurrentState == UnitState.Idle)
		{
			// Only show path preview when the tile under mouse gets updated
			if (lastFrameTile != tileUnderMouse)
			{
				playerPathPreview.ShowPathPreview(tileUnderMouse);
				lastFrameTile = tileUnderMouse;
			}

			CheckWalkInput();
			CheckEndOfTurnInput();
		}
		else if (player.CurrentState == UnitState.PickingSpawn)
		{
			CheckSpawnPickInput();
			CheckPlayerReadyInput();
		}
	}

	private void CheckSpawnPickInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (tileUnderMouse != null && tileUnderMouse.Type == TileType.Spawnable)
			{
				player.SetOnTile(tileUnderMouse);
			}
		}
	}

	private void CheckPlayerReadyInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (player.CurrentState == UnitState.PickingSpawn)
			{
				player.CurrentState = UnitState.Ready;
			}
		}
	}

	private void CheckWalkInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (tileUnderMouse != null)
			{
				Debug.Log(tileUnderMouse.ToString());
				if (tileUnderMouse.Type == TileType.WalkablePreview)
				{
					player.MoveToTile(tileUnderMouse);
					playerPathPreview.SetToNull();
				}
			}
		}
	}

	private void CheckEndOfTurnInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (player.IsItsTurn)
			{
				TurnManager.Instance.EndTurn();
			}
		}
	}

	private Tile GetTileUnderMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.CompareTag("Tile"))
			{
				Tile tile = hit.collider.GetComponent<Tile>();
				if (tile != null)
				{
					return tile;
				}
				else
				{
					Debug.LogError("Tile not found under mouse.");
				}
			}
		}
		return null;
	}
}