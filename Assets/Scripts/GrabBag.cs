using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrabBag<T> where T : class
{
    private readonly List<int> _historyIndexes = new List<int>();
    private readonly T[] _things;

    public GrabBag(T[] things)
    {
        _things = things;
    }

    private int _lastIndex = -1;

    public T Grab()
    {
        int thingsCount = _things.Length;
        if (thingsCount == 0)
            return null;

        if (_historyIndexes.Count == thingsCount)
        {
            _lastIndex = _historyIndexes.Last();
            _historyIndexes.Clear();
        }

        int[] unvisitedIndexes = GetUnvisitedIndexes(thingsCount);
        
        if (_lastIndex != -1 && _historyIndexes.Count == 0)
            unvisitedIndexes = unvisitedIndexes.Where(index => index != _lastIndex).ToArray();

        int newThingIndex = unvisitedIndexes[Random.Range(0, unvisitedIndexes.Length)];
        _historyIndexes.Add(newThingIndex);

        return _things[newThingIndex];
    }

    private int[] GetUnvisitedIndexes(int thingsCount)
    {
        return Enumerable.Range(0, thingsCount)
            .Except(_historyIndexes)
            .ToArray();
    }
}