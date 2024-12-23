using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemyVisualMovement : MonoBehaviour {
    private Vector3 movementDirection;
    private float speed;

    void Update() {
        // Move the enemy in the specified direction
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }

    public void SetMovementDirection(Vector3 direction) {
        movementDirection = direction;
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
}
