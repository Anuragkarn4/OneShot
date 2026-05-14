using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    [Header("References")]
    public Transform arrowTransform;        // drag Arrow child here
    public Transform bowTransform;          // drag Bow child here
    public GameObject arrowPrefab;          // your Arrow prefab

    [Header("Rotation Settings")]
    public float minAngle = -80f;           // max aim downward
    public float maxAngle = 80f;            // max aim upward
    public float rotationSmoothSpeed = 10f; // how smoothly bow follows mouse

    [Header("Power / Pull Settings")]
    public float minPower = 8f;
    public float maxPower = 30f;
    public float maxHoldTime = 2f;          // seconds to reach max power

    [Header("Arrow Spawn")]
    public Transform arrowSpawnPoint;       // empty child at arrow tip

    [Header("Reset Settings")]
    public Vector3 defaultPosition = new Vector3(-4f, 1f, 0f);
    public Quaternion defaultRotation;
    public float resetDelay = 0.8f;         // seconds before bow resets

    [Header("Aiming Area")]
    public float aimRadius = 2f;

    // Internal state
    private float holdTime = 0f;
    private bool isHolding = false;
    private bool isAiming = false;
    private bool hasArrow = true;
    private Camera mainCam;

    // For smooth rotation
    private float targetAngle = 0f;
    private float currentAngle = 0f;
    private Vector2 lastAimDir = Vector2.right;

    private float lastShotPercent = 0f;   // 0–1, how strong the last shot was

    void Start()
    {
        mainCam = Camera.main;
        defaultRotation = transform.rotation;
        defaultPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver) return;

         if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y,
                        mainCam.WorldToScreenPoint(transform.position).z));

            float dist = Vector2.Distance(mouseWorld, transform.position);
            if (dist <= aimRadius)
                isAiming = true;
        }

        if (Input.GetMouseButtonUp(0))
            isAiming = false;

        if (isAiming)
        {
            HandleAiming();      // only rotate while aiming
        }

        HandlePullAndRelease();
        UpdateArrowVisual();
    }

    // ─────────────────────────────────────────
    // AIMING: rotate bow to point toward mouse
    // ─────────────────────────────────────────
    void HandleAiming()
    {
        // Get mouse position in world space
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
                        Input.mousePosition.y,
                        mainCam.WorldToScreenPoint(transform.position).z));

        // Direction from bow to mouse
        Vector2 dir = (mouseWorld - transform.position);
        lastAimDir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Offset because the bow's "forward" may be along Y or X depending on model
        // Adjust the -90f offset below if your arrow points the wrong way
        angle -= 35f;

        // Clamp so bow stays in a reasonable arc
        angle = Mathf.Clamp(angle, minAngle, maxAngle);
        targetAngle = angle;

        // Smooth rotation
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle,
                                        rotationSmoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    // ─────────────────────────────────────────
    // PULL: hold mouse button to charge power
    // RELEASE: fire arrow
    // ─────────────────────────────────────────
    void HandlePullAndRelease()
    {
        if (!hasArrow) return;

        if (Input.GetMouseButton(0))
        {
            isHolding = true;
            holdTime += Time.deltaTime;
            holdTime = Mathf.Min(holdTime, maxHoldTime);
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            float percent = holdTime / maxHoldTime;
            float power = Mathf.Lerp(minPower, maxPower, percent);

            lastShotPercent = percent;
            FireArrow(power);
            isHolding = false;
            holdTime = 0f;
        }
    }

    // ─────────────────────────────────────────
    // VISUAL: compress arrow while pulling
    // ─────────────────────────────────────────
    void UpdateArrowVisual()
    {
        if (arrowTransform == null || !hasArrow) return;

        if (isHolding)
        {
            float t = holdTime / maxHoldTime;
            // Pull arrow back along local axis (Z or Y depending on model)
            float pullDistance = Mathf.Lerp(0f, -0.5f, t);
            arrowTransform.localPosition = new Vector3(pullDistance, 0f, 0f);

            // Optional: slightly squish the bow scale to show tension
            bowTransform.localScale = new Vector3(
                1f + t * 0.05f,
                1f - t * 0.04f,
                1f);
        }
        else
        {
            // Return arrow and bow to neutral
            arrowTransform.localPosition = Vector3.Lerp(
                arrowTransform.localPosition, Vector3.zero, Time.deltaTime * 10f);
            bowTransform.localScale = Vector3.Lerp(
                bowTransform.localScale, Vector3.one, Time.deltaTime * 10f);
        }
    }

    // ─────────────────────────────────────────
    // FIRE
    // ─────────────────────────────────────────
    void FireArrow(float power)
    {
        hasArrow = false;

        // Hide the visual arrow child
        if (arrowTransform != null)
            arrowTransform.gameObject.SetActive(false);

        // Spawn real physics arrow prefab
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            GameObject fired = Instantiate(arrowPrefab,
                                           arrowSpawnPoint.position,
                                           arrowSpawnPoint.rotation);
            Rigidbody rb = fired.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.drag = 0f;
                rb.angularDrag = 0f;


                // Direction from bow to mouse at release
                Vector3 dir = arrowSpawnPoint.right;
                if (dir == Vector3.zero)
                    dir = Vector3.right;

                rb.velocity = dir.normalized * power;
                
            
            }

            ArrowProjectile proj = fired.GetComponent<ArrowProjectile>();
            if (proj != null)
                proj.SetOwner(this);
        }

        // Schedule reset
        Invoke(nameof(ResetBow), resetDelay);
    }

    // ─────────────────────────────────────────
    // RESET
    // ─────────────────────────────────────────
    public void ResetBow()
    {
        hasArrow = true;
        holdTime = 0f;
        isHolding = false;

        // Restore visual arrow child
        if (arrowTransform != null)
        {
            arrowTransform.localPosition = Vector3.zero;
            arrowTransform.gameObject.SetActive(true);
        }

        // Restore bow scale
        if (bowTransform != null)
            bowTransform.localScale = Vector3.one;
    }

    // ─────────────────────────────────────────
    // POWER BAR: read by UIManager
    // ─────────────────────────────────────────
    public float GetPowerPercent()
    {
        return holdTime / maxHoldTime;
    }

    public bool IsHolding() => isHolding;
    public bool HasArrow() => hasArrow;

    public float GetLastShotPercent() => lastShotPercent;
}
