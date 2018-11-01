using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public bool Walkable { private get; set; }
	public bool Current { private get; set; }
	public bool Target { private get; set; }
	public bool Selectable { get; set; }
	public bool Attackable { get; set; }
	public bool AOETouched { get; set; }

	public List<Tile> AdjacencyList = new List<Tile>();
	public List<Tile> AOEAdjencyList = new List<Tile>();

	//Needed BFS (breadth first search)
	public bool Visited { get; set; }
	public Tile Parent { get; set; }
	public int Distance { get; set; }

	//For A*
	public float F { get; set; }
	public float G { get; set; }
	public float H { get; set; }

	private Vector3 halfExtents;

	private void Awake()
	{
		Walkable = true;
		halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
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
		else if (AOETouched)
			ChangeMaterialColor(Color.blue);
		else
			ChangeMaterialColor(Color.white);
	}

	private void ChangeMaterialColor(Color color)
	{
		GetComponent<Renderer>().material.color = color;
	}

	public void Reset(bool isAOE)
	{
		if(isAOE)
			AOEAdjencyList.Clear();
		else
			AdjacencyList.Clear();

		Current = false;
		Target = false;
		Selectable = false;
		AOETouched = false;
		if(!isAOE)
			Attackable = false;

		Visited = false;
		Parent = null;
		Distance = 0;

		F = G = H = 0;
	}

	public void FindNeighborsWalkableTiles(Tile target)
	{
		Reset(false);

		CheckWalkableTile(Vector3.forward, target);
		CheckWalkableTile(-Vector3.forward, target);
		CheckWalkableTile(Vector3.right, target);
		CheckWalkableTile(-Vector3.right, target);
	}
	
	public void FindNeighborsTiles(List<Tile> adjencyList,bool isAOE)
	{
		Reset(isAOE);

		CheckTile(Vector3.forward,adjencyList);
		CheckTile(-Vector3.forward,adjencyList);
		CheckTile(Vector3.right,adjencyList);
		CheckTile(-Vector3.right,adjencyList);
	}

	public void CheckWalkableTile(Vector3 direction, Tile target)
	{
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

	private void CheckTile(Vector3 direction,List<Tile> adjencyList)
	{
		var colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

		adjencyList.AddRange(colliders.Select(item => item.GetComponent<Tile>()).Where(tile => tile != null));
	}
	
	

	public bool IsObjectOnTopOfTile(out RaycastHit hit)
	{
		return Physics.Raycast(transform.position, Vector3.up, out hit, 1);
	}
}