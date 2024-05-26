using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEncounter : MonoBehaviour
{
    [SerializeField] List<Transform> _lightnings;
    [SerializeField] float _delayBetweenLightning = 1f;

    void OnEnable()
    {
        StartCoroutine(StartEncounter());
    }
    
    IEnumerator StartEncounter()
    {
        while (true)
        {
            foreach (var lightning in _lightnings)
            {
                lightning.gameObject.SetActive(false);
            }

            yield return null;

            int index = Random.Range(0, _lightnings.Count);
            _lightnings[index].gameObject.SetActive(true);

            yield return new WaitForSeconds(_delayBetweenLightning);
        }
    }
}
