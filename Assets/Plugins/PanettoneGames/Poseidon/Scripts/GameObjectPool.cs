using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameObject Pool", menuName = "Poseidon/GameObject Pool")]
public class GameObjectPool : ScriptableObject
{
    [SerializeField] GameObject prefab;
    [SerializeField] int prewarmCount = 20;

    private Queue<GameObject> objects = new Queue<GameObject>();
    public void PreWarm() => AddObjects(prewarmCount);
    public GameObject Get()
    {
        if (objects.Count == 0)
            AddObjects(1);

        return objects.Dequeue();
    }

    private void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var newObject = Instantiate(prefab);

            newObject.SetActive(false);
            objects.Enqueue(newObject);
            newObject.GetComponent<IGameObjectPooled>().Pool = this;
        }
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        objects.Enqueue(objectToReturn);
    }

    public async void ReturnToPool(GameObject objectToReturn, float delay = 0)
    {
        if (objectToReturn == null) return;
        
        if (delay > 0)
        {
            //Disable Visuals
            var rends = objectToReturn.GetComponents<Renderer>().ToList();
            rends.ForEach(r => r.enabled = false);

            //Induce some delay
            var endTime = Time.time + delay;
            while (Time.time < endTime)
            {
                await Task.Yield();
            }
        }
            objectToReturn.SetActive(false);
            objects.Enqueue(objectToReturn);
    }
}

public interface IGameObjectPooled
{
    GameObjectPool Pool { get; set; }
}