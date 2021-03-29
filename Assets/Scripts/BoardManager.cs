using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    [SerializeField]
    public List<TileValueScriptableObj> characters = new List<TileValueScriptableObj>();
    public GameObject tile;
    public int xSize, ySize;
    public Vector2 offset;
    private List<List<GameObject>> tiles = new List<List<GameObject>>();
    private GameObject selectedTile;
    private int selectX = -1, selectY = -1;
    //private GameObject[,] tiles;

    public bool IsShifting { get; set; }

    void Start()
    {
        instance = this;
        CreateBoard(offset.x, offset.y);
    }

    private void FixedUpdate()
    {
        
        for (int x = 0; x < tiles.Count; x++)
        {
            for (int y = 0; y < tiles[x].Count; y++)
            {
                if (!tiles[x][y].GetComponent<Tile>().inPlace)
                {
                    IsShifting = true;
                    return;
                }
            }
        }
        IsShifting = false;
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        for (int x = 0; x < xSize; x++)
        {
            tiles.Add(new List<GameObject>());
        }
        //tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;

        TileValueScriptableObj[] previousLeft = new TileValueScriptableObj[ySize];
        TileValueScriptableObj previousBelow = null;
        for (int x = 0; x < xSize; x++)
        {      
            for (int y = 0; y < ySize; y++)
            {
                List<TileValueScriptableObj> possibleCharacters = new List<TileValueScriptableObj>();
                possibleCharacters.AddRange(characters);

                possibleCharacters.Remove(previousLeft[y]);
                possibleCharacters.Remove(previousBelow);

                GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * ySize), 0), tile.transform.rotation);
                //GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                newTile.transform.parent = transform;
                newTile.GetComponent<Tile>().position = new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0);
                newTile.GetComponent<Tile>().tilevalue = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                previousLeft[y] = newTile.GetComponent<Tile>().tilevalue;
                previousBelow = newTile.GetComponent<Tile>().tilevalue;
                tiles[x].Add(newTile);
                //tiles[x, y] = newTile;
            }
        }
    }

    void SetProperPosition()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        for (int x = 0; x < tiles.Count; x++)
        { 
            for (int y = 0; y < tiles[x].Count; y++)
            {
                Tile tile = tiles[x][y].GetComponent<Tile>();
                tile.position = new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0);
            }
        }
    }

    public void SelectTile(GameObject obj)
    {
        int xIndex=-1;
        int yIndex=-1;

        if (selectedTile == null)
        { // nothing selected yet
            selectedTile = obj;
            for (selectX = 0; selectX < tiles.Count; selectX++)
            {

                selectY = tiles[selectX].IndexOf(obj);
                if (selectY >= 0)
                    break;
            }
            Tile tile = obj.GetComponent<Tile>();
            tile.selected = true;
            return;
        }
        else if (obj == selectedTile)
        { //selected tiles deselected
            selectX = selectY = -1;
            Tile tile = selectedTile.GetComponent<Tile>();
            tile.selected = false;
            selectedTile = null;
            return;
        }
        else
        { // already have selected and another tile clicked
            for (xIndex = 0; xIndex < tiles.Count; xIndex++)
            {

                yIndex = tiles[xIndex].IndexOf(obj);
                if (yIndex >= 0)
                    break;
            }
            Debug.Log("select x: " + selectX + "   y:" + selectY);
            Debug.Log(" new   x: " + xIndex + "   y:" + yIndex);
            // check for adjacentcy if yes swap
            if (selectX == xIndex)
            {
                Debug.Log("x the same = " + Mathf.Abs(yIndex - selectY));
                if (Mathf.Abs(yIndex - selectY) == 1)
                {
                    SwapTile(selectedTile, selectX, selectY, obj, xIndex, yIndex);
                    Debug.Log("swap");
                    selectX = selectY = -1;
                    Tile tile2 = selectedTile.GetComponent<Tile>();
                    tile2.selected = false;
                    selectedTile = null;
                    return;
                }
            }
            else if (selectY == yIndex)
            {
                Debug.Log("y the same = "+ Mathf.Abs(xIndex - selectX));
                if (Mathf.Abs(xIndex - selectX) == 1)
                {
                    Debug.Log("swap");
                    SwapTile(selectedTile, selectX, selectY, obj, xIndex, yIndex); 
                    selectX = selectY = -1;
                    Tile tile2 = selectedTile.GetComponent<Tile>();
                    tile2.selected = false;
                    selectedTile = null;
                    return;
                }
            }
            // if not adjacent then deselected current tile and select new tile
            selectX = selectY = -1;
            Tile tile = selectedTile.GetComponent<Tile>();
            tile.selected = false;
            selectedTile = null;

            selectedTile = obj;
            tile = selectedTile.GetComponent<Tile>();
            selectX = xIndex;
            selectY = yIndex;
            tile.selected = true;
            Debug.Log("new select x: " + selectX + "   y:" + selectY);
        }
        
    }

    void SwapTile(GameObject tile1, int x1, int y1, GameObject tile2, int x2, int y2)
    {
        if (x1 == x2)
        { // same column
            if (y1 < y2)
            {
                tiles[x1].RemoveAt(y1);
                tiles[x1].Insert(y2, tile1);
            }
            else
            {
                tiles[x1].RemoveAt(y2);
                tiles[x1].Insert(y1, tile2);
            }
        }
        else if (y1 == y2)
        { // same row
            tiles[x1].RemoveAt(y1);
            tiles[x2].RemoveAt(y2);

            tiles[x2].Insert(y2, tile1);
            tiles[x1].Insert(y1, tile2);
        }

        SetProperPosition();
    }

}
