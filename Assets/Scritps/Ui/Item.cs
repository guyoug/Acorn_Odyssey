using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public int itemType;

    [Header("Move Settings")]
    public float moveSpeed = 1.5f; 
    private void Start()
    {
        string name = gameObject.name;
        if (name.Contains("elecitem"))
            itemType = 1;
        else if (name.Contains("fireitem"))
            itemType = 2;
        else if (name.Contains("wateritem"))
            itemType = 3;
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.pickPowerupSFX);
            collision.GetComponent<PlayerItem>().PickItem(itemType);
            Destroy(gameObject);
        }
    }
}
