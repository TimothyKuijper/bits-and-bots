using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Events;
using Yakapedia;

public struct Icon
{
    public GridData.IconTypes type;
    public Vector2 pos;
    public GameObject gameObject;
}

public class IconSwitcher : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    
    private Vector3 initPos;

    private Directions direction;

    private Icon selectedIcon;
    private Icon neighbouringIcon;
    
    public UnityEvent<int> onMatchMade = new();
    
    private enum Directions
    {
        Left,
        Right,
        Up,
        Down
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) initPos = Input.mousePosition;
        
        if (Input.GetMouseButtonUp(0))
        {
            CheckSwap();
        }
    }
    
    private void CheckSwap()
    {
        var delta = Input.mousePosition - initPos;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                direction = Directions.Right;
            }
            else
            {
                direction = Directions.Left;
            }
        }
        else
        {
            if (delta.y > 0)
            {
                direction = Directions.Up;
            }
            else
            {
                direction = Directions.Down;
            }
        }
        
        CheckTile();
    }

    private void CheckTile()
    {
        selectedIcon = new();
        neighbouringIcon = new();
        var initposWorld = Camera.main.ScreenToWorldPoint(initPos);
        var gridx = Mathf.RoundToInt((initposWorld.x / gridData.tileSize) + (gridData.width - 1) / 2);
        var gridy = Mathf.RoundToInt((initposWorld.y / gridData.tileSize) + (gridData.height - 1));
        selectedIcon.type = gridData.grid[gridx, gridy].type;
        selectedIcon.pos = new Vector2(gridx, gridy);

        
        
        switch (direction)
        {
            case Directions.Right:
                neighbouringIcon.type = gridData.grid[gridx + 1, gridy].type;
                neighbouringIcon.pos = new Vector2(gridx + 1, gridy);
                if (!CheckBounds()) return;
                break;
            
            case Directions.Left:
                neighbouringIcon.type = gridData.grid[gridx - 1, gridy].type;
                neighbouringIcon.pos = new Vector2(gridx - 1, gridy);
                if (!CheckBounds()) return;
                break;
            
            case Directions.Up:
                neighbouringIcon.type = gridData.grid[gridx, gridy + 1].type;
                neighbouringIcon.pos = new Vector2(gridx, gridy + 1);
                if (!CheckBounds()) return;
                break;
            
            case Directions.Down:
                neighbouringIcon.type = gridData.grid[gridx, gridy - 1].type;
                neighbouringIcon.pos = new Vector2(gridx, gridy - 1);
                if (!CheckBounds()) return;
                break;
        }

        var initialIcon = gridData.grid[gridx, gridy];
        var neighbourIcon = gridData.grid[(int) neighbouringIcon.pos.x, (int) neighbouringIcon.pos.y];

        initialIcon.pos = neighbouringIcon.pos;
        neighbourIcon.pos = gridData.grid[gridx, gridy].pos;

        gridData.grid[gridx, gridy] = neighbourIcon;
        gridData.grid[(int)neighbouringIcon.pos.x, (int)neighbouringIcon.pos.y] = initialIcon;

        var inititalIconPosition = initialIcon.gameObject.transform.position;
        var neighbourIconPosition = neighbourIcon.gameObject.transform.position;

        // initialIcon.gameObject.transform.position = neighbourIconPosition;
        // neighbourIcon.gameObject.transform.position = inititalIconPosition;
        
        var validMatch = CheckForMatch((int)neighbouringIcon.pos.x, (int)neighbouringIcon.pos.y);

        if (validMatch)
        {
            initialIcon.gameObject.transform.position = neighbourIconPosition;
            neighbourIcon.gameObject.transform.position = inititalIconPosition;
        }
    }

    private bool CheckBounds()
    {
        return neighbouringIcon.pos.x >= 0 && neighbouringIcon.pos.y >= 0 &&
               neighbouringIcon.pos.x < gridData.grid.GetLength(1) &&
               neighbouringIcon.pos.y < gridData.grid.GetLength(0);
    }

    private bool CheckForMatch(int x, int y)
    {
        var targetType = gridData.grid[x, y].type;
        var match = new List<Icon>();
        var visited = new List<Icon>();
        var toCheck = new Queue<Icon>();
        toCheck.Enqueue(gridData.grid[x, y]);
        match.Add(gridData.grid[x, y]);

        while (toCheck.Count > 0)
        {
            var temp = toCheck.Dequeue();
            var newX = (int)temp.pos.x;
            var newY = (int)temp.pos.y;
            var neighbours = CheckNeighbours(newX, newY);
            visited.Add(temp);
            

            foreach (var neighbour in neighbours)
            {
                if (neighbour.type == targetType)
                {
                    if (visited.Contains(neighbour)) continue;
                    match.Add(neighbour);
                    toCheck.Enqueue(neighbour);
                }
            }
        }

        if (match.Count < 3) return false;
        
        foreach (var icon in match)
        {
            onMatchMade.Invoke(match.Count);
            gridData.grid[(int)icon.pos.x, (int)icon.pos.y] = new Icon();
            Destroy(icon.gameObject);
        }
        
        return true;
    }

    private List<Icon> CheckNeighbours(int x, int y)
    {
        var neighbours = new List<Icon>();
        
        try
        {
            neighbours.Add(gridData.grid[x, y + 1]);
        }
        catch (IndexOutOfRangeException e){}
        
        try
        {
            neighbours.Add(gridData.grid[x, y - 1]);
        }
        catch (IndexOutOfRangeException e){}
        
        try
        {
            neighbours.Add(gridData.grid[x - 1, y]);
        }
        catch (IndexOutOfRangeException e){}
        
        try
        {
            neighbours.Add(gridData.grid[x + 1, y]);
        }
        catch (IndexOutOfRangeException e){}

        return neighbours;

    }
}
