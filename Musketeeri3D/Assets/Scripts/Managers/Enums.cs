#region Player
public enum PlayerLookDirection
{
    Right,
    Left
}

public enum PlayerActionState
{
    Idle,
    Walk,
    Duck,
    Jump,
    Attack,
    PickingUp,
    Pushing,
    CarryObj,
    LoweringObj,
    TakingDamage,
    FreezeAction
}
public enum PlayerThrowState
{
    Idle,
    Throwing,
}
#endregion
public enum GameState
{
    Running,
    Pause
}

public enum StartPoint
{
    Apoint,
    Bpoint,
    Cpoint,
    Dpoint
}