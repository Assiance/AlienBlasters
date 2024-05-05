using UnityEngine;

public interface IItem  
{
    void Use();
    GameObject gameObject { get; }
    Transform transform { get; }
}