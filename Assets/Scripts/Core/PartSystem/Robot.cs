using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : ScriptableObject
{
    private Part _head;
    public Part Head
    {
        get => _head;
        set
        {
            if (value.Type == Part.PartType.Head) _head = value;
        }
    }

    private Part _weapon;
    public Part Weapon
    {
        get => _weapon;
        set
        {
            if (value.Type == Part.PartType.Weapon) _weapon = value;
        }
    }

    private Part _body;
    public Part Body
    {
        get => _body;
        set
        {
            if (value.Type == Part.PartType.Body) _body = value;
        }
    }

    private Part _movementModule;
    public Part MovementModule
    {
        get => _movementModule;
        set
        {
            if (value.Type == Part.PartType.MovementModule) _movementModule = value;
        }
    }

    private Part _cpu;
    public Part Cpu
    {
        get => _cpu;
        set
        {
            if (value.Type == Part.PartType.Cpu) _cpu = value;
        }
    }


    public int GetTotalRarity() => _head.Rarity + _weapon.Rarity + _body.Rarity + _movementModule.Rarity + _cpu.Rarity;
    public float GetTotalValue() => _head.GetPartValue() + _weapon.GetPartValue() + _body.GetPartValue() + _movementModule.GetPartValue() + _cpu.GetPartValue();
}
