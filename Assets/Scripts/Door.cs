using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject _opened1;
    [SerializeField] GameObject _opened2;
    [SerializeField] GameObject _closed1;
    [SerializeField] GameObject _closed2;

    [ContextMenu(nameof(Open))]
    public void Open()
    {
        _opened1.SetActive(true);
        _opened2.SetActive(true);
        _closed1.SetActive(false);
        _closed2.SetActive(false);
    }
    
    [ContextMenu(nameof(Close))]
    public void Close()
    {
        _opened1.SetActive(false);
        _opened2.SetActive(false);
        _closed1.SetActive(true);
        _closed2.SetActive(true);
    }
}
