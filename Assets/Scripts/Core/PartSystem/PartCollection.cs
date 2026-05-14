using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Robot PartCollection", menuName = "Bits & Bots/Parts/New PartCollection")]
public class PartCollection : ScriptableObject
{
    [SerializeField] public List<Part> Parts = new List<Part>();

    // Ordering each type in a different list
    [SerializeField][HideInInspector] private List<Part> headParts;
    [SerializeField][HideInInspector] private List<Part> weaponParts;
    [SerializeField][HideInInspector] private List<Part> bodyParts;
    [SerializeField][HideInInspector] private List<Part> movementParts;
    [SerializeField][HideInInspector] private List<Part> cpuParts;

    private void OnValidate()
    {
        ReorderTypes();
    }

    private void ReorderTypes()
    {
        headParts = new List<Part>();
        weaponParts = new List<Part>();
        bodyParts = new List<Part>();
        movementParts = new List<Part>();
        cpuParts = new List<Part>();

        foreach (Part part in Parts)
        {
            switch (part.Type)
            {
                case Part.PartType.Head: headParts.Add(part); break; 
                case Part.PartType.Weapon: weaponParts.Add(part); break; 
                case Part.PartType.Body: bodyParts.Add(part); break; 
                case Part.PartType.MovementModule: movementParts.Add(part); break; 
                case Part.PartType.Cpu: cpuParts.Add(part); break; 
            }
        }
    }

    public List<Part> GetPartListByType(Part.PartType type)
    {
        switch (type)
        {
            case Part.PartType.Head: return headParts;
            case Part.PartType.Weapon: return weaponParts;
            case Part.PartType.Body: return bodyParts;
            case Part.PartType.MovementModule: return movementParts;
            case Part.PartType.Cpu: return cpuParts;
        }
        return null;
    }

    // These functions automatically duplicate the parts to make changing their stats not reflect on their original objects
    // Regular functions
    public Part GetPart(int index, List<Part> list = null)
    {
        if (list == null) list = Parts;

        var part = list[Mathf.Clamp(index, 0, list.Count)];
        return Instantiate(part);
    }

    public Part GetRandomPart()
    {
        var part = GetPart(Random.Range(0, Parts.Count));
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
