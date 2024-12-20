using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class CloneSkillController : MonoBehaviour
{
    private Player player;
    
    private SpriteRenderer sr;
    private Animator anim;
    private float attackMultiplier;
    [SerializeField] private float colorLoosingSpeed;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField]private Transform closestEnemy;
    [SerializeField] private float closestEnemyCheckRadius = 8;
    [SerializeField]private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    int ATTACKNUMBER = Animator.StringToHash("AttackNumber");
    [SerializeField] private float Radius = 10f;
    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private int facingDir = 1;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(FaceClosestTarget());

    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        
        if (cloneTimer < 0)
        {
            
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,bool _canDuplicate,float _chanToDuplicate, Player _player, float _attackMultiplier)
    {
       
       
            if (_canAttack)
            {
                int Number_Attack = Random.Range(1, 5);
                anim.SetInteger(ATTACKNUMBER,Number_Attack);
                
            }

            attackMultiplier = _attackMultiplier;
            player = _player;
            
            transform.position = _newTransform.position + _offset;
            cloneTimer = _cloneDuration;
            canDuplicateClone = _canDuplicate;
            chanceToDuplicate = _chanToDuplicate;
            
            
        
        
       
        
    }
    private void AnimationTrigger()
    {
        Debug.Log("animation trigger from cloneskillcontroller");
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();
                Debug.Log("enemy hit from cloneskillcontroller");
                hit.GetComponent<Enemy>().DamageEffect();
                player.stats.DoDamage(target);
                
            }
        }
    }

    private IEnumerator FaceClosestTarget()
    {
        yield return null;

        FindClosestEnemy();
        

            if (closestEnemy != null)
            {
               
                if (transform.position.x > closestEnemy.position.x)
                {
                    facingDir = -1;
                    Debug.Log("turn right ");
                    transform.Rotate(0, 180, 0);
                }
            }
        
    }

    private void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, closestEnemyCheckRadius,whatIsEnemy);
        
        float closestDistance = Mathf.Infinity;
        
        foreach (var hit in colliders)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = hit.transform;
            }
        }
    }

    private void OnDrawGizmos()
    {   Debug.Log("draw gizmos from cloneskillcontroller");
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,closestEnemyCheckRadius);
        Gizmos.DrawWireSphere(attackCheck.transform.position,attackCheckRadius);
    }
}
