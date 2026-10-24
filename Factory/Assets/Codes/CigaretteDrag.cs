
using UnityEngine;

public class CigaretteDrag : MonoBehaviour
{
    private Camera mainCamera;
    private CigaretteRolling rolling;
    private Collider2D cigaretteCollider;

    private bool dragging;
    private Vector3 previousPosition;

    void Awake()
    {
        mainCamera = Camera.main;
        rolling = GetComponent<CigaretteRolling>();
        cigaretteCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        dragging = true;
        previousPosition = transform.position;

        if (rolling != null)
            rolling.enabled = false;

        transform.rotation = Quaternion.identity;
    }

    void OnMouseDrag()
    {
        if (!dragging) return;

        Vector3 mouse = mainCamera.ScreenToWorldPoint(
            Input.mousePosition
        );

        mouse.z = 0f;
        transform.position = mouse;
    }

    void OnMouseUp()
    {
        if (!dragging) return;
        dragging = false;

        PackManager pack = PackManager.instance;

        if (pack != null && pack.CanPack())
        {
            Collider2D packCollider =
                pack.GetComponent<Collider2D>();

            if (packCollider != null &&
                packCollider.OverlapPoint(transform.position))
            {
                bool packed = pack.AddCigarette(gameObject);

                if (packed)
                {
                    CigaretteSpawner spawner =
                        FindFirstObjectByType<CigaretteSpawner>();

                    if (spawner != null)
                        spawner.CigaretteUsed();

                    return;
                }
            }
        }

        transform.position = previousPosition;

        if (rolling != null)
            rolling.enabled = true;
    }
}
