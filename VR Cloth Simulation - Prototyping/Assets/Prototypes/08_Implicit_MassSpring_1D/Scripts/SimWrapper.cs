using System;
using System.Collections.Generic;
using Helpers;
using MattMath;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototypes._08_Implicit_MassSpring_1D.Scripts
{
    public class SimWrapper : MonoBehaviour
    {
        [SerializeField] private GameObject massPrefab;
    
        private readonly ImplicitMassSpring1D system = new();
        
        private List<RunStatistics1D> runStatistics = new();
        private List<GameObject> createdPrefabs = new();
        /// <summary>
        /// Time, in seconds, since the simulation started.
        /// </summary>
        private double Elapsed;

        [SerializeField] private InputAction downloadReport;
        
        public bool isEnabled = false;
    
        private void Start()
        {
            foreach (var pos in system.positions)
            {
                var newPrefab = Instantiate(massPrefab);
                createdPrefabs.Add(newPrefab);

                newPrefab.transform.position = new Vector3(0, (float)pos, 0);
            }
            
            downloadReport.Enable();
        }
        
        // Update is called once per frame
        void Update()
        {
            if (downloadReport.WasPerformedThisFrame())
            {
                Debug.Log("Downloading CSV report.");
                // TODO: Actually create report here

                StatsWriter.WriteRunStatistics(runStatistics);
            }

            if (!isEnabled) return;

            system.Update(Time.deltaTime);

            for (var i = 0; i < system.positions.Count; i++)
            {
                createdPrefabs[i].transform.position = new Vector3(0, (float)system.positions[i], 0);
            }

            Elapsed += Time.deltaTime;

            runStatistics.Add(new RunStatistics1D()
            {
                Elapsed = Elapsed,
                Position = system.positions[0],
            });
        }
    }
}
