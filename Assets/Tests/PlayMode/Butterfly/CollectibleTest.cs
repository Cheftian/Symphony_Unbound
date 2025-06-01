using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;

public class CollectibleTest
{
    private GameObject playerObject;
    private bool sceneLoaded = false;
    private Collectible collectible;
    private Collider2D collectibleCollider;

    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("GameScene");
        yield return new WaitUntil(() => sceneLoaded);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoaded = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [UnityTest]
    public IEnumerator Collectible_ExistsInScene()
    {
        collectible = Object.FindObjectOfType<Collectible>();
        Assert.IsNotNull(collectible, "Collectible tidak ditemukan di scene.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Collectible_IsCollected_WhenPlayerCollides()
    {
        SetupPlayerNearCollectible();
        yield return new WaitForFixedUpdate();
        Physics2D.Simulate(Time.fixedDeltaTime);

        Assert.IsTrue(IsCollected(collectible), "Collectible seharusnya dikoleksi setelah bertabrakan dengan player.");
    }

    [UnityTest]
    public IEnumerator Collectible_DisablesCollider_WhenCollected()
    {
        SetupPlayerNearCollectible();
        yield return new WaitForFixedUpdate();
        Physics2D.Simulate(Time.fixedDeltaTime);

        collectibleCollider = collectible.GetComponent<Collider2D>();
        Assert.IsFalse(collectibleCollider.enabled, "Collider collectible harus dinonaktifkan setelah dikoleksi.");
    }

    [UnityTest]
    public IEnumerator Collectible_IsAddedToStaticList_WhenCollected()
    {
        SetupPlayerNearCollectible();
        yield return new WaitForFixedUpdate();
        Physics2D.Simulate(Time.fixedDeltaTime);

        var field = typeof(Collectible).GetField("collectedObjects", BindingFlags.NonPublic | BindingFlags.Static);
        var list = field.GetValue(null) as System.Collections.IList;
        Assert.Contains(collectible, list, "Collectible tidak ditemukan dalam daftar static setelah dikoleksi.");
    }

    private void SetupPlayerNearCollectible()
    {
        collectible = Object.FindObjectOfType<Collectible>();
        Assert.IsNotNull(collectible, "Collectible tidak ditemukan di scene.");

        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.transform.position = collectible.transform.position;
        playerObject.AddComponent<CircleCollider2D>();

        if (collectible.GetComponent<Collider2D>() == null)
        {
            collectibleCollider = collectible.gameObject.AddComponent<CircleCollider2D>();
        }
    }

    private bool IsCollected(Collectible c)
    {
        var field = typeof(Collectible).GetField("isCollected", BindingFlags.NonPublic | BindingFlags.Instance);
        return (bool)field.GetValue(c);
    }

    [UnityTearDown]
    public IEnumerator Cleanup()
    {
        if (playerObject != null)
            Object.Destroy(playerObject);

        yield return null;
    }
}
