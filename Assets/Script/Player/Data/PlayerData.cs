using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


[CreateAssetMenu(fileName ="newPlayerData",menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
 
    [Header("Collision info")] 
     public float attackCheckRadius;
     public float groundCheckDistance;
     public float wallCheckDistance;
     public float frontBottomCheckDistance;
     public float bottomGroundCheckDistance;
     public float wallTopCheckDistance;
     public float headCheckDistance;
     public LayerMask whatIsGround;
     public float ledgeCheckDistance;
     public float fontGroundCheckDistance;
     public float wallBackCheckDistance;
     public float edgeGroundDistance;
     [Header("Gravity info")]
     public float gravityMultiplier;
     public float maxFallSpeed;
     public float fallForce;
     [Header("wallslide info")] 
     public float wallSlideDownForce;
     public float climbUpForce;
     public bool isWallSliding;
     public float exitSlideForce =20f;
     [Header("ledge info")]
     public Vector2 startOffset;
     public Vector2 stopOffset;
     public bool isHanging;
     public bool isClimbLedge;
     
     [Header("throw grenade info")]
     public bool isThrowComplete;
     
     [Header("blackhole info")] 
     [Header("grenade info")]
     public bool isAiming;
     public bool isAimCheckDecided;
     public bool rightButtonLocked;
     public bool grenadeCanceled;
     public bool mouseButttonIsInUse;
     [Header("Attack Details")] 
     public Vector2[] attackMovement;
     public float counterAttackDuration = .2f;
     [Header("Movement jump info")] 
     public float movementSpeed = 2f;
     public float horizontalSpeed = 1f;
     public float verticalAirSpeed = 3f;
     public float straightJumpForce = 6f;
     public float jumpForce = 6f;
     public float grenadeReturnImpact;
     public readonly float defaultMoveSpeed  = 2f;
     public readonly float defaultJumpForce  = 6f;
     public readonly float defaultStraightJumpForce  = 6f;
     [Header("snap grid info")] 
     public float gridSize = 1f;

     public float moveDistance = 0.5f;
     public float moveAlittleDistance = 0.2f;
     public float moveAlotDistance = 0.8f;
     public Vector2 moveDirection = Vector2.right;
     public Vector2 moveLeftDirection = Vector2.left;


     [Header("dash")]
     public readonly float defaultDashSpeed  = 2f;
     public float dashSpeed;
     public float dashDuration;
     [Header("status info")]
     public bool isRun;
     public bool isIdle;
     public bool isSprint;
     public bool isInAir;
     [Header("highest jump")] 
     public bool reachedApex;

     public float highestPoint = 0f;
     public readonly float defaultHighestPoint = 0f;
     [Header("stick on ground")] 
     public float stickingForce = 20f;

     public float maxPushForce = 50f;
     [Header("fall info")] 
     public float fallThreshold = 10f;
     private void OnEnable()
     {
         highestPoint = defaultHighestPoint;
         movementSpeed = defaultMoveSpeed;
         jumpForce = defaultJumpForce;
         straightJumpForce = defaultStraightJumpForce;
         dashSpeed = defaultDashSpeed;
         Debug.Log("player data enabled");
     }
}
