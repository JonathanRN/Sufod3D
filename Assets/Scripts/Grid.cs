using UnityEngine;

public class Grid : MonoBehaviour
{
	[SerializeField] private GameObject tilePrefab;
	[SerializeField] private int amountToGenerate;
	
	public GameObject[] Tiles { get; private set; }

	private void Awake()
	{
		GenerateGrid();
		Tiles = GameObject.FindGameObjectsWithTag("Tile");
	}

	private void GenerateGrid()
	{
		//First half
		for (int x = 0; x < amountToGenerate; x++)
		{
			Instantiate(tilePrefab, new Vector3(x, 0, 0), Quaternion.identity, transform);

			if (x >= 1)
			{
				for (int z = 1; z <= x; z++)
				{
					Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
					Instantiate(tilePrefab, new Vector3(x, 0, -z), Quaternion.identity, transform);
				}
			}
		}
		
		//Second half
		for (int x = amountToGenerate*2; x >= amountToGenerate; x--)
		{
			Instantiate(tilePrefab, new Vector3(x, 0, 0), Quaternion.identity, transform);

			if (x < amountToGenerate*2)
			{
				for (int z = 1; z <= (amountToGenerate*2 - x); z++)
				{
					Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
					Instantiate(tilePrefab, new Vector3(x, 0, -z), Quaternion.identity, transform);
				}
			}
		}
		
		Vector3 newPos = new Vector3(-amountToGenerate, 0, 0);
		transform.position = newPos;
	}
}