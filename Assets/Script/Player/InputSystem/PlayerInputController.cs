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
   [SerializeField] private float inputHoldTime = 0.2f;
   private float jumpInputStartTime;
   private bool isRun;
   private bool isIdle;
   private bool isSprint;
   private void Update()
   {
      isRun = playerData.isRun;
      isIdle = playerData.isIdle;
      isSprint = playerData.isSprint;
      CheckJumpInputHoldTime();
   }

   public void OnMovement(InputAction.CallbackContext context)
   {
     
         
        
         RawMovementInput = context.ReadValue<Vector2>();
         norInputX = Mathf.RoundToInt((RawMovementInput * Vector2.right).normalized.x);
         norInputY = Mathf.RoundToInt((RawMovementInput * Vector2.up).normalized.y);





   }

   public void OnSprintInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isRun)
      {
         sprintInput = true;
         
      }
   }
   public void OnStraightJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isIdle)
      {
         straightJumpInput = true;
         jumpInputStartTime = Time.time;
      }
   }

   public void OnRunJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isRun)
      {
         runJumpInput = true;
         jumpInputStartTime = Time.time;
      }
   }

   public void OnSprintJumpInput(InputAction.CallbackContext context)
   {
      if (context.started && playerData.isSprint)
      {
         sprintJumpInput = true;
         jumpInputStartTime = Time.time;
      }
   }

   public void UseRunJumpInput()=>runJumpInput = false;
   

   public void UseSprintJumpInput()=>sprintJumpInput = false;
   

   public void UseStraightJumpInput()=>straightJumpInput = false;
   public void UseSprintInput()=>sprintInput = false;

   private void CheckJumpInputHoldTime()
   {
      if (Time.time >= jumpInputStartTime + inputHoldTime)
      {
         runJumpInput = false;
         sprintJumpInput = false;
         straightJumpInput = false;
      }
   }
}
