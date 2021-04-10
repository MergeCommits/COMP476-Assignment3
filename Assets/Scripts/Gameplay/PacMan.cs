using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour {
    private PhotonView photonView;

    private LayerMask collision;

    private Vector2 movePosition;
    private float moveSpeed = 8f;
    
    private void Awake() {
        photonView = GetComponent<PhotonView>();
        
        collision = LayerMask.GetMask("Wall");

        movePosition = transform.position.XZ();
    }

    void FixedUpdate() {
        if (photonView.IsMine) {
            float speed = 1.5f;
            Vector2 direction = new Vector2(InputManager.movement.axisDelta.x, InputManager.movement.axisDelta.y);
            direction = Vector2.ClampMagnitude(direction, 1.0f);

            Vector2 displacement = direction * (speed * Time.deltaTime);
            transform.Translate(displacement.x, 0f, displacement.y);
            
            UpdateMovement();
        }
    }

    private void UpdateMovement() {
        transform.position = Vector3.MoveTowards(transform.position, movePosition.ToXZ(), moveSpeed * Time.deltaTime);

        if (!(Vector2.Distance(transform.position.XZ(), movePosition) <= 0.05f)) {
            return;
        }

        if (!Mathf.Approximately(InputManager.movement.axisDelta.x, 0f)) {
            if (InputManager.movement.axisDelta.x > 0f) {
                AttemptMove(new Vector2(1f, 0f));
            } else {
                AttemptMove(new Vector2(-1f, 0f));
            }
        } else if (!Mathf.Approximately(InputManager.movement.axisDelta.y, 0f)) {
            if (InputManager.movement.axisDelta.y > 0f) {
                AttemptMove(new Vector2(0f, 1f));
            } else {
                AttemptMove(new Vector2(0f, -1f));
            }
        }
    }

    private void AttemptMove(Vector2 offset) {
        Vector2 attemptPosition = movePosition + offset;
        if (SafeToMoveTowards(attemptPosition)) {
            movePosition = attemptPosition;
        }
    }

    private bool SafeToMoveTowards(Vector2 newPosition) {
        Collider[] results = new Collider[1];
        Physics.OverlapSphereNonAlloc(newPosition.ToXZ(), 0.4f, results, collision.value);
            
        return results[0] == null;
    }
}
