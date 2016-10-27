using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	private GameObject[] tilePrefabs;
	public Vector3 worldStart;

	//sets up delimiter characters
	char[] delimiterChars = { ' ', ',', '(', ')'};

	public float TileSize{
		get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * 3;}
	}

	void Start () {
		CreateLevel();
	}

	//creates level based off of strings read from txt file, breaks it down using delimiters
	void CreateLevel(){
		string[] mapData = ReadLevelText();
		worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

		//parsing each delimited element
		for (int i = 0; i < mapData.Length; i ++){
			string[] data = mapData[i].Split(delimiterChars);

//			string parentText = data[0];
			int mapX = int.Parse(data[1]);

			int mapY = int.Parse(data[2]);
			int tileType = int.Parse(data[3]);

			PlaceTile(tileType, mapX, mapY, worldStart);
		}
	}

	private void PlaceTile(int tileType, int x, int y, Vector3 worldStart){
		GameObject newTile = Instantiate(tilePrefabs[tileType]);
		newTile.transform.position = new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0);
		newTile.transform.parent = gameObject.transform;
	}

	//reads the txt file and turns it to a string
	public string[] ReadLevelText(){
		TextAsset bindData = Resources.Load("Level 2") as TextAsset;

		string data = bindData.text;

		//break down the string by spaces
		return data.Split(' ');
	}
}
