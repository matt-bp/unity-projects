using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayShooter : MonoBehaviour
{
    private Camera _camera;

    // Start is called before the first frame update
    private void Start()
    {
        _camera = GetComponent<Camera>();

        // Cursor.lockState = CursorLockMode.Locked; // Cursor stays in the center of the screen
        // Cursor.visible = false;
    }

    // Runs every frame after the 3D scene is rendered
    private void OnGUI()
    {
        const int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    // Update is called once per frame
    private void Update()
    {
        // Left (first) mouse button
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            var ray = _camera.ScreenPointToRay(point);

            if (Physics.Raycast(ray, out var hit))
            {
                var hitObject = hit.transform.GameObject();
                var target = hitObject.GetComponent<ReactiveTarget>();

                if (target != null)
                {
                    target.ReactToHit();
                }
                else
                {
                    StartCoroutine(SphereIndicator(hit.point));
                }
            }
        }
    }

    private IEnumerator<YieldInstruction> SphereIndicator(Vector3 position)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;

        yield return new WaitForSeconds(1);
        
        Destroy(sphere);
    }
}