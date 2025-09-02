using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public enum NodeState
{
    Running,
    Success,
    Failure
}

public abstract class Node
{
    protected NodeState state;

    public abstract NodeState Evaluate();
}
}
