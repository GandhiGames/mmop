using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D), typeof(PlayerControls))]
public class PlayerGrapplingHook : MonoBehaviour, MovementAction
{
    public float pullDistancePerSecond = 4;
    public float hookLineWidth = 0.05f;

    public Material hookLineMaterial;

    private DistanceJoint2D grapplingHook;
    private PlayerControls controls;
    private LineRenderer hookLine;
    private Transform hookAttachmentLoc;

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
        if(controls.IsMovementActionButtonPressed())
        {
            Vector2 targetDir = controls.GetAimDirection();

            var hit = Physics2D.Raycast(transform.position, targetDir);

            if(hit.collider != null)
            {
                var connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                if (connectedBody != null)
                { 
                    grapplingHook.connectedBody = connectedBody;
                    grapplingHook.connectedAnchor = connectedBody.transform.InverseTransformPoint(hit.point);
                    grapplingHook.distance = hit.distance;
                    grapplingHook.enabled = true;

                    hookLine.SetPosition(0, (Vector2)transform.position + grapplingHook.anchor);
                    hookLine.SetPosition(1, hit.point);
                    hookLine.enabled = true;

                    hookAttachmentLoc.position = hit.point;
                    hookAttachmentLoc.SetParent(hit.collider.transform);
                }
            }
        }
        else if(controls.IsMovementActionButtonHeld())
        {
            if (grapplingHook.distance > 0.2f)
            {
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
        else if(controls.IsMovementActionButtonReleased())
        {
            grapplingHook.enabled = false;
            hookLine.enabled = false;
        }
    }

}
