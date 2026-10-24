
using UnityEngine;
using System.Collections;

public class PackManager : MonoBehaviour
{
    public static PackManager instance;

    [Header("Packing")]
    public int maxCigarettes = 10;
    public int currentCigarettes = 0;
    public int completedPacks = 0;
    public Transform[] slots;

    [Header("Cigarette Size Inside Pack")]
    [Range(0.1f, 2f)]
    public float cigaretteWidth = 0.35f;

    [Range(0.1f, 2f)]
    public float cigaretteHeight = 0.45f;

    [Header("Pack Visuals")]
    public Sprite closedPackSprite;

    [Header("Conveyor Movement")]
    public float conveyorSpeed = 4f;
    public float exitX = -11f;
    public float closeDelay = 0.4f;
    public float newPackDelay = 0.3f;

    private Sprite openPackSprite;
    private SpriteRenderer sr;
    private bool isClosing = false;
    private Vector3 startingPosition;

    void Awake()
    {
        instance = this;

        sr = GetComponent<SpriteRenderer>();
        openPackSprite = sr.sprite;
        startingPosition = transform.position;
    }

    public bool CanPack()
    {
        return !isClosing &&
               currentCigarettes < maxCigarettes;
    }

    public bool AddCigarette(GameObject cigarette)
    {
        if (!CanPack()) return false;

        int index = currentCigarettes;

        if (index >= slots.Length || slots[index] == null)
            return false;

        CigaretteRolling rolling =
            cigarette.GetComponent<CigaretteRolling>();

        if (rolling != null)
            rolling.enabled = false;

        CigaretteDrag drag =
            cigarette.GetComponent<CigaretteDrag>();

        if (drag != null)
            drag.enabled = false;

        Rigidbody2D rb =
            cigarette.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        cigarette.transform.SetParent(slots[index], false);
        cigarette.transform.localPosition = Vector3.zero;
        cigarette.transform.localRotation = Quaternion.identity;
        cigarette.transform.localScale = new Vector3(
            cigaretteWidth,
            cigaretteHeight,
            1f
        );

        Collider2D col = cigarette.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        SpriteRenderer cigaretteSR =
            cigarette.GetComponent<SpriteRenderer>();

        if (cigaretteSR != null)
            cigaretteSR.sortingOrder = 11;

        currentCigarettes++;

        if (currentCigarettes >= maxCigarettes)
            StartCoroutine(FinishPack());

        return true;
    }

    IEnumerator FinishPack()
    {
        isClosing = true;

        // Wait before closing
        yield return new WaitForSeconds(closeDelay);

        // Hide cigarettes inside
        foreach (Transform slot in slots)
        {
            if (slot == null) continue;

            foreach (Transform child in slot)
                child.gameObject.SetActive(false);
        }

        // Close the pack
        if (closedPackSprite != null)
            sr.sprite = closedPackSprite;

        completedPacks++;

        // Move closed pack to the left
        while (transform.position.x > exitX)
        {
            transform.position += Vector3.left *
                                  conveyorSpeed * Time.deltaTime;

            yield return null;
        }

        // Remove packed cigarettes
        foreach (Transform slot in slots)
        {
            if (slot == null) continue;

            for (int i = slot.childCount - 1; i >= 0; i--)
                Destroy(slot.GetChild(i).gameObject);
        }

        yield return new WaitForSeconds(newPackDelay);

        // Reset position and open pack
        transform.position = startingPosition;
        sr.sprite = openPackSprite;

        currentCigarettes = 0;
        isClosing = false;
    }
}
