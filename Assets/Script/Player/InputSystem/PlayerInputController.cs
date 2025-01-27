using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
   private Player player => GetComponent<Player>();
   private PlayerData playerData => player.playerData;
   public Vector2 RawMovementInput { get; private set; }
   public int norInputX { get; private set; }
   public int norInputY { get; private set; }
   
   public bool straightJumpInput { get; private set; }
   public bool runJumpInput { get; private set; }
   public bool sprintJumpInput { get; private set; }
   public bool sprintInput {get; private set;}
   [SerializeField] private float runJumpInputHoldTime = 0.2f;
   [SerializeField] private float straightJumpInputHoldTime = 0.2f;
   [SerializeField] private float sprintJumpInputHoldTime = 0.2f;
   private float runJumpInputStartTime;
   private float straightJumpInputStartTime;
   private float sprintJumpInputStartTime;
   public bool isJumping;
   private bool isRun;
   private bool isIdle;
   private bool isSprint;

   private void Update()
   {
      isRun = playerData.isRun;
      isIdle = playerData.isIdle;
      isSprint = playerData.isSprint;

      if (norInputX != 0 && isSprint)
      {
         
         CheckSprintJumpInputHoldTime();
      }

      if (norInputX != 0)
      {
         
         CheckRunJumpInputHoldTime();
         
      }

      if (norInputX == 0)
      {
         
         CheckStraightJumpInputHoldTime();
      }
}

   public void OnMovement(InputAction.CallbackContext context)
   {


      if (!player.isBusy)
      {
         RawMovementInput = context.ReadValue<Vector2>();
         norInputX = Mathf.RoundToInt((RawMovementInput * Vector2.right).normalized.x);
         norInputY = Mathf.RoundToInt((RawMovementInput * Vector2.up).normalized.y);

      }
         




   }

   public void OnSprintInput(InputAction.CallbackContext context)
   {
      if (context.performed && playerData.isRun)
      {
         
         sprintInput = true;
         
      }

      if (context.canceled)
      {
         UseSprintInput();
      }
   }
   public void OnStraightJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isIdle || context.started && playerData.isWallSliding)
      {
         isJumping = true;
         straightJumpInput = true;
         straightJumpInputStartTime = Time.time;
      }

     
   }

   public void OnRunJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isRun|| context.started && playerData.isWallSliding)
      {
         isJumping = true;
         runJumpInput = true;
         runJumpInputStartTime = Time.time;
      }

      
   }

   public void OnSprintJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isSprint|| context.started && playerData.isWallSliding)
      {
         isJumping = true;
         sprintJumpInput = true;
         sprintJumpInputStartTime = Time.time;
      }

      
   }

   public void UseRunJumpInput()
   {
      Debug.Log("UseRunJumpInput"+isJumping);
      runJumpInput = false;
      
   }


   public void UseSprintJumpInput()
   {
      sprintJumpInput = false;
      
   }


   public void UseStraightJumpInput()
   {
      straightJumpInput = false;
      
   }
   public void UseSprintInput()=>sprintInput = false;

   public void CancelAllJumpInput()
   {
      UseRunJumpInput();
      UseSprintJumpInput();
      UseStraightJumpInput();
   }
   
   private void CheckRunJumpInputHoldTime()
   {
      if (Time.time >= runJumpInputStartTime + runJumpInputHoldTime)
      {
         float time = runJumpInputStartTime + runJumpInputHoldTime;
         runJumpInput = false;
         
      }
   }

   private void CheckStraightJumpInputHoldTime()
   {
      if (Time.time >= straightJumpInputStartTime + straightJumpInputHoldTime)
      {
         float time = straightJumpInputStartTime + straightJumpInputHoldTime;
         straightJumpInput = false;
         
      }
   }

   private void CheckSprintJumpInputHoldTime()
   {
      if (Time.time >= sprintJumpInputStartTime + sprintJumpInputHoldTime)
      {
         float time = sprintJumpInputStartTime + sprintJumpInputHoldTime;
         sprintJumpInput = false;
         
      }
   }
}
