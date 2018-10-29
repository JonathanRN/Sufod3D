using UnityEngine;

public class RockMaker : MonoBehaviour
{
	[SerializeField] private GameObject rockPrefab;
	[SerializeField] private int amount;

	private GameObject[] tiles;

	private void Start()
	{
		tiles = GameObject.FindWithTag("Grid").GetComponent<Grid>().Tiles;
		Debug.Log(tiles);
		
		RandomlyGenerateRocks();
	}

	private void RandomlyGenerateRocks()
	{
		var i = 0;
		while (i < amount)
		{
			var tile = tiles[Random.Range(0, tiles.Length)];
			
			RaycastHit hit;

			if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
			{
				var instance = Instantiate(rockPrefab,
										   new Vector3(tile.transform.position.x, 0.75f, tile.transform.position.z),
										   Quaternion.identity,
										   transform);

				i++;
			}
		}
	}
}