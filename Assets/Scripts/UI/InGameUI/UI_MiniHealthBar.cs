using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity; 

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }


    private void OnEnable()
    {
        entity.OnFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        entity.OnFlipped -= HandleFlip;
    }

    private void HandleFlip() => transform.rotation = Quaternion.identity; // keeps rotation at default so that when the enemy turns the other side the health bar stays still
}
