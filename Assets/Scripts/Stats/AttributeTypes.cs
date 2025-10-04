using UnityEngine;
using System;

[Flags]
public enum AttributeTypes
{
    None            = 0,
    FireAttack      = 1 << 0,
    FireResistence  = 1 << 1,
    WaterAttack     = 1 << 2,
    WaterResistence = 1 << 3,
}
