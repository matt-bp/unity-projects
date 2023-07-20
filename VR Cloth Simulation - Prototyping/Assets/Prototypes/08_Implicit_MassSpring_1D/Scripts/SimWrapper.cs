using System;
using System.Collections;
using System.Collections.Generic;
using MattMath;
using UnityEngine;

public class SimWrapper : MonoBehaviour
{
    [SerializeField] private GameObject massPrefab;
    
    private ImplicitMassSpring1D system = new();

    private List<GameObject> CreatedPrefabs = new();
    
    private void Start()
    {
        foreach (var pos in system.positions)
        {
            var newPrefab = Instantiate(massPrefab);
            CreatedPrefabs.Add(newPrefab);

            newPrefab.transform.position = new Vector3(0, (float)pos, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //system.Update(Time.deltaTime);

        for (var i = 0; i < system.positions.Count; i++)
        {
            CreatedPrefabs[i].transform.position = new Vector3(0, (float)system.positions[i], 0);
        }
    }
}
