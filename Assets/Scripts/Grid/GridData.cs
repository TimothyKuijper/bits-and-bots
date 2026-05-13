using System.Collections.Generic;
using UnityEngine;
using Yakanashe.Yautl;

// A stuct to define Icon data and easily move it around
public struct Icon
{
    public IconData data;
    public Vector2 pos;
    public GameObject GO;
}

[System.Serializable]
public class IconData
{
    public string id;
    public GameObject prefab;
}

public class GridData : MonoBehaviour
{
    public int height;
    public int width;

    public float tileSize;

    private float offsetx;
    private float offsety;

    [SerializeField] private IconSwitcher iconSwitcher;

    [SerializeField] private List<IconData> icons;

    [SerializeField] private GameObject gridObject;

    private List<GameObject> gridObjects = new();

    public Icon[,] grid;

    void Start()
    {
        offsetx = (width - 1) / 2f;
        offsety = height - 1;
        BuildGrid();
    }

    
    //function to initialize and update the grid
    public void BuildGrid()
    {
        IconSwitcher.IsSwapping = true;
        gridObjects = new();
        
        if (grid == null)
        {
            grid = new Icon[width, height];
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                
                var worldX = (x - offsetx) * tileSize;
                var worldY = (y - offsety) * tileSize;
                
                if (grid[x, y].GO != null) continue;
                
                var randomData = icons[Random.Range(0, icons.Count)];
                grid[x, y] = new Icon();
                grid[x, y].data = randomData;
                grid[x, y].pos = new Vector2(x, y);

                var pos = new Vector2(worldX, worldY);
                
                var icon = Instantiate(randomData.prefab, pos, randomData.prefab.transform.rotation);
                icon.transform.parent = gridObject.transform;
                
                grid[x, y].GO = icon;
                var gridTransform = grid[x, y].GO.transform;
                
                gridObjects.Add(icon);
                var a = gridTransform.position;
                var b = a;
                b.y = a.y + 2 + height;
                gridTransform.position = b;
                gridTransform.localScale = Vector3.zero;

                iconSwitcher.CheckForMatch(x, y, out var amount);
                IconSwitcher.onMatchMade.Invoke(amount);

                gridTransform.ScaleTo(Vector3.one, 1.1f, EaseType.InOutCubic);
                gridTransform.MoveTo(a, 1, EaseType.InCubic).OnComplete(() =>
                {
                    IconSwitcher.IsSwapping = false;
                });
            }
        }
    }
}
