using UnityEditor;
using UnityEngine;

namespace Projects.Line_Rendering.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    // [ExecuteInEditMode]
    public class LineGenerator : MonoBehaviour
    {
        public int numPoints;
        
        private void Awake()
        {
            Generate();
        }

        public void Generate()
        {
            Debug.Log($"GOInG");
            var renderer = GetComponent<LineRenderer>();
            renderer.positionCount = numPoints;

            var positions = new Vector3[renderer.positionCount];

            var anglePerThing = 360 / (positions.Length - 1);
            var vectorToRotate = Vector3.left;

            for (int i = 0, currentAngle = 0; i < positions.Length; i++, currentAngle += anglePerThing)
            {
                positions[i] = Quaternion.AngleAxis(currentAngle, Vector3.forward) * vectorToRotate;
            }

            // positions[positions.Length - 1] = vectorToRotate;

            renderer.SetPositions(positions);
        }
    }

    [CustomEditor(typeof(LineGenerator)), CanEditMultipleObjects]
    public class LineGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var generator = target as LineGenerator;
            
            GUILayout.Label ("This is a Label in a Custom Editor");
            if (GUILayout.Button("Generate"))
            {
                Debug.Log("Generate!");
                generator.Generate();
            }

            generator.numPoints = EditorGUILayout.IntField("Number of Points", generator.numPoints);
        }
    }
}
