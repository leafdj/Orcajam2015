﻿using UnityEngine;
using System.Collections;

public class PlayerLogic : Logic {

    #region Player Stats
    public int Level = 1;
    #endregion
    #region Components
    private GameEntity coreComponent;
    private Rigidbody2D rigidBodyRef;
    #endregion
    #region Control Information
    private int inputID;  // The rewired player.
    private Vector2 moveVector;
    private Vector2 aimVector;
    #endregion
    #region AI
    //private GameSettings.TeamActor actor; // TODO-DG: We could totally put AI in woah
    #endregion
    #region Movement
    public float Acceleration = 5.0f;
    public float MaxSpeed = 3.0f;
    #endregion
    #region Attacking
    private float attackCooldown = 0.5f;
    private int bulletDamage = 10;
    public int BulletDamage { get { return bulletDamage; } }
    private float bulletSpeed = 25.0f;
    public float BulletSpeed { get { return bulletSpeed; } }
    private float bulletRange = 3.0f;
    public float BulletRange { get { return bulletRange; } }

    private bool bulletPenetration = false;
    public bool BulletPenetration { get { return bulletPenetration; } }
    #endregion
    #region Death and Undying
    public bool Dead = false;
    private bool respawnable = false;
    private float respawnTimer;
    private float initialRespawnTime = 10.0f;
    private float respawnTimeIncrease = 20.0f;
    private Vector2 spawnPos;
    private Quaternion spawnRot;
    #endregion

    public void Init(GameController gc, int playerID, Vector2 spawnPos, Quaternion spawnRot) {
        this.spawnPos = transform.position = spawnPos;
        this.spawnRot = spawnRot;
        coreComponent = GetComponent<GameEntity>();
        coreComponent.GameController = gc;
        rigidBodyRef = GetComponent<Rigidbody2D>();
    }

    float shotTimer = 0.0f;
	void Update () {
        if (Dead) {
            if (respawnable) {
                if (respawnTimer <= 0f) {
                    Respawn();
                } else {
                    respawnTimer -= Time.deltaTime;
                }
            }
        } else {
            if (shotTimer > 0.0f) {
                shotTimer -= Time.deltaTime;
            }
        }
        
        //if (actor == GameSettings.TeamActor.Human) {
            if (!Dead)
                ProcessMovementInput();
        //} else {
            // TODO-DG: AI for computer players
        //}
	}

    /**
     * Function that changes AI difficulty and to and from human
     **/
    /*public void ChangeActor(GameSettings.TeamActor newActor, int id) {
        if (actor != newActor) {
            // Stop behaviour coroutine, start new coroutine or set human.
            actor = newActor;
            switch (newActor) {
                case GameSettings.TeamActor.Human:
                    this.inputID = id;
                    break;
                    // TODO-DG: Assign proper behaviour coroutine.
            }
        } else {
            if (newActor == GameSettings.TeamActor.Human) {
                this.inputID = id;
            }
        }
    }*/

    private void ProcessMovementInput() {
        moveVector = PlayerInputController.Instance.GetMovement(inputID);
        aimVector = PlayerInputController.Instance.GetAim(inputID);

        MovePlayer();

        if (aimVector.x != 0.0f || aimVector.y != 0.0f) {
            if (shotTimer <= 0.0f) { // check cooldown and fire, if possible
                // Shoot!
                //coreComponent.GameController.ShootBullet(aimVector, this);
                shotTimer = attackCooldown;
            }
        }
    }

    private void MovePlayer() {
        if (moveVector.x != 0.0f || moveVector.y != 0.0f) {
            rigidBodyRef.AddForce(moveVector * Acceleration);
            if (rigidBodyRef.velocity.magnitude > MaxSpeed) {
                rigidBodyRef.velocity = rigidBodyRef.velocity.normalized * MaxSpeed;
            }
            coreComponent.ChildSprite.transform.up = moveVector.normalized;
        }
    }

    public void Respawn() {
        /*transform.position = spawnPos;
        Dead = false;
        GetComponent<Attackable>().ResetHealth();
        coreComponent.ChangeSprite("Level1", true); // TODO-DG: Load for current level
        coreComponent.ChildSprite.transform.rotation = spawnRot;*/
    }

    public override void Kill() {
        GetComponent<Collider2D>().enabled = false;
        Dead = true;
        if (respawnable) {
            respawnTimer = initialRespawnTime;
            initialRespawnTime += respawnTimeIncrease;
        }
    }

    public override void ChangeTeam(int newTeam) {
        // Nothing in here yet
    }
}