using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public bool Walkable { private get; set; }
	public bool Current { private get; set; }
	public bool Target { private get; set; }
	public bool Selectable { get; set; }
	public bool Attackable { get; set; }

	public List<Tile> AdjacencyList = new List<Tile>();

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
		Walkable = true;
	}

	private void Update()
	{
		if (Current)
			ChangeMaterialColor(Color.magenta);
		else if (Target)
			ChangeMaterialColor(Color.green);
		else if (Selectable)
			ChangeMaterialColor(Color.red);
		else if (Attackable)
			ChangeMaterialColor(Color.yellow);
		else
			ChangeMaterialColor(Color.white);
	}

	private void ChangeMaterialColor(Color color)
	{
		GetComponent<Renderer>().material.color = color;
	}

	public void Reset()
	{
		AdjacencyList.Clear();

		Current = false;
		Target = false;
		Selectable = false;
		Attackable = false;

		Visited = false;
		Parent = null;
		Distance = 0;

		F = G = H = 0;
	}

	public void FindNeighborsWalkableTiles(Tile target)
	{
		Reset();

		CheckWalkableTile(Vector3.forward, target);
		CheckWalkableTile(-Vector3.forward, target);
		CheckWalkableTile(Vector3.right, target);
		CheckWalkableTile(-Vector3.right, target);
	}
	
	public void FindNeighborsAttackableTiles(Tile target)
	{
		Reset();

		CheckAttackableTile(Vector3.forward, target);
		CheckAttackableTile(-Vector3.forward, target);
		CheckAttackableTile(Vector3.right, target);
		CheckAttackableTile(-Vector3.right, target);
	}

	public void CheckWalkableTile(Vector3 direction, Tile target)
	{
		var halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
		var colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

		foreach (var item in colliders)
		{
			var tile = item.GetComponent<Tile>();

			if (tile != null && tile.Walkable)
			{
				RaycastHit hit;

				if (!tile.IsObjectOnTopOfTile(out hit) || tile == target)
					AdjacencyList.Add(tile);
			}
		}
	}

	public void CheckAttackableTile(Vector3 direction, Tile target)
	{
		var halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
		var colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

		foreach (var item in colliders)
		{
			var tile = item.GetComponent<Tile>();
			if (tile != null)
				AdjacencyList.Add(tile);
		}
	}

	public bool IsObjectOnTopOfTile(out RaycastHit hit)
	{
		return Physics.Raycast(transform.position, Vector3.up, out hit, 1);
	}
}