using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformGrabBag
{
    private readonly List<int> _historyIndexes = new List<int>();
    private readonly Transform[] _transforms;

    public TransformGrabBag(Transform[] transforms)
    {
        _transforms = transforms;
    }

    private int _lastIndex = -1;

    public Transform Grab()
    {
        int itemsCount = _transforms.Length;
        if (itemsCount == 0)
            return null;

        if (_historyIndexes.Count == itemsCount)
        {
            _lastIndex = _historyIndexes.Last();
            _historyIndexes.Clear();
        }

        int[] unvisitedIndexes = GetUnvisitedIndexes(itemsCount);
        
        if (_lastIndex != -1 && _historyIndexes.Count == 0)
            unvisitedIndexes = unvisitedIndexes.Where(index => index != _lastIndex).ToArray();

        int newItemIndex = unvisitedIndexes[Random.Range(0, unvisitedIndexes.Length)];
        _historyIndexes.Add(newItemIndex);

        return _transforms[newItemIndex];
    }

    private int[] GetUnvisitedIndexes(int itemsCount)
    {
        return Enumerable.Range(0, itemsCount)
            .Except(_historyIndexes)
            .ToArray();
    }
}