﻿using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	private GameObject[] tilePrefabs;
	public Vector3 worldStart;

	public float TileSize{
		get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * 3;}
	}

	void Start () {
		CreateLevel();
	}

	void CreateLevel(){
		string[] mapData = ReadLevelText();

		int mapX = mapData[0].ToCharArray().Length;
		int mapY = mapData.Length;

		worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

		for (int y = 0; y < mapY; y++){
			char[] newTiles = mapData[y].ToCharArray();

			for (int x = 0; x < mapX; x++){
				PlaceTile(newTiles[x].ToString(), x, y, worldStart);
			}
		}
	}

	private void PlaceTile(string tileType, int x, int y, Vector3 worldStart){
		int tileIndex = int.Parse(tileType);

		GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
		newTile.transform.position = new Vector3(worldStart.x + TileSize * x, worldStart.y - TileSize * y, 0);
	}

	public string[] ReadLevelText(){
		TextAsset bindData = Resources.Load("Level") as TextAsset;

		string data = bindData.text.Replace(Environment.NewLine, string.Empty);

		return data.Split('-');
	}
}
