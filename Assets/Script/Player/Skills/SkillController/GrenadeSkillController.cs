using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GrenadeSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D _circleCollider2D;
    private Player player;

    

    [Header("grenade info")] 
    [SerializeField]private GameObject grenadeExplodeFx;
    
    private bool explosion;
    [SerializeField] private Material hitMat;
    private Material originalMat;
    private SpriteRenderer sr;
    [SerializeField]private float explosionTimer;
    [SerializeField]private AnimationCurve flashRateCurve;
    public bool isFlashing { get; private set; }
    public float currentTimer { get; private set; }
    [Header("frag grenade")] 
    private List<Transform> enemyTarget;
    private bool isFragGrenade;
    [Header("flash grenade")] 
    private bool isFlashGrenade;
    private int targetIndex;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        

    }

    private void Start()
    {
        currentTimer = explosionTimer;
        originalMat = sr.material;
    }

    private IEnumerator CoundownToExplode()
    {
        isFlashing = true;
        while (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            if (isFlashing)
            {
                float flashRate = flashRateCurve.Evaluate(1-(currentTimer/explosionTimer));
                StartCoroutine(GrenadeFlashFx(1 / flashRate));
                yield return new WaitForSeconds(explosionTimer);
                explosion = true;
                isFlashing = false;
                if (!isFlashing && explosion)
                {
                    Debug.Log("explosiontriggered");
                
                    anim.SetBool("Rotation",false);
            
                    _circleCollider2D.enabled = false;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
            
                
        
                    GrenadeExplosion(); 
                }
            }
            ;
            
        }
        
    }

    private IEnumerator GrenadeFlashFx(float _flasDuration)
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(_flasDuration/2);
        sr.material = originalMat;
        yield return new WaitForSeconds(_flasDuration/2);
        
    }
    private void Update()
    {
        // if (canRotate)
        // {
        //     Debug.Log("can rotate");
        //     transform.right = rb.linearVelocity;
        // }

        // if (explosion)
        // {
        //      Debug.Log("camera follow grenade from grenadeskillcontroller");
        //      CameraManager.instance.newCamera.FollowGrenade();
        // }
        // else if(!explosion)
        // {
        //     Debug.Log("camera follow player from grenadeskillcontroller");
        //     CameraManager.instance.newCamera.FollowPlayer(CameraManager.instance.newCamera.grenadeCamera);
        // }
        
    }

    public void SetupGrenade(Vector2 _direction, float _gravity, Player _player)
    {
        Debug.Log("setup grenade");
        if (player == null || rb == null)
        {
            Debug.Log("player or rb is null from setup grenade");
            player = PlayerManager.instance.player;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            if (player && rb && _circleCollider2D != null && anim != null)
            {
                Debug.Log("player rb anim col not null from setup grenade");
                player = _player;
                rb.linearVelocity = _direction;
                rb.gravityScale = _gravity;
                anim.SetBool("Rotation",true);
                
                StartCoroutine(CoundownToExplode());

            }
            
        }
        
    }
    
    public void SetupFragGrenade(bool _isFragGrenade)
    {
        Debug.Log("set up frag grenade");
        isFragGrenade = _isFragGrenade;
        enemyTarget = new List<Transform>();
    }

    public void SetupFlashGrenade(bool _isFlashGrenade)
    {
        Debug.Log("set up flash grenade");
        isFlashGrenade = _isFlashGrenade;
        enemyTarget = new List<Transform>();
    }
    public void ReturnGrenade()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemy"))
        {
            // 减少弹跳力
            // 调整因子以获得所需的反弹效果
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,Mathf.Abs(rb.linearVelocity.y) * 0.7f); // 例如，把垂直速度减少到原来的70%
            // 其他与反弹相关的逻辑
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //    
    //     // collision.GetComponent<Enemy>().Damage();
    //     // if (collision.GetComponent<Enemy>() != null)
    //     // {
    //     //     
    //     //     if (isFragGrenade && enemyTarget.Count <= 0)
    //     //     {
    //     //         Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
    //     //         foreach (var hit in colliders)
    //     //         {
    //     //             if (hit.GetComponent<Enemy>() != null)
    //     //             {
    //     //                 enemyTarget.Add(hit.transform);
    //     //             }
    //     //         }
    //     //     }
    //     // }
    //     if (collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
    //     {
    //         Debug.Log("grenade on trigger enter enemy or ground");
    //        
    //        
    //         
    //     }
    //     
    // }
    // 定义触发生命的逻辑
    public void OnChildTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Debug.LogError("trigger detected by child"+ other.gameObject.name);
            EnemyStats target = other.GetComponent<EnemyStats>();
            player.stats.DoDamage(target);
        
        
        }
    }
    private void GrenadeExplosion()
    {
        grenadeExplodeFx.SetActive(true);
        grenadeExplodeFx = GetComponent<GrenadeExplodeAnimation>().gameObject;
        grenadeExplodeFx.transform.parent = null;
        Animator grenadeAnim = grenadeExplodeFx.GetComponent<Animator>();
        
        AnimatorStateInfo grenadeAnimStateInfo = grenadeAnim.GetCurrentAnimatorStateInfo(0);
        grenadeAnim.SetBool("GrenadeExplodeFx",true);
        
        if (grenadeAnimStateInfo.IsName("GrenadeExplodeFx"))
        {
            Debug.LogError("animation is playing from grenadeexplosion");
            if (grenadeAnimStateInfo.normalizedTime >= 1f)
            {
                Debug.Log("animation is done from grenadeexplosion");
                grenadeExplodeFx.SetActive(false);
                grenadeAnim.SetBool("GrenadeExplodeFx",false);
                player.ClearGrenade();    
                explosion = false;
        
        
                _circleCollider2D.enabled = true;
                transform.parent = null;
        
                rb.bodyType = RigidbodyType2D.Dynamic;
                CameraManager.instance.newCamera.FollowPlayer(CameraManager.instance.newCamera.grenadeCamera);
                // 例如：生成粒子特效、伤害附近的敌人等
                
                // 例如：Instantiate(explosionPrefab, position, Quaternion.identity);
            }
        }
    }
}
