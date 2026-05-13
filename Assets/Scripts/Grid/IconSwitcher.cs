using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yakanashe.Yautl;

public class IconSwitcher : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    
    private Vector3 initPos;

    private DirectionTypes direction;

    private Icon selectedIcon;
    private Icon neighbouringIcon;

    private List<Icon> match;
    
    public static UnityEvent<int> onMatchMade = new();

    public static bool IsSwapping;
    
    //enum to define the directions swiped in
    private enum DirectionTypes
    {
        Left,
        Right,
        Up,
        Down
    }

    void Update()
    {
        if (IsSwapping) return;
        if (Input.GetMouseButtonDown(0)) initPos = Input.mousePosition;
        //check for touch
        if (Input.GetMouseButtonUp(0))
        {
            CheckSwapDirection();
        }
    }
    
    //check to see in which direction 
    private void CheckSwapDirection()
    {
        
        var delta = Input.mousePosition - initPos;
        if (Mathf.Abs(delta.x)  < 15 && Mathf.Abs(delta.y) < 15) return; 
        
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                direction = DirectionTypes.Right;
            }
            else
            {
                direction = DirectionTypes.Left;
            }
        }
        else
        {
            if (delta.y > 0)
            {
                direction = DirectionTypes.Up;
            }
            else
            {
                direction = DirectionTypes.Down;
            }
        }
        
        PerformSwap();
    }

    //function that swaps the desired Icons and initiates a tween
    private void PerformSwap()
    {
        IsSwapping = true;
        selectedIcon = new();
        neighbouringIcon = new();
        
        
        var initposWorld = Camera.main.ScreenToWorldPoint(initPos);
        var gridx = Mathf.RoundToInt((initposWorld.x / gridData.tileSize) + (gridData.width - 1) / 2);
        var gridy = Mathf.RoundToInt((initposWorld.y / gridData.tileSize) + (gridData.height - 1));
        selectedIcon.data = gridData.grid[gridx, gridy].data;
        selectedIcon.pos = new Vector2(gridx, gridy);
        
        //selects the Icon in the direction of the swipe
        switch (direction)
        {
            case DirectionTypes.Right:
                if (!CheckBounds(new Vector2(gridx + 1, gridy)))
                {
                    IsSwapping = false;
                    return;
                }
                neighbouringIcon.data = gridData.grid[gridx + 1, gridy].data;
                neighbouringIcon.pos = new Vector2(gridx + 1, gridy);
                break;
            
            case DirectionTypes.Left:
                if (!CheckBounds(new Vector2(gridx - 1, gridy)))
                {
                    IsSwapping = false;
                    return;
                }                
                neighbouringIcon.data = gridData.grid[gridx - 1, gridy].data;
                neighbouringIcon.pos = new Vector2(gridx - 1, gridy);
                break;
            
            case DirectionTypes.Up:
                if (!CheckBounds(new Vector2(gridx, gridy + 1)))
                {
                    IsSwapping = false;
                    return;
                }                
                neighbouringIcon.data = gridData.grid[gridx, gridy + 1].data;
                neighbouringIcon.pos = new Vector2(gridx, gridy + 1);
                break;
            
            case DirectionTypes.Down:
                if (!CheckBounds(new Vector2(gridx, gridy - 1)))
                {
                    IsSwapping = false;
                    return;
                }                
                neighbouringIcon.data = gridData.grid[gridx, gridy - 1].data;
                neighbouringIcon.pos = new Vector2(gridx, gridy - 1);
                break;
        }

        var initialIcon = gridData.grid[gridx, gridy];
        var neighbourIcon = gridData.grid[(int) neighbouringIcon.pos.x, (int) neighbouringIcon.pos.y];

        initialIcon.pos = neighbouringIcon.pos;
        neighbourIcon.pos = gridData.grid[gridx, gridy].pos;

        gridData.grid[gridx, gridy] = neighbourIcon;
        gridData.grid[(int)neighbouringIcon.pos.x, (int)neighbouringIcon.pos.y] = initialIcon;

        var inititalIconPosition = initialIcon.GO.transform.position;
        var neighbourIconPosition = neighbourIcon.GO.transform.position;
        
        //checks if the match is valid
        var validMatch = CheckForMatch((int)neighbouringIcon.pos.x, (int)neighbouringIcon.pos.y, out var amount);
        
        //tweens - and optionally destroys - the Icons that get swapped
        if (validMatch)
        {
            if (neighbourIcon.GO == null) return;
            TweenRunner.Instance.KillAllFrom(neighbourIcon.GO.transform);
            TweenRunner.Instance.KillAllFrom(initialIcon.GO.transform);
            
            neighbourIcon.GO.transform.MoveTo(initialIcon.GO.transform.position, .2f, EaseType.InOutCubic);
            initialIcon.GO.transform.MoveTo(neighbourIcon.GO.transform.position, .2f, EaseType.InOutCubic).OnComplete(() =>
            {
                foreach (var icon in match)
                {
                    gridData.grid[(int)icon.pos.x, (int)icon.pos.y] = new Icon();
                    Destroy(icon.GO);
                }
                onMatchMade.Invoke(amount);
                IsSwapping = false;
            });
            initialIcon.GO.transform.position = neighbourIconPosition;
            neighbourIcon.GO.transform.position = inititalIconPosition;
            
        }
        else
        {
            if (initialIcon.GO == null) return;
            initialIcon.GO.transform.MoveTo(neighbourIcon.GO.transform.position, .2f, EaseType.InOutCubic).OnComplete(() =>
            {
                initialIcon.GO.transform.MoveTo(neighbourIcon.GO.transform.position, .2f, EaseType.InOutCubic);
            });
                
            
            neighbourIcon.GO.transform.MoveTo(initialIcon.GO.transform.position, .2f, EaseType.InOutCubic).OnComplete(() =>
            {
                neighbourIcon.GO.transform.MoveTo(initialIcon.GO.transform.position, .2f, EaseType.InOutCubic);
                IsSwapping = false;
            });
        }
    }

    //checks if a requested locations is out of bounds
    private bool CheckBounds(Vector2 request)
    {
        return request.x >= 0 && request.y >= 0 &&
               request.x < gridData.grid.GetLength(0) &&
               request.y < gridData.grid.GetLength(1);
    }

    //checks if a match is valid by checking if there are 3 of more
    //neighbours that are the same Icon as the initial Icon
    //then returning a bool, and the amount of Icons matched
    private bool CheckForMatch(int x, int y, out int matchAmount)
    {
        var targetData = gridData.grid[x, y].data;
        var visited = new List<Icon>();
        var toCheck = new Queue<Icon>();
        match = new List<Icon>();
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
                if (neighbour.data == targetData)
                {
                    if (visited.Contains(neighbour)) continue;
                    match.Add(neighbour);
                    toCheck.Enqueue(neighbour);
                }
            }
        }

        matchAmount = 0;
        if (match.Count < 3) return false;
        
        matchAmount = match.Count;
        return true;
    }

    
    //gets all the neighbours involved in the swap
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