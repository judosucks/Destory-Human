using System;
using UnityEngine;
using UnityEngine.InputSystem;


    public class PlayerInputController:MonoBehaviour
    {
        private PlayerInput playerInput;
        public Player player;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            
            Debug.Log(playerInput+" "+player);
        }
    }
