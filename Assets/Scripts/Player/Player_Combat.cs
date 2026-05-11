using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter attack details")]
    [SerializeField] private float counter_recovery = 0.1f;
    [SerializeField] private LayerMask what_is_counterable;



    public bool CounterAttackPerformed()
    {
        bool has_performed_counter = false;

        foreach (var target in GetDetectedColliders(what_is_counterable))
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable == null)
                continue; // skip this target, go to next target

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                has_performed_counter = true;
            }
        }

        return has_performed_counter;
    }


    public float GetCounterRecoveryDuration() => counter_recovery;
}
