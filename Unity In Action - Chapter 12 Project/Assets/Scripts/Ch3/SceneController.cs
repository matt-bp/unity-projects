using UnityEngine;
using Random = UnityEngine.Random;

namespace Ch3
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        private GameObject enemy;

        private void Update()
        {
            if (enemy == null)
            {
                enemy = Instantiate(enemyPrefab) as GameObject;
                enemy.transform.position = new Vector3(0, 1, 0);
                float angle = Random.Range(0, 360);
                enemy.transform.Rotate(0, angle, 0);
            }
        }
    }
}