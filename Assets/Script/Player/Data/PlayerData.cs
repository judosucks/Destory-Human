using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="newPlayerData",menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
 
    [Header("Collision info")] 
     public float attackCheckRadius;
     public float groundCheckDistance;
     public float wallCheckDistance;
     public float headCheckDistance;
     public LayerMask whatIsGround;
     public float ledgeCheckDistance;
     public float fontGroundCheckDistance;
     [Header("Gravity info")]
     public float gravityMultiplier;
     public float maxFallSpeed;
     public float fallForce;
     [Header("wallslide info")] 
     public float wallSlideDownForce;
     public float climbUpForce;
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
     public float straightJumpForce = 5f;
     public float jumpForce = 6f;
     public float grenadeReturnImpact;
     public float defaultMoveSpeed;
     public float defaultJumpForce;
     public float defaultStraightJumpForce;
     
     
     [Header("dash")] 
     public float defaultDashSpeed;
     public float dashSpeed;
     public float dashDuration;
     [Header("status info")]
     public bool isRun;
     public bool isIdle;
     public bool isSprint;
     public bool isInAir;

}
