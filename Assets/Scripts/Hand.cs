using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hand : MonoBehaviour
{
    public string handName; // ���� ������
    public float range; // ���� ����
    public int damage; // ���ݷ�
    public float workSpeed; // �۾� �ӵ�
    public float attackDelayA; // ���� �ӵ�
    public float attackDelayB; // ���� Ȱ��ȭ ����
    public float attackDelayC; // ���� ��Ȱ��ȭ ����

    public Animator aniM; // �ִϸ��̼�
}
