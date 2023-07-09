using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hand : MonoBehaviour
{
    public string handName; // 무장 아이템
    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelayA; // 공격 속도
    public float attackDelayB; // 공격 활성화 시점
    public float attackDelayC; // 공격 비활성화 시점

    public Animator aniM; // 애니메이션
}
