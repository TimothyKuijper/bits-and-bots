using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public struct Icon
{
    public GridData.IconTypes type;
    public Vector2 pos;
    public GameObject GO;
}

public class GridData : MonoBehaviour
{
    public int height;
    public int width;

    public float tileSize;

    private float offsetx;
    private float offsety;

    [SerializeField] private Sprite[] icons;

    [SerializeField] private GameObject iconPrefab;

    private List<GameObject> gridObjects = new();
    
    public enum IconTypes
    {
        _,
        square,
        circle,
        triangle,
        hexagon,
        diamond
    }

    public Icon[,] grid;

    void Start()
    {
        offsetx = (width - 1) / 2f;
        offsety = height - 1;
        BuildGrid();
    }

    public void BuildGrid()
    {
        gridObjects = new();
        
        if (grid == null)
        {
            grid = new Icon[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var randomType = (IconTypes)Random.Range(1, 6);
                    grid[x, y] = new Icon();
                    grid[x, y].type = randomType;
                    grid[x, y].pos = new Vector2(x, y);
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var worldX = (x - offsetx) * tileSize;
                var worldY = (y - offsety) * tileSize;
                
                if (grid[x, y].GO != null) continue;
                
                var randomType = (IconTypes)Random.Range(1, 6);
                grid[x, y] = new Icon();
                grid[x, y].type = randomType;
                grid[x, y].pos = new Vector2(x, y);

                var pos = new Vector2(worldX, worldY);
                var icon = Instantiate(iconPrefab, pos, Quaternion.identity);
                grid[x, y].GO = icon;

                var index = (int)grid[x,y].type;
                icon.GetComponent<SpriteRenderer>().sprite = icons[index -1];
                gridObjects.Add(icon);
            }
        }
    }
}
