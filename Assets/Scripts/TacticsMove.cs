using System.Collections.Generic;
using UnityEngine;
using WeSoLit;
using WeSoLit.Perso1;

public class TacticsMove : MonoBehaviour
{
	// TODO: MOVE STUFF OUT OF THIS, ITS HUGE!
	
	[Header("Variables")]
	[SerializeField] private bool isMoving;
	[SerializeField] private bool isAttacking;
	[SerializeField] private float moveSpeed = 4;

	protected TurnManager TurnManager;
	protected CombatStats CombatStats;
	protected List<Ability> Abilities;
	protected Health health;

	private readonly List<Tile> selectableTiles = new List<Tile>();
	protected readonly List<Tile> attackableTiles = new List<Tile>();

	private readonly Stack<Tile> path = new Stack<Tile>();

	private GameObject[] tiles;
	
	private Vector3 velocity;
	private Vector3 heading;
	private float halfHeight;
	private int nbOfTilesCrossed = 0;
	
	public Tile CurrentTile { get; set; }
	protected bool IsItsTurn { get; private set; }

	public bool IsMoving
	{
		protected get { return isMoving; }
		set { isMoving = value; }
	}

	public bool IsAttacking
	{
		get { return isAttacking; }
		set { isAttacking = value; }
	}

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
	
	protected virtual void Awake()
	{
		TurnManager = GameObject.FindWithTag("GameController").GetComponent<TurnManager>();
		CombatStats = GetComponentInChildren<CombatStats>();
		health = GetComponentInChildren<Health>();
	}

	protected void Init()
	{
		IsItsTurn = false;
		tiles = GameObject.FindWithTag("Grid").GetComponent<Grid>().Tiles;
		Abilities = new List<Ability>();
		halfHeight = GetComponent<Collider>().bounds.extents.y;
		TurnManager.AddUnit(gameObject);
	}

	protected virtual void Update()
	{
		if (health.IsDead)
		{
			TurnManager.RemoveUnit(gameObject);
			Destroy(gameObject);
		}
	}

	public void GetCurrentTile()
	{
		CurrentTile = GetTargetTile(gameObject);
		CurrentTile.Current = true;
	}

	protected Tile GetTargetTile(GameObject target)
	{
		RaycastHit hit;
		Tile tile = null;

		if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
			tile = hit.collider.GetComponent<Tile>();

		return tile;
	}

	public void ComputeWalkableAdjacencyLists(Tile target)
	{
		foreach (var tile in tiles)
		{
			var t = tile.GetComponent<Tile>();
			t.FindNeighborsWalkableTiles(target);
		}
	}
	
	public void ComputeAttackableAdjacencyLists(Tile target)
	{
		foreach (var tile in tiles)
		{
			var t = tile.GetComponent<Tile>();
			t.FindNeighborsAttackableTiles(target);
		}
	}

	public void FindSelectableTiles()
	{
		ComputeWalkableAdjacencyLists(null);
		GetCurrentTile();

		var process = new Queue<Tile>();

		process.Enqueue(CurrentTile);
		CurrentTile.Visited = true;

		while (process.Count > 0)
		{
			var t = process.Dequeue();

			selectableTiles.Add(t);
			t.Selectable = true;

			if (t.Distance < CombatStats.MovementPoints)
				foreach (var tile in t.AdjacencyList)
					if (!tile.Visited)
					{
						tile.Parent = t;
						tile.Visited = true;
						tile.Distance = 1 + t.Distance;
						process.Enqueue(tile);
					}
		}
	}

	public void FindAttackableTiles(Ability ability)
	{
		ComputeAttackableAdjacencyLists(null);
		GetCurrentTile();

		var process = new Queue<Tile>();

		process.Enqueue(CurrentTile);
		CurrentTile.Visited = true;

		while (process.Count > 0)
		{
			var t = process.Dequeue();

			attackableTiles.Add(t);
			t.Attackable = true;

			var currentAttackRange = ability.IsAffectedByPlayerRange
				? ability.Range + CombatStats.AttackRange
				: ability.Range;
				
			if (t.Distance < currentAttackRange)
				foreach (var tile in t.AdjacencyList)
					if (!tile.Visited)
					{
						tile.Parent = t;
						tile.Visited = true;
						tile.Distance = 1 + t.Distance;
						process.Enqueue(tile);
					}
		}
	}

	public void MoveToTile(Tile tile)
	{
		path.Clear();
		tile.Target = true;
		IsMoving = true;

		var next = tile;

		while (next != null)
		{
			path.Push(next);
			next = next.Parent;
		}
	}

	public void Move()
	{
		if (path.Count > 0)
		{
			var t = path.Peek();
			var target = t.transform.position;
			
			//Calculate the unit's position on top of the target tile
			target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;
			
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
			OnFinishedMoving();
		}
	}

	protected virtual void OnFinishedMoving()
	{
		ClearTilesList(selectableTiles);
		IsMoving = false;
		CombatStats.RemoveMovementPoints(nbOfTilesCrossed - 1); //bug for some reason it adds +1 once
		nbOfTilesCrossed = 0;
	}

	protected void AttackTile(Tile tile, Ability ability)
	{
		CombatStats.RemoveAbilityPoints(ability.ApCost);
		
		ClearTilesList(attackableTiles);
		IsAttacking = false;

		CheckObjectAttackedOnTile(tile, ability);
	}

	private void CheckObjectAttackedOnTile(Tile tile, Ability ability)
	{
		RaycastHit hit;

		if (tile.IsObjectOnTopOfTile(out hit))
		{
			Debug.Log($"{hit.collider.gameObject} got hit with {ability.Name}!");
			hit.collider.GetComponentInChildren<Health>().Hit(ability.Damage);	
		}
	}

	private void ClearTilesList(List<Tile> list)
	{
		if (CurrentTile != null)
		{
			CurrentTile.Current = false;
			CurrentTile = null;
		}

		foreach (var tile in list)
			tile.Reset();

		list.Clear();
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

	public void BeginTurn()
	{
		IsItsTurn = true;
		CombatStats.Reset();
	}

	public void EndTurn()
	{
		IsItsTurn = false;
	}
}