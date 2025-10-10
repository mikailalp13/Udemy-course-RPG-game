using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera main_camera;
    private float last_camera_position_x;
    private float camera_half_width;
    [SerializeField] private ParallaxLayer[] background_layers;

    private void Awake()
    {
        main_camera = Camera.main;
        camera_half_width = main_camera.orthographicSize * main_camera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        float current_camera_position_x = main_camera.transform.position.x;
        float distance_to_move = current_camera_position_x - last_camera_position_x;
        last_camera_position_x = current_camera_position_x;

        float camera_left_edge = current_camera_position_x - camera_half_width;
        float camera_right_edge = current_camera_position_x + camera_half_width;

        foreach (ParallaxLayer layer in background_layers)
        {
            layer.Move(distance_to_move);
            layer.LoopBackground(camera_left_edge, camera_right_edge);
        }
    }

    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in background_layers)
            layer.CalculateImageWidth();
    }
}
