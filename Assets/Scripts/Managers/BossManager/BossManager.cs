using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public Vector3 battlePosition = new Vector3(75f, 7.6f, 0); 
    public float retreatX = 110f; 

    [Header("Idle Settings")]
    public float minIdleDuration = 5f;
    public float maxIdleDuration = 10f;
    public float idleYHigh = 18f;
    public float idleYLow = -18f;

    [Header("Attack Settings")]
    public Animator anim;
    public int totalAttacks = 3; 
    public float timeBetweenAttacks = 10f; 
    public float minAttackY = -20f;
    public float maxAttackY = 20f;
    public float minYDistanceBetweenAttacks = 5f;

    private bool isRetreating = false;
    private bool isFrozen = false;

    void Start()
    {

        StartCoroutine(BossRoutine());
    }

    IEnumerator BossRoutine()
    {
        if (ShouldFreezeBecauseGameStopped())
        {
            FreezeBossForGameOver();
            yield break;
        }

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

        transform.position = battlePosition;

        yield return new WaitForSeconds(1f); 

        float previousY = transform.position.y;
        for (int i = 0; i < totalAttacks; i++)
        {
            if (ShouldFreezeBecauseGameStopped())
            {
                FreezeBossForGameOver();
                yield break;
            }

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

            float randomY = GetDiverseAttackY(previousY);
            Vector3 targetYPos = new Vector3(transform.position.x, randomY, transform.position.z);

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

            anim.SetTrigger("isFiring");
            yield return new WaitForSeconds(2.5f); 

            yield return new WaitForSeconds(timeBetweenAttacks);
        }

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

            transform.position += Vector3.right * moveSpeed * 2f * Time.deltaTime;

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