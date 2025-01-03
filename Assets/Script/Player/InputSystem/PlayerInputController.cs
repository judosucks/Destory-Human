using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
   public Vector2 RawMovementInput { get; private set; }
   public int norInputX { get; private set; }
   public int norInputY { get; private set; }

   public bool straightJumpInput { get; private set; }
   public bool runJumpInput { get; private set; }
   public bool sprintJumpInput { get; private set; }

   public void OnMovement(InputAction.CallbackContext context)
   {
      RawMovementInput = context.ReadValue<Vector2>();
      norInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
      norInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
   }

   public void OnStraightJumpInput(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         straightJumpInput = true;
      }
   }

   public void OnRunJumpInput(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         runJumpInput = true;
      }
   }

   public void OnSprintJumpInput(InputAction.CallbackContext context)
   {
      if (context.started)
      {
         sprintJumpInput = true;
      }
   }

   public void UseRunJumpInput()
   {
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

}
