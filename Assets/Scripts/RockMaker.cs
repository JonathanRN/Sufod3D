using UnityEngine;

public class RockMaker : MonoBehaviour
{
	[SerializeField] private GameObject rockPrefab;
	[SerializeField] private int amount;

	private Tile[] tiles;

	private void Start()
	{
		tiles = GridManager.Instance.Tiles.ToArray();
		
		RandomlyGenerateRocks();
	}

	private void RandomlyGenerateRocks()
	{
		var i = 0;
		while (i < amount)
		{
			var tile = tiles[Random.Range(0, tiles.Length)];
			
			RaycastHit hit;

			// If there's nothing 1 meter up from the tile... todo change this
			if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
			{
                Instantiate(rockPrefab,
                           new Vector3(tile.transform.position.x, 0.75f, tile.transform.position.z),
                           Quaternion.identity,
                           transform);

				tile.SetTileType(TileType.Invalid);
				i++;
			}
		}
	}
}