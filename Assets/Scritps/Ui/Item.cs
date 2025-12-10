using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public int itemType;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerItem>().PickItem(itemType);
            Destroy(gameObject);
        }
    }
}
