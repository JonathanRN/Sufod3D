using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro mpPreviewText;

	private TileMaterial tileMaterial;
	private Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);

	public bool Uninteractable;

	public TileType Type { get; private set; }
	public bool Occupied => IsTileOccupied();

	//Needed BFS (breadth first search)
	public bool Visited { get; set; }
	public Tile Parent { get; set; }
	public int Distance { get; set; }

	//For A*
	public float F { get; set; }
	public float G { get; set; }
	public float H { get; set; }

	private void Awake()
	{
		tileMaterial = GetComponent<TileMaterial>();
	}

	public void ResetTile()
	{
		mpPreviewText.gameObject.SetActive(false);
		Visited = false;
		Parent = null;
		Distance = 0;

		F = G = H = 0;
		SetTileType(TileType.None);
	}

	public void SetPreviewNumber(int mp)
	{
		mpPreviewText.gameObject.SetActive(true);
		mpPreviewText.SetText(mp.ToString());
	}

	public void DisablePreviewText()
	{
		mpPreviewText.gameObject.SetActive(false);
		mpPreviewText.SetText(string.Empty);
	}

	public void SetTileType(TileType type)
	{
		TileType newType = TileType.None;
		switch (type)
		{
			case TileType.Walkable:
				if (Occupied || Uninteractable)
				{
					newType = TileType.Invalid;
				}
				else
				{
					newType = TileType.Walkable;
				}
				break;
			default:
				newType = type;
				break;
		}

		Type = newType;
		tileMaterial.SetTile(newType);
	}
	
	public List<Tile> FindNeighborsTiles()
	{
		List<Tile> adjacencyList = new List<Tile>();

		adjacencyList.AddRange(GetAdjacentTile(Vector3.forward));
		adjacencyList.AddRange(GetAdjacentTile(Vector3.back));
		adjacencyList.AddRange(GetAdjacentTile(Vector3.right));
		adjacencyList.AddRange(GetAdjacentTile(Vector3.left));

		return adjacencyList;
	}

	private IEnumerable<Tile> GetAdjacentTile(Vector3 direction)
	{
		var colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

		return colliders.Select(item => item.GetComponent<Tile>()).Where(tile => tile != null);
	}

	public float HalfHeight()
	{
		return GetComponent<Collider>().bounds.extents.y;
	}

	private bool IsTileOccupied()
	{
		return Physics.Raycast(transform.position, Vector3.up, 1);
	}

	public override string ToString()
	{
		string s = $"Position: {transform.position}\nType: {Type}\nOccupied: {Occupied}";
		return s;
	}
}