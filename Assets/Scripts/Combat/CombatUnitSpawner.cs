using System.Collections.Generic;
using UnityEngine;

public class CombatUnitSpawner : Singleton<CombatUnitSpawner>
{
	[SerializeField]
	private GameObject playerPrefab;

	// TODO CHANGE THIS ONE DAY
	[SerializeField]
	private GameObject npcPrefab;

	[SerializeField]
	private int spawnableTilesAmount;

	private List<Tile> spawnableTiles;

	private void OnEnable()
	{
		CombatManager.OnCombatInitialized += OnCombatInitialized;
	}

	private void OnDisable()
	{
		CombatManager.OnCombatInitialized -= OnCombatInitialized;
	}

	private void OnCombatInitialized()
	{
		CreateSpawnableTiles();
		SpawnPlayer(GridManager.Instance.GetRandomTile(TileType.Spawnable));
		SpawnEnemies(spawnableTiles.ToArray());
	}

	private void SpawnPlayer(Tile tile)
	{
		Player player = Instantiate(playerPrefab).GetComponent<Player>();
		player.SetOnTile(tile);
		player.CurrentState = UnitState.PickingSpawn;
	}

	private void SpawnEnemies(Tile[] tilesToSpawnOn)
	{
		for (int i = 0; i < tilesToSpawnOn.Length; i++)
		{
			if (!tilesToSpawnOn[i].Occupied)
			{
				Npc npc = Instantiate(npcPrefab).GetComponent<Npc>();
				npc.SetOnTile(tilesToSpawnOn[i]);
				npc.CurrentState = UnitState.Ready;
			}
		}
	}

	private void CreateSpawnableTiles()
	{
		spawnableTiles = new List<Tile>();
		// Need some better logic for spawn areas
		for (int i = 0; i < spawnableTilesAmount; i++)
		{
			Tile tile = GridManager.Instance.GetRandomTile(TileType.None);
			tile.SetTileType(TileType.Spawnable);
			spawnableTiles.Add(tile);
		}
	}
}
