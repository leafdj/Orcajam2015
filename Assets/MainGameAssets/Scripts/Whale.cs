﻿using System;
using UnityEngine;

public class Whale : MonoBehaviour {

	float rotUpDown = 0f;
	float rotLeftRight = 0f;

	public float constantSpeed;
	public float mouseSensitivity = 5.0f;
	public float upDownRange = 60.0f;
	public Camera playerCamera;

    private Vector3 topLeft;
    private Vector3 bottomRight;

    // TODO-DG: Get Screen Bounds (of camera, also for player boats
    // TODO-DG: Implement breath for whale, have to recharge by coming up for air
    // TODO-DG: Whale location appears as dark spot when close-ish to surface
    // TODO-DG: Bubbles to show whale randomly as oxygen runs out.
    // TODO-DG: move away from walls and foghorns, move towards open water (shrimp?)

    private Transform childModel;
    [SerializeField] private GameController gameController;

    private bool isAI = true; // Set to false when there is an oculus player.

    void Start() {
        childModel = transform.Find("whalewhale");

    }

	void Update () {
        if (isAI) {
            // Move this whale all around, take care of dark spot when close to surface.
            //Vector2 facingDir = transform.position + coreComponent.ChildSprite.transform.up;
            //float angle = 0.0f;
            //float steerDirection = 0;
            /*for (int i = 0; i < nearbyObjects.Count; i++) {
                for (int j = 0; j < nearbyObjects[i].Count; j++) {
                    GameObject otherObject = nearbyObjects[i][j];
                    if (otherObject != gameObject) {    // Don't test on self
                        float distanceToObject = Vector2.Distance(otherObject.transform.position, transform.position) - (otherObject.GetComponent<GameEntity>().SpriteWidth + coreComponent.SpriteWidth)/2;
                        steerDirection = (facingDir.x - transform.position.x) * (otherObject.transform.position.y - transform.position.y) - (facingDir.y - transform.position.y) * (otherObject.transform.position.x - transform.position.x);

                        // Collision avoidance: (How is this different than separation right now?
                        if (otherObject.GetComponent<Collidable>() && distanceToObject < collisionDistance) {
                            if (Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(facingDir, moveTarget.transform.position) / (facingDir.magnitude * moveTarget.transform.position.magnitude)) < 20.0f) {
                                angle += (-steerDirection * collisionContribution) / (rigidbodyRef.velocity.magnitude * distanceToObject);
                            }
                        }

                        // Cohesion?
                        // Alignment?

                        // Separation:
                        if (distanceToObject < separationDistance) {
                            // TODO-DG: Maybe increase the separation distance?
                            angle += steerDirection * separationContribution / separationDistance;
                        }
                    }
                }
            }*/
            // "Follow the leader": (Used for heading to objective)
            /*steerDirection = (facingDir.x - transform.position.x) * (moveTarget.transform.position.y - transform.position.y) - (facingDir.y - transform.position.y) * (moveTarget.transform.position.x - transform.position.x);
            float targetAngle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(facingDir, moveTarget.transform.position) / (facingDir.magnitude * moveTarget.transform.position.magnitude));
            // TODO-DG: [Efficiency] Can maybe make this more efficient by just using the cross product to calc side and angle
            if (Mathf.Abs(steerDirection) > 0.1f && targetAngle < 8.0f) { // Don't contribute to target, we're going close enough
                steerDirection = 0.0f;
            }

            angle += steerDirection * targetAngle * targetContribution;

            if (angle > 170.0f) {
                angle = 170f;
            } else if (angle < -170f) {
                angle = -170f;
            } /*else if (Mathf.Abs(angle) < 30.0f) { // Prevent wobble! // DG: Aug. 18: Not the way to do this as it's affecting the exact opposite direction too.
                angle = 0f;
            }*/
            //angleToTurn = angle;
        } else {            
		    rotLeftRight = Input.GetAxis ("Mouse X") * mouseSensitivity;
		    transform.Rotate (0, rotLeftRight, 0);

		    rotUpDown -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		    rotUpDown = Mathf.Clamp (rotUpDown, -upDownRange, upDownRange);
		    playerCamera.transform.localRotation = Quaternion.Euler (rotUpDown, 0, 0);

		    Vector3 speed = new Vector3 (constantSpeed,0,0);
		    speed = transform.rotation * speed;

		    transform.position += transform.forward * Time.deltaTime * constantSpeed;
        }

        if (childModel.position.x > gameController.BottomRightScreenToWorld.x - childModel.GetComponent<MeshRenderer>().bounds.size.x / 2f) {
            //childModel.Translate(coreComponent.GameController.BottomRightScreenToWorld.x - childModel.position.x, 0f, 0f);
            childModel.transform.position = new Vector3(gameController.BottomRightScreenToWorld.x - childModel.GetComponent<MeshRenderer>().bounds.size.x / 2f, childModel.transform.position.y, childModel.transform.position.z);
        } else if (childModel.position.x < gameController.TopLeftScreenToWorld.x + childModel.GetComponent<MeshRenderer>().bounds.size.x / 2f) {
            childModel.transform.position = new Vector3(gameController.TopLeftScreenToWorld.x + childModel.GetComponent<MeshRenderer>().bounds.size.x / 2f, childModel.transform.position.y, childModel.transform.position.z);
        }

        if (childModel.position.z > gameController.TopLeftScreenToWorld.z - childModel.GetComponent<MeshRenderer>().bounds.size.z / 2f) {
            childModel.transform.position = new Vector3(childModel.transform.position.x, childModel.transform.position.y, gameController.TopLeftScreenToWorld.z - childModel.GetComponent<MeshRenderer>().bounds.size.z / 2f);
        } else if (childModel.position.z < gameController.BottomRightScreenToWorld.z + childModel.GetComponent<MeshRenderer>().bounds.size.z / 2f) {
            childModel.transform.position = new Vector3(childModel.transform.position.x, childModel.transform.position.y, gameController.BottomRightScreenToWorld.z + childModel.GetComponent<MeshRenderer>().bounds.size.z / 2f);
        }
	}
}