using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colors
{
    RED,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    VIOLET
}

[CreateAssetMenu(fileName = "TileValue", menuName = "ScriptableObjects/TileValue", order = 1)]
public class TileValueScriptableObj : ScriptableObject
{
    public Colors value;
    public Color color;

}
