using UnityEngine;

public class Player : Unit
{
	private Tile[] previousTiles;

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
		Debug.Log("Player start turn");
		ShowWalkableTiles();
	}

	public override void EndTurn()
	{
		base.EndTurn();
		Debug.Log("Player end turn");
	}

	protected override void StartMoving()
	{
		base.StartMoving();
	}

	protected override void DoneMoving()
	{
		base.DoneMoving();
		ShowWalkableTiles();
	}

	private void ShowWalkableTiles()
	{
		Tile[] tiles = GetWalkableTiles();
		foreach (Tile tile in tiles)
		{
			tile.SetTileType(TileType.Walkable);
		}
	}
}