
using UnityEngine;

public class CigaretteRolling : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private float spinSpeed;
    private bool rolling;

    public void Initialize(Vector3 target, float moveSpeed)
    {
        targetPosition = target;
        speed = moveSpeed;
        spinSpeed = Random.Range(-450f, 450f);
        rolling = true;
    }

    void Update()
    {
        if (!rolling) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        transform.Rotate(
            0f, 0f,
            spinSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            rolling = false;
        }
    }
}
