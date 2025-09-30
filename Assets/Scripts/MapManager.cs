using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject map;
    public void openOrClose()
    {
        map.SetActive(!map.active);
    }

}
