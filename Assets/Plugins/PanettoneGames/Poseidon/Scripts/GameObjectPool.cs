using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace PanettoneGames
{
    [CreateAssetMenu(fileName = "New GameObject Pool", menuName = "Poseidon/GameObject Pool")]
    public class GameObjectPool : ScriptableObject
    {
        [SerializeField] GameObject prefab;
        [SerializeField] int prewarmCount = 20;

        private Queue<GameObject> objects = new Queue<GameObject>();
        private Transform poolContainer;
        /// <summary>
        /// Prewarms the pool with the specified prewarm Count on the GameObjectPool Scriptable Object
        /// </summary>
        /// <param name="parentTransform">[Optional] The name of parent transform of the pooled objects</param>
        public void Prewarm(string parentTransform = null)
        {
            poolContainer = parentTransform == null ? new GameObject(name).transform: new GameObject(parentTransform).transform;
            AddObjects(prewarmCount);
        }

        /// <summary>
        /// Fetches the next GameObject from the pool
        /// </summary>
        /// <returns>An instance of the pooled GameObject</returns>
        public GameObject Get()
        {
            if (objects.Count == 0)
                AddObjects(1);

            var obj = objects.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        private void AddObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newObject = Instantiate(prefab);

                newObject.SetActive(false);
                objects.Enqueue(newObject);
                newObject.GetComponent<IGameObjectPooled>().Pool = this;

                newObject.transform.parent = poolContainer;
            }
        }

        /// <summary>
        /// Returns a GameObject to the pool to be recycled
        /// </summary>
        /// <param name="objectToReturn">An instance of the pooled gameObject implementing IGameObjectPooled</param>
        /// <param name="delay">An optional delay of n milli seconds in which all all renders are disabled for any jobs to be handled asynchrounsly </param>
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

    /// <summary>
    /// Provides access to the Pool
    /// </summary>
    public interface IGameObjectPooled
    {
        /// <summary>
        /// This is your reference to the pool which you will use to return gameObjects no longer needed
        /// </summary>
        GameObjectPool Pool { get; set; }
    }
}