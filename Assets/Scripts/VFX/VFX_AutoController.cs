using System.Runtime.Serialization;
using Microsoft.VisualBasic;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool auto_destroy = true;
    [SerializeField] private float destroy_delay = 1;
    [Space]
    [SerializeField] private bool random_offset = true;
    [SerializeField] private bool random_rotation = true;


    [Header("Random rotation")]
    [SerializeField] private float min_rotation = 0;
    [SerializeField] private float max_rotation = 360;


    [Header("Random Position")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [Space]
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;


    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (auto_destroy)
            Destroy(gameObject, destroy_delay);
    }

    private void ApplyRandomOffset()
    {
        if (random_offset == false)
            return;

        float x_offset = Random.Range(xMinOffset, xMaxOffset);
        float y_offset = Random.Range(yMinOffset, yMaxOffset);

        transform.position += new Vector3(x_offset, y_offset);
    }


    private void ApplyRandomRotation()
    {
        if (random_rotation == false)
            return;

        float z_rotation = Random.Range(min_rotation, max_rotation);
        transform.Rotate(0, 0, z_rotation);
    }
}
