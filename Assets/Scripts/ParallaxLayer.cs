using UnityEngine;
[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallax_multiplier;
    [SerializeField] private float image_width_offset = 10;

    private float image_full_width;
    private float image_half_width;

    public void CalculateImageWidth()
    {
        image_full_width = background.GetComponent<SpriteRenderer>().bounds.size.x;
        image_half_width = image_full_width / 2;
    }
    public void Move(float distance_to_move)
    {
        background.position += Vector3.right * (distance_to_move * parallax_multiplier); // new Vector3(distance_to_move * parallax_multiplier, 0, 0); 
    }

    public void LoopBackground(float camera_left_edge, float camera_right_edge)
    {
        float image_right_edge = (background.position.x + image_half_width) - image_width_offset;
        float image_left_edge = (background.position.x - image_half_width) + image_width_offset;

        if (image_right_edge < camera_left_edge)
            background.position += Vector3.right * image_full_width;
        else if (image_left_edge > camera_right_edge)
            background.position += Vector3.right * -image_full_width;
        
    }
}   
