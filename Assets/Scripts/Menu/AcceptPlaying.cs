using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptPlaying : MonoBehaviour
{
    [SerializeField] int _PlayCount = 0;
    [SerializeField] public int _MaxPlayCount = 2;
    [SerializeField] GameObject cacheplay;

    void Update()
    {
        if( _PlayCount == _MaxPlayCount)
        {
            cacheplay.SetActive(false);
        }
        if (_PlayCount != _MaxPlayCount)
        {
            cacheplay.SetActive(true);
        }
    }

    public void AcceptPlayeur()
    {
        _PlayCount++;
    }
    public void RemoveAcceptPlayeur()
    {
        _PlayCount--;
    }
    public void MorePlayeur()
    {
        _MaxPlayCount++;
    }
    public void UnmorePlayeur()
    {
        _MaxPlayCount--;
    }
}
