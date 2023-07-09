using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCtl : MonoBehaviour
{
    //���� ������ hand�� Ÿ�� ����
    [SerializeField]
    private Hand currentHand;
    //������
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitinfo;
    void Update()
    {
        TryAttack();
    }
    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                // �ڷ�ƾ ����
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.aniM.SetTrigger("Attack");
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = true;
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentHand.attackDelayC);
        isSwing = false;
        yield return new WaitForSeconds(currentHand.attackDelayA - currentHand.attackDelayB - currentHand.attackDelayC);
        isAttack = false;
    }
    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //�浹����
                isSwing = false;
                Debug.Log(hitinfo.transform.name);
            }
            yield return null;
        }
    }
    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitinfo, currentHand.range))
        {
            return true;
        }
        return false;  
    }
}
