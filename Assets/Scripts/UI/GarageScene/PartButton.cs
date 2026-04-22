using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartButton : WorldButton
{
    [SerializeField] private PartsScreen partMenu;
    [SerializeField] private Part.PartType partType;

    private void Start()
    {
        // ADD PHYSICS FORCE LATER
        onPressed.AddListener(() => partMenu.OpenPart(partType));
    }
}
