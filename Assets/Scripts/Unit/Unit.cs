using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
	[Header("Variables")]
	[SerializeField] private float moveSpeed = 4;

	[SerializeField]
	private UnitStats unitStats = null;

	private Stack<Tile> path = new Stack<Tile>();

	private Vector3 velocity;
	private Vector3 heading;
	private float halfHeight;
	private int nbOfTilesCrossed = -1;

	public UnitStats UnitStats => unitStats;
	public Tile CurrentTile => GridManager.Instance.GetTargetTile(gameObject);

	public bool IsItsTurn { get; private set; }
	public UnitState CurrentState { get; set; }

	public float MoveSpeed
	{
		get { return moveSpeed; }
		set { moveSpeed = value; }
	}

	public Vector3 Velocity
	{
		get { return velocity; }
		set { velocity = value; }
	}

	public Vector3 Heading
	{
		get { return heading; }
		set { heading = value; }
	}

	protected virtual void Update()
	{
		Debug.DrawRay(transform.position, transform.forward);
		if (CurrentState == UnitState.IsMoving)
		{
			Move();
		}
	}

	protected virtual void Awake()
	{
		halfHeight = GetComponent<Collider>().bounds.extents.y;
	}

	protected void Init()
	{
		TurnManager.Instance.AddUnit(this);
		unitStats.ResetAll();
	}

	public Tile[] GetWalkableTiles(int radius = -1)
	{
		if (radius == -1)
		{
			radius = unitStats.Mp;
		}
		return GridManager.Instance.GetAdjacentTiles(CurrentTile, radius, true);
	}

	public Stack<Tile> GetPathTo(Tile tile)
	{
		var path = GridManager.Instance.GetPathTo(tile);
		if (path.Count == 0)
		{
			Debug.LogWarning(gameObject.name + "couldn't find a path to " + tile.gameObject.name);
		}
		return path;
	}

	public void MoveToTile(Tile tile)
	{
		CurrentState = UnitState.IsMoving;
		StartMoving();

		path = GridManager.Instance.GetPathTo(tile);
	}

	protected void Move()
	{
		if (path.Count > 0)
		{
			var t = path.Peek();
			var target = t.transform.position;

			//Calculate the unit's position on top of the target tile
			target.y += GetHalfHeightFromTile(t);

			if (Vector3.Distance(transform.position, target) >= 0.05f)
			{
				CalculateHeading(target);
				SetHorizontalVelocity();

				//Locomotion
				transform.forward = heading;
				transform.position += velocity * Time.deltaTime;
			}
			else
			{
				//Tile center reached
				transform.position = target;
				path.Pop();
				nbOfTilesCrossed++;
			}
		}
		else
		{
			CurrentState = UnitState.Idle;
			DoneMoving();
		}
	}

	private float GetHalfHeightFromTile(Tile tile)
	{
		return halfHeight + tile.HalfHeight();
	}

	public void SetOnTile(Tile tile)
	{
		if (tile != null)
		{
			transform.position = tile.transform.position + GetHalfHeightFromTile(tile) * Vector3.up;
		}
	}

	public void CalculateHeading(Vector3 target)
	{
		heading = target - transform.position;
		heading.Normalize();
	}

	public void SetHorizontalVelocity()
	{
		velocity = heading * MoveSpeed;
	}

	protected virtual void DoneMoving()
	{
		unitStats.Mp -= nbOfTilesCrossed;
	}

	protected virtual void StartMoving()
	{
		nbOfTilesCrossed = -1; // Don't count the first one
		// Not all implement this
	}

	public virtual void BeginTurn()
	{
		IsItsTurn = true;
		CurrentState = UnitState.Idle;
		unitStats.NewTurn();
	}

	public virtual void EndTurn()
	{
		IsItsTurn = false;
		CurrentState = UnitState.WaitingForTurn;
	}
}