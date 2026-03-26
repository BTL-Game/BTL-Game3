using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public Vector3 battlePosition = new Vector3(75f, 7.6f, 0); // Boss lane while attacking
    public float retreatX = 110f; // X position where boss leaves the scene

    [Header("Idle Settings")]
    public float minIdleDuration = 5f;
    public float maxIdleDuration = 10f;
    public float idleYHigh = 18f;
    public float idleYLow = -18f;

    [Header("Attack Settings")]
    public Animator anim;
    public int totalAttacks = 3; // Number of fire attacks before retreat
    public float timeBetweenAttacks = 15f; // Delay between attacks
    public float minAttackY = -20f;
    public float maxAttackY = 20f;
    public float minYDistanceBetweenAttacks = 5f;

    private bool isRetreating = false;
    private bool isFrozen = false;

    void Start()
    {
        // Start boss behavior sequence on spawn.
        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        if (ShouldFreezeBecauseGameStopped())
        {
            FreezeBossForGameOver();
            yield break;
        }

        // PHASE 1: Enter battle area.
        while (Vector3.Distance(transform.position, battlePosition) > 0.1f)
        {
            if (ShouldFreezeBecauseGameStopped())
            {
                FreezeBossForGameOver();
                yield break;
            }

            transform.position = Vector3.MoveTowards(transform.position, battlePosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to exact battle position before attack loop starts.
        transform.position = battlePosition;

        // Optional screen shake when boss enters.
        // Camera.main.GetComponent<CameraShake>().Shake(0.5f, 0.2f);

        yield return new WaitForSeconds(1f); // Short pause before first attack.

        // PHASE 2: Repeated attack loop.
        float previousY = transform.position.y;
        for (int i = 0; i < totalAttacks; i++)
        {
            if (ShouldFreezeBecauseGameStopped())
            {
                FreezeBossForGameOver();
                yield break;
            }

            // 1. Idle high or low for a few seconds.
            float targetIdleY = (Random.value > 0.5f) ? idleYHigh : idleYLow;
            Vector3 targetIdlePos = new Vector3(transform.position.x, targetIdleY, transform.position.z);
            
            while (Mathf.Abs(transform.position.y - targetIdleY) > 0.1f)
            {
                if (ShouldFreezeBecauseGameStopped())
                {
                    FreezeBossForGameOver();
                    yield break;
                }
                transform.position = Vector3.MoveTowards(transform.position, targetIdlePos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            
            float idleTime = Random.Range(minIdleDuration, maxIdleDuration);
            yield return new WaitForSeconds(idleTime);

            // 2. Pick a varied Y attack position.
            float randomY = GetDiverseAttackY(previousY);
            Vector3 targetYPos = new Vector3(transform.position.x, randomY, transform.position.z);

            // 3. Move smoothly to that fire position.
            while (Mathf.Abs(transform.position.y - randomY) > 0.1f)
            {
                if (ShouldFreezeBecauseGameStopped())
                {
                    FreezeBossForGameOver();
                    yield break;
                }

                transform.position = Vector3.MoveTowards(transform.position, targetYPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            previousY = transform.position.y;

            // 4. Trigger fire attack animation.
            anim.SetTrigger("isFiring");
            yield return new WaitForSeconds(2.5f); 

            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        // PHASE 3: Retreat.
        isRetreating = true;
    }

    float GetDiverseAttackY(float previousY)
    {
        float yMin = Mathf.Min(minAttackY, maxAttackY);
        float yMax = Mathf.Max(minAttackY, maxAttackY);

        float chosenY = previousY;
        const int maxTries = 8;

        for (int i = 0; i < maxTries; i++)
        {
            float candidateY = Random.Range(yMin, yMax);
            if (Mathf.Abs(candidateY - previousY) >= minYDistanceBetweenAttacks)
            {
                chosenY = candidateY;
                break;
            }

            chosenY = candidateY;
        }

        return chosenY;
    }

    void Update()
    {
        if (ShouldFreezeBecauseGameStopped())
        {
            FreezeBossForGameOver();
            return;
        }

        if (isRetreating)
        {
            // Move out to the right side.
            transform.position += Vector3.right * moveSpeed * 2f * Time.deltaTime;

            // End boss phase when fully out of scene.
            if (transform.position.x > retreatX)
            {
                EndBossPhase();
            }
        }
    }

    bool ShouldFreezeBecauseGameStopped()
    {
        return GameManager.Instance != null && !GameManager.Instance.isGameStarted;
    }

    public void FreezeBossForGameOver()
    {
        if (isFrozen)
        {
            return;
        }

        isFrozen = true;
        isRetreating = false;
        StopAllCoroutines();

        if (anim != null)
        {
            anim.speed = 0f;
        }

        enabled = false;
    }

    void EndBossPhase()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CompleteBossPhase();
        }
        
        Destroy(gameObject);
    }
}