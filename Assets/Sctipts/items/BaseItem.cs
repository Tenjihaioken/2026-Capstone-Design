using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    public string itemName;

    public abstract bool Use(GameObject user);
}