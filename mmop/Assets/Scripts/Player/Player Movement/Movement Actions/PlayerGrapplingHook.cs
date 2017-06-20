using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: look into better movement for shooting/retracting (rather that a linear speed).
[RequireComponent(typeof(DistanceJoint2D), typeof(PlayerControls))]
public class PlayerGrapplingHook : MonoBehaviour, MovementAction
{
    public float pullDistancePerSecond = 4;
    public float hookLineWidth = 0.05f;
    public float hookSpeed = 4f;
    public float retractionSpeed = 8f;
    public float maxDistance = 10f;
    public float overRunCatchDistance = 0.8f;
    public Transform shootDir;

    public Material hookLineMaterial;

    private static readonly float LOOK_AHEAD_DISTANCE = 0.2f;

    private DistanceJoint2D grapplingHook;
    private PlayerControls controls;
    private LineRenderer hookLine;
    private Transform hookAttachmentLoc;
    private bool isFindingTarget = false;
    private bool isRetracting = false;
    private Vector2 direction;

    void Awake()
    {
        grapplingHook = GetComponent<DistanceJoint2D>();
        controls = GetComponent<PlayerControls>();

        var otherMoveActions = GetComponents<MovementAction>();

        if (otherMoveActions.Length > 1)
        {
            Debug.LogWarning("Only one movement action should be attached to a character");
        }
    }

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        Physics2D.queriesHitTriggers = false;

        grapplingHook.enabled = false;

        var newObj = new GameObject("Grappling Hook Line");
        hookLine = newObj.AddComponent<LineRenderer>();
        hookLine.enabled = false;
        hookLine.positionCount = 2;
        hookLine.startWidth = hookLineWidth;
        hookLine.endWidth = hookLineWidth;
        hookLine.material = hookLineMaterial;

        hookAttachmentLoc = new GameObject("Grappling Hook Attach Location").transform;
    }

    void Update()
    {
        if(isRetracting)
        {
            RetractHook();
        }
        else if (isFindingTarget && controls.IsMovementActionButtonHeld())
        {
            FindTarget();
        }

        if(controls.IsMovementActionButtonPressed())
        {
            StartHookMovement();
        }
        else if(!isFindingTarget && !isRetracting && controls.IsMovementActionButtonHeld())
        {
            UpdateHookPosition();
        }
        else if(controls.IsMovementActionButtonReleased())
        {
            grapplingHook.enabled = false;
            isRetracting = true;
        }
    }

    private void RetractHook()
    {
        isFindingTarget = false;

        hookLine.SetPosition(0, (Vector2)transform.position + grapplingHook.anchor);
        hookLine.SetPosition(1, (Vector2)hookLine.GetPosition(1) + (Vector2)(hookLine.GetPosition(0) - hookLine.GetPosition(1)).normalized * retractionSpeed * Time.deltaTime);

        if (Vector2.Distance(hookLine.GetPosition(0), hookLine.GetPosition(1)) < 0.2f)
        {
            hookLine.enabled = false;
            isRetracting = false;
        }
        else if(controls.IsMovementActionButtonHeld())
        {
            CheckTargetInFront();
        }
    }

    private void FindTarget()
    {
        hookLine.SetPosition(0, (Vector2)transform.position + grapplingHook.anchor);
        hookLine.SetPosition(1, (Vector2)hookLine.GetPosition(1) + direction * hookSpeed * Time.deltaTime);

        var distance = Vector2.Distance(hookLine.GetPosition(0), hookLine.GetPosition(1));

        if (distance > maxDistance)
        {
            print("Reached max distance");
            isRetracting = true;
            return;
        }

        var blockedHit = Physics2D.Raycast((Vector2)hookLine.GetPosition(0), (hookLine.GetPosition(1) - hookLine.GetPosition(0)).normalized, distance);

        if (blockedHit.collider != null)
        {
            print("Blocked by " + blockedHit.collider.gameObject.name);

            if(Vector2.Distance(blockedHit.point, hookLine.GetPosition(1)) < overRunCatchDistance)
            {
                var connectedBody = blockedHit.collider.gameObject.GetComponent<Rigidbody2D>();

                if (connectedBody != null)
                {
                    print("Connected to blocked");
                    ConnectToTarget(connectedBody, blockedHit);
                }
                else
                {
                    isRetracting = true;
                }
            }
            else
            {
                isRetracting = true;
            }
            
            return;
        }

        CheckTargetInFront();
    }

    private void CheckTargetInFront()
    {
        var hit = Physics2D.Raycast((Vector2)hookLine.GetPosition(1), direction, LOOK_AHEAD_DISTANCE);

        if (hit.collider != null)
        {
            var connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();

            if (connectedBody != null)
            {
                ConnectToTarget(connectedBody, hit);
            }
        }
    }

    private void ConnectToTarget(Rigidbody2D target, RaycastHit2D hit)
    {
        grapplingHook.connectedBody = target;
        grapplingHook.connectedAnchor = target.transform.InverseTransformPoint(hit.point);
        grapplingHook.distance = Vector2.Distance(hookLine.GetPosition(0), hookLine.GetPosition(1));
        grapplingHook.enabled = true;

        hookLine.SetPosition(0, (Vector2)transform.position + grapplingHook.anchor);
        hookLine.SetPosition(1, hit.point);
        hookLine.enabled = true;

        hookAttachmentLoc.position = hit.point;
        hookAttachmentLoc.SetParent(hit.collider.transform);

        isFindingTarget = false;
        isRetracting = false;
    }

    private void StartHookMovement()
    {
        direction = (shootDir.position - transform.position).normalized;

        hookLine.enabled = true;

        hookLine.SetPosition(0, (Vector2)transform.position + grapplingHook.anchor);
        hookLine.SetPosition(1, (Vector2)transform.position + grapplingHook.anchor);

        isFindingTarget = true;
        isRetracting = false;
    }

    private void UpdateHookPosition()
    {
        if(!grapplingHook.enabled)
        {
            return;
        }

        if (grapplingHook.distance > 0.2f)
        {
            var blockedHit = Physics2D.Raycast((Vector2)hookLine.GetPosition(0), direction, grapplingHook.distance);

            if (blockedHit.collider != null
                && blockedHit.collider.gameObject.GetInstanceID() != grapplingHook.connectedBody.gameObject.GetInstanceID()
                && blockedHit.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID())
            {
                print("Blocked while moving " + blockedHit.collider.gameObject.name);
                isRetracting = true;
                return;
            }

            grapplingHook.distance -= pullDistancePerSecond * Time.deltaTime;

            hookLine.SetPosition(0, (Vector2)transform.position + grapplingHook.anchor);
            hookLine.SetPosition(1, hookAttachmentLoc.position);
        }
        else
        {
            grapplingHook.enabled = false;
            hookLine.enabled = false;
        }
    }

}
