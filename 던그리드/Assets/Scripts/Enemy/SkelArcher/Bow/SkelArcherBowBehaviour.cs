using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelArcherBowBehaviour : MonoBehaviour
{
    [SerializeField] SkelArcherBehaviour skelArcher;
    [SerializeField] Animator bowAnimator;
    [SerializeField] ObjectPool ArrowPool;
    [SerializeField] SFXPool bowSfxPool;

    public void AimPlayer()
    {
        Vector2 direction = GameManager.Instance.player.transform.position - transform.position;
        transform.right = direction.normalized;
    }

    public void Ready()
    {
        bowSfxPool.Play("Ready");
    }

    public void Shot()
    {
        bowSfxPool.Play("Shot");
        ArrowPool.GetObject().GetComponent<SkelArcherArrowBehaviour>().Shot(transform.position, transform.right, skelArcher.Damage);
    }
}
