using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Task6_Pet : MonoBehaviour
{
    public float distanceThreshold = 2.5f;
    public float scorePenaltyPerSecond = 100f;
    public GameObject player;
    private Task6_PlayerScore player_score;
    private SpriteRenderer renderer;

    void Start()
    {
        if (player == null) {
            Debug.LogError("Pet: no player!");
        }
        player_score = player.GetComponent<Task6_PlayerScore>();
        if (player_score == null) {
            Debug.LogError("Pet: no score!");
        }
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update() 
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= distanceThreshold) {
            player_score.score -= scorePenaltyPerSecond * Time.deltaTime;
            renderer.color = Color.red;
        } else {
            renderer.color = Color.white;
        }
    }
}
