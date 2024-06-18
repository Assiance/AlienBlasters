using UnityEngine;

public interface IItem
{
    public string name { get; }
    void Use();
    GameObject gameObject { get; }
    Transform transform { get; }
}