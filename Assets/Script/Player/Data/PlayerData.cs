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
     public float ceilingCheckDistance;
     public LayerMask groundAndEdgeLayer;

     public LayerMask whatIsGround;
     public LayerMask whatIsEdge;
     public LayerMask whatIsWall;
     public float slopeCheckDistance;
     public float ledgeCheckDistance;
     public float frontGroundCheckDistance;
     public float wallBackCheckDistance;
     public float edgeGroundDistance;
     public float edgeCheckDistance;
     public float edgeWallCheckDistance;
     [Header("Gravity info")]
     public float gravityMultiplier;
     public float maxFallSpeed;
     public float fallForce;
     [Header("wallslide info")] 
     public float wallSlideVelocity = 3f;
     public float wallSlideDownForce;
     public float climbUpForce;
     public bool isWallSliding;
     public float exitSlideForce =20f;
     [Header("climb state")]
     public float wallClimbVelocity = 3f;
     [Header("ledge info")]
     
     public bool isHanging = false;
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
     public float defaultMoveSpeed = 2f;
     public float defaultJumpForce  = 6f;
     public float defaultStraightJumpForce  = 6f;
     public int amountOfJumps = 1;
     public float coyoteTime = 0.2f;
     public float variableJumpHeightMultiplier = 0.5f;
     
     [Header("Crouch States")]
     public float crouchMovementSpeed = 0.6f; // 蹲下移动速度

     [Header("Collider Sizes")]
     public Vector2 crouchColliderSize = new Vector2(0.34f, 0.53f); // 蹲下状态碰撞体尺寸 (宽度, 高度)
     public Vector2 standColliderSize = new Vector2(0.34f, .93f);   // 站立状态碰撞体尺寸 (宽度, 高度)

     [Header("Collider Offsets")]
     public Vector2 crouchColliderOffset = new Vector2(0f, 0.27f); // 蹲下状态碰撞体偏移
     public Vector2 standColliderOffset = new Vector2(0f, 0.47f);   // 站立状态碰撞体偏移
     [Header("Raycast Settings")]
     
     public float ceilingCheckOffset = 0.015f; // 射线高度偏移
     [Header("snap grid info")] 
     public float gridSize = 1f;

     public float moveDistance = 0.5f;
     public float moveAlittleDistance = 0.2f;
     public float moveAlotDistance = 0.8f;
     public Vector2 moveDirection = Vector2.right;
     public Vector2 moveLeftDirection = Vector2.left;
     [Header("clone info")] 
     public float closestEnemyCheckRadius = 8;

     public LayerMask whatIsEnemy;
     [Header("dash")]
     public readonly float defaultDashSpeed  = 2f;
     public float dashSpeed;
     public float dashDuration;
     [Header("status info")]
     public bool isRun;
     public bool isIdle;
     public bool isSprint;
     public bool isInAir;
     public bool isJumpState;
     public bool isWallSlidingState;
     public bool isClimbLedgeState;
     public bool isGrenadeState;
     public bool isCounterAttackState;
     public bool isBlackholeState;
     public bool isRunJumpLandState;
     public bool isEdgeClimbState;
     public bool isLedgeClimbState;
     public bool isCrouchMoveState;
     public bool isCrouchIdleState;
     public bool isSlopeClimbState;
     [Header("highest jump")] 
     public bool reachedApex;

     public float highestPoint = 0f;
     public readonly float defaultHighestPoint = 0f;
     [Header("stick on ground")] 
     public float stickingForce = 20f;

     public float maxPushForce = 20f;
     [Header("fall info")] 
     public float fallThreshold = 10f;

     [Header("slope info")] 
     
     
     [Header("Wall Jump Info")]
     public float wallJumpVelocity = 6f;
     public float wallJumpTime = 0.4f;
     public Vector2 wallJumpAngle = new Vector2(1, 2);
     [Header("ledge edge offset info")]
     public Vector2 startOffset;
     public Vector2 stopOffset;
     public Vector2 startEdgeOffset;
     public Vector2 stopEdgeOffset;
     private void OnEnable()
     {
         isCrouchIdleState = false;
         isCrouchMoveState = false;
         isRun=false;
         isIdle=false;
         isSprint=false;
         isInAir=false;
         isJumpState=false;
         isWallSlidingState=false;
         isClimbLedgeState=false;
         isGrenadeState=false;
         isCounterAttackState = false;
         isBlackholeState=false;
         isRunJumpLandState=false;
         isEdgeClimbState=false;
         isLedgeClimbState=false;
         highestPoint = defaultHighestPoint;
         movementSpeed = defaultMoveSpeed;
         jumpForce = defaultJumpForce;
         straightJumpForce = defaultStraightJumpForce;
         dashSpeed = defaultDashSpeed;
         Debug.Log("player data enabled");
     }
}