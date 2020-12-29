using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnumManager : MonoBehaviour
{
    public PlayerLookDirection lookDir { get; set; }
    public PlayerActionState actionState { get; set; }

    public PlayerThrowState throwState { get; set; }
}
