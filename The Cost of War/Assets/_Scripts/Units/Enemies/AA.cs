using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AA : Enemy
{
    private Operation op;

    private void Awake()
    {
        type = EnemyType.AA;
        SetHealth();
        op = GameObject.FindGameObjectWithTag("Operation").GetComponent<Operation>();
    }
}
