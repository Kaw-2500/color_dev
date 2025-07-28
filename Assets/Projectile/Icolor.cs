using UnityEngine;

public enum ThisColor
{
    Red,
    Blue,
    Green
}

public interface Icolor
{
    ThisColor ReturnThisColor();
}
