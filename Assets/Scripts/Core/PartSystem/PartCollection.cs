using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robot PartCollection", menuName = "Bits & Bots/Parts/New PartCollection")]
public class PartCollection : ScriptableObject
{
    [SerializeField] public List<Part> parts = new List<Part>();

    // Ordering each type in a different list
    [SerializeField][HideInInspector] private List<Part> _headParts;
    [SerializeField][HideInInspector] private List<Part> _weaponParts;
    [SerializeField][HideInInspector] private List<Part> _bodyParts;
    [SerializeField][HideInInspector] private List<Part> _movementParts;
    [SerializeField][HideInInspector] private List<Part> _cpuParts;

    private void OnValidate()
    {
        ReorderTypes();
    }

    private void ReorderTypes()
    {
        _headParts = new List<Part>();
        _weaponParts = new List<Part>();
        _bodyParts = new List<Part>();
        _movementParts = new List<Part>();
        _cpuParts = new List<Part>();

        foreach (Part part in parts)
        {
            switch (part.Type)
            {
                case Part.PartType.Head: _headParts.Add(part); break; 
                case Part.PartType.Weapon: _weaponParts.Add(part); break; 
                case Part.PartType.Body: _bodyParts.Add(part); break; 
                case Part.PartType.MovementModule: _movementParts.Add(part); break; 
                case Part.PartType.Cpu: _cpuParts.Add(part); break; 
            }
        }
    }

    public List<Part> GetPartListByType(Part.PartType type)
    {
        switch (type)
        {
            case Part.PartType.Head: return _headParts;
            case Part.PartType.Weapon: return _weaponParts;
            case Part.PartType.Body: return _bodyParts;
            case Part.PartType.MovementModule: return _movementParts;
            case Part.PartType.Cpu: return _cpuParts;
        }
        return null;
    }

    // These functions automatically duplicate the parts to make changing their stats not reflect on their original objects
    // Regular functions
    public Part GetPart(int index, List<Part> list = null)
    {
        if (list == null) list = parts;

        var part = parts[Mathf.Clamp(index, 0, parts.Count)];
        return Instantiate(part);
    }

    public Part GetRandomPart()
    {
        var part = GetPart(Random.Range(0, parts.Count));
        return part;
    }

    // Using type
    public Part GetPartByType(int index, Part.PartType type)
    {
        var list = GetPartListByType(type);
        var part = GetPart(index, list);
        if (part == null)
        {
            Debug.LogError("No Parts of type " + type.ToString() + " found in Parts list on object " + name + "!");
            return null;
        }
        return part;
    }

    public Part GetRandomPartByType(Part.PartType type)
    {
        var list = GetPartListByType(type);
        var part = GetPart(Random.Range(0, list.Count), list);
        if (part == null)
        {
            Debug.LogError("No Parts of type " + type.ToString() + " found in Parts list on object " + name + "!");
            return null;
        }
        return part;
    }
}
