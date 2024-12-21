using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class GrenadeSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D _circleCollider2D;
    private Player player;
    private GrenadeSkill grenadeSkill;
    

    [Header("grenade info")] 
    [SerializeField]private GameObject grenadeExplodeFx;
    private bool explosion;
    [SerializeField] private Material hitMat;
    private Material originalMat;
    private SpriteRenderer sr;
    
    [SerializeField]private AnimationCurve flashRateCurve;
    public bool isFlashing { get; private set; }
    
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
        grenadeSkill = SkillManager.instance.grenadeSkill;
        
    }

    private void Start()
    {
        
        originalMat = sr.material;
    }
    
    private IEnumerator CoundownToExplode()
    {
        Debug.Log("[CoundownToExplode] Countdown started...");
        float elapsedTime = 0f;

        // Maximum and minimum flash rates
        float maxFlashRate = 5f; // Flash 5 times per second when close to exploding
        float minFlashRate = 0.5f; // Flash every 2 seconds at the beginning

        while (elapsedTime < grenadeSkill.explosionTimer)
        {
            float remainingTime = grenadeSkill.explosionTimer - elapsedTime;

            // Dynamically calculate flash rate based on remaining time
            float flashRate = Mathf.Lerp(maxFlashRate, minFlashRate, remainingTime / grenadeSkill.explosionTimer); // Linearly interpolate

            Debug.Log($"[Flashing] Time Remaining: {remainingTime}, Flash Rate: {flashRate} flashes/second");

            // Flash effect starts here
            if (isFlashing)
            {
                StartCoroutine(GrenadeFlashFx(1 / flashRate));
            }

            // Wait based on the calculated flash rate
            yield return new WaitForSeconds(1 / flashRate);

            elapsedTime += 1 / flashRate; // Increment elapsed time based on flash

            Debug.Log($"[ElapsedTime] {elapsedTime}/{grenadeSkill.explosionTimer}");
        }

        Debug.Log("[CoundownToExplode] Countdown completed. Explosion has started.");
        isFlashing = false;
        explosion = true;

        GrenadeExplosion();
    }
            
            
        
        
    

    private IEnumerator GrenadeFlashFx(float flashInterval)
    {
        // Set grenade to flash on and off
        Renderer rend = GetComponent<Renderer>();

        // Flash on
        rend.material.color = Color.red; // Example color when flashing
        yield return new WaitForSeconds(flashInterval / 2);

        // Flash off
        rend.material.color = Color.white; // Default color
        yield return new WaitForSeconds(flashInterval / 2);
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
        if (!explosion)
        {
            isFlashing = true;
            Debug.Log("flashing");
        }
        else
        {
            isFlashing = false;
            Debug.Log("not flashing");
        }
    }

    public void SetupGrenade(Vector2 _direction, float _gravity, Player _player)
    {
        Debug.Log("设置手榴弹方向和引力");
        Debug.Log($"[SetupGrenade] Direction: {_direction}, Gravity: {_gravity}, Player: {_player}");
        // Existing initialization logic...
        player = _player ?? PlayerManager.instance.player; // 确保 player 不为空
        rb = rb ?? GetComponent<Rigidbody2D>();
        anim = anim ?? GetComponentInChildren<Animator>();
        _circleCollider2D = _circleCollider2D ?? GetComponent<CircleCollider2D>();

        if (rb != null)
        {
            rb.linearVelocity = _direction;
            rb.gravityScale = _gravity;
            anim.SetBool("Rotation", true);
            StartCoroutine(CoundownToExplode()); // 启动爆炸协程
        }
        else
        {
            Debug.LogError("手榴弹未正确设置刚体组件");
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
        Debug.Log("[GrenadeExplosion] Start explosion effects!");
        isFlashing = false;
        
        if (ExplosionEffect()) 
        {
            Debug.LogError("ExplosionEffect failed due to missing grenadeExplodeFx or other issues."); 
            return; 
        }

        
        
    }

    private bool ExplosionEffect()
    {
        if (grenadeExplodeFx == null)
        {
            Debug.LogError("grenadeexplodefx is null please assin the explosion effect in the inspector");
            return true;
        }
        // GameObject explodableFx = Instantiate(grenadeExplodeFx, transform.position, Quaternion.identity);
        // // 确保激活对象
        // explodableFx.SetActive(true);
        // 运行爆炸动画
        GameObject newGrenadeFx = GameObject.FindGameObjectWithTag("GrenadeExplodeFx");
        newGrenadeFx = Instantiate(grenadeExplodeFx, transform.position, Quaternion.identity);
        newGrenadeFx.SetActive(true);
        grenadeExplodeFx.SetActive(true);
        grenadeExplodeFx.transform.parent = null;
        CameraManager.instance.newCamera.FollowGrenadeExplosion();
        Animator grenadeAnim = newGrenadeFx.GetComponent<Animator>();
        
       
        if (grenadeAnim != null)
        {
            grenadeAnim.SetBool("GrenadeExplodeFx", true);
            StartCoroutine(DestroyExplosionEffect(newGrenadeFx, grenadeAnim));
            CameraManager.instance.newCamera.FollowPlayer(CameraManager.instance.newCamera.grenadeExplodeFxCamera);
        }
        else
        {
            Debug.LogError("Explosion FX Animator is missing.");
        }

        Debug.Log("[GrenadeExplosion] Clearing grenade.");
        player?.ClearGrenade(); // 清空玩家手榴弹数据
        explosion = false;

        // 为防止后续复用错误，禁用碰撞器
        _circleCollider2D.enabled = false;

        rb.bodyType = RigidbodyType2D.Dynamic; // 恢复刚体设置
        transform.parent = null; // 取消手榴弹对象的父对象
        // 重设摄像机目标回玩家
        // CameraManager.instance.newCamera.FollowPlayer(CameraManager.instance.newCamera.grenadeCamera);
        return false;
    }

    // 动画销毁协程
    private IEnumerator DestroyExplosionEffect(GameObject explodableFx, Animator grenadeAnim)
    {
        Debug.Log("[DestroyExplosionEffect] Waiting for explosion animation to finish...");
    
        
        yield return new WaitUntil(() => grenadeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Debug.Log("Explosion animation complete. Destroying grenade.");
        Debug.Log($"Explosion animation state: {grenadeAnim.GetCurrentAnimatorStateInfo(0).IsName("GrenadeExplodeFx")}"); 
        Debug.Log($"Normalized time: {grenadeAnim.GetCurrentAnimatorStateInfo(0).normalizedTime}");
        // 销毁爆炸特效父级
        Destroy(explodableFx,1f); 
        // 销毁手榴弹主体
        Destroy(gameObject);
    }
}
