using UnityEngine;
using UnityEngine.UI;

public class CollectibleCounter : MonoBehaviour
{
    public Text collectibleCountText;
    public static int objectsCollected = 0;

    void Start()
    {
        collectibleCountText.text = "Collectibles: " + objectsCollected.ToString();
    }

    public static void IncrementCollectibles()
    {
        objectsCollected++;
    }

    void Update()
    {
        collectibleCountText.text = "Collectibles: " + objectsCollected.ToString();
    }
}

