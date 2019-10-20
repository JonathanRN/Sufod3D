using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : Singleton<GridManager>
{
	[SerializeField]
	private GameObject tilePrefab;

	[SerializeField]
	private int amountToGenerate;

	public List<Tile> Tiles { get; } = new List<Tile>();

	protected override void Awake()
	{
		base.Awake();
		GenerateGrid();
	}

	/// <summary>
	/// O2(n^2) ?
	/// </summary>
	private void GenerateGrid()
	{
		//First half
		for (int x = 0; x < amountToGenerate; x++)
		{
			InstantiateTile(new Vector3(x, 0, 0));

			if (x >= 1)
			{
				for (int z = 1; z <= x; z++)
				{
					InstantiateTile(new Vector3(x, 0, z));
					InstantiateTile(new Vector3(x, 0, -z));
				}
			}
		}
		
		//Second half
		for (int x = amountToGenerate*2; x >= amountToGenerate; x--)
		{
			InstantiateTile(new Vector3(x, 0, 0));

			if (x < amountToGenerate*2)
			{
				for (int z = 1; z <= (amountToGenerate*2 - x); z++)
				{
					InstantiateTile(new Vector3(x, 0, z));
					InstantiateTile(new Vector3(x, 0, -z));
				}
			}
		}
		
		Vector3 newPos = new Vector3(-amountToGenerate, 0, 0);
		transform.position = newPos;
	}

	private void InstantiateTile(Vector3 position)
	{
		GameObject go = Instantiate(tilePrefab, position, Quaternion.identity, transform);
		Tiles.Add(go.GetComponent<Tile>());
	}

	public Tile[] GetAdjacentTiles(Tile tileToCheck, int radius, bool resetTiles = false)
	{
		if (resetTiles)
		{
			ResetTiles();
		}

		List<Tile> adjacentTiles = new List<Tile>();
		Queue<Tile> process = new Queue<Tile>();

		process.Enqueue(tileToCheck);
		tileToCheck.Visited = true;

		while (process.Count > 0)
		{
			Tile tile = process.Dequeue();
			adjacentTiles.Add(tile);

			if (tile.Distance < radius)
			{
				List<Tile> neighbours = tile.FindNeighborsTiles();
				foreach (Tile next in neighbours)
				{
					if (!next.Visited)
					{
						next.Visited = true;
						next.Parent = tile;
						next.Distance = 1 + tile.Distance;
						if (!next.Occupied)
						{
							process.Enqueue(next);
						}
					}
				}
			}
		}

		return adjacentTiles.ToArray();
	}

	public Stack<Tile> GetPathTo(Tile tileToCheck)
	{
		Stack<Tile> path = new Stack<Tile>();
		Tile next = tileToCheck;
		while (next != null)
		{
			path.Push(next);
			next = next.Parent;
		}
		return path;
	}

	public Tile GetTargetTile(GameObject target)
	{
		Tile tile = null;
		RaycastHit hit;

		if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
		{
			tile = hit.collider.GetComponent<Tile>();
		}
		else
		{
			Debug.LogWarning("No tile found for target: " + target.name);
		}

		return tile;
	}

	public void ResetTiles()
	{
		foreach (Tile tile in Tiles)
		{
			tile.ResetTile();
		}
	}

	public Tile GetRandomTile(TileType type)
	{
		var sortedTiles = Tiles.Where(t => t.Type == type);
		return sortedTiles.ElementAt(Random.Range(0, sortedTiles.Count()));
	}
}