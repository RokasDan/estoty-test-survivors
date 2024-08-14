using System;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Input;
using RokasDan.EstotyTestSurvivors.Runtime.Components.Locomotion;
using UnityEngine;
using UnityEngine.Serialization;

namespace RokasDan.EstotyTestSurvivors.Runtime.Actors
{
    internal sealed class PlayerActor : MonoBehaviour, IPlayerActor
    {
        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private bool alwaysMaxSpeed = true;

        private IMovement playerMovement;
        private IPlayerInput playerInput;

        private void Awake()
        {
            playerInput = new SimplePlayerInput();

            playerMovement = alwaysMaxSpeed ? new MaxSpeedMovement() : new VariableSpeedMovement();
        }

        private void Update()
        {
            playerMovement.Move(transform, playerInput.GetPlayerDirection(), moveSpeed);
        }
    }
}

