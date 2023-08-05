using System.Collections.Generic;
using MattMath._1D;
using UnityEngine;

namespace Prototypes._08_Implicit_MassSpring_1D.Scripts
{
    public class ClothVisualization : MonoBehaviour
    {
        [SerializeField] private GameObject massPrefab;
        private List<GameObject> createdPrefabs = new();

        private ImplicitMassSpring cloth;

        private void Start()
        {
            cloth = GetComponent<ImplicitMassSpring>();
            Debug.Assert(cloth is not null);

            CreatePrefabs();
        }

        private void LateUpdate()
        {
            if (cloth.Positions?.Count != createdPrefabs.Count)
                CreatePrefabs();
            
            UpdateMassVisualization();
        }

        private void CreatePrefabs()
        {
            if (cloth.Positions is null) return;

            createdPrefabs = new List<GameObject>();
            
            foreach (var pos in cloth.Positions)
            {
                var newPrefab = Instantiate(massPrefab);
                createdPrefabs.Add(newPrefab);

                newPrefab.transform.position = new Vector3(0, (float)pos, 0);
            }
        }

        private void UpdateMassVisualization()
        {
            Debug.Assert(cloth is not null);
            for (var i = 0; i < cloth.Positions.Count; i++)
            {
                var pos = cloth.Positions[i];
                createdPrefabs[i].transform.position = new Vector3(0, (float)pos, 0);
            }
        }
    }
}