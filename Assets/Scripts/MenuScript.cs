using UnityEditor;
using UnityEngine;

public class MenuScript
{
	[MenuItem("Tools/Assign Tile Material")]
	public static void AssignTileMaterial()
	{
		var tiles = GameObject.FindGameObjectsWithTag("Tile");
		var material = Resources.Load<Material>("Tile");

		foreach (var t in tiles)
			t.GetComponent<Renderer>().material = material;
	}

	[MenuItem("Tools/Assign Tile Script")]
	public static void AssignTileScript()
	{
		var tiles = GameObject.FindGameObjectsWithTag("Tile");

		foreach (var t in tiles)
			t.AddComponent<Tile>();
	}
}