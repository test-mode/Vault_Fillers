using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


public class QueueManager : MonoBehaviour
{
    // Settings

    // Connections
    List<GameObject> queue = new List<GameObject>();
    List<GameObject> queuePositions = new List<GameObject>();
    [HideInInspector] public GameObject stickmanDestination;

    [Header("Connections")]
    public GameObject vault;
    public GameObject moneyCountersParent;
    public GameSettings playerSettings;


    // State Variables

    //int

    //float

    //bool


    void Start()
    {
        InitConnections();
        InitState();
    }

    void InitConnections()
    {
        
    }
    void InitState()
    {
        SetQueuePositions();
    }

    void SetQueuePositions()
    {
        GameObject queuePosParent = new GameObject("Queue Position Parent");

        Vector3 vaultPos = vault.transform.position;
        Vector3 cubeScale = Vector3.one * .1f;

        GameObject vaultFrontCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 vaultFrontPos = new Vector3(vaultPos.x, vaultPos.y, vaultPos.z - 4);
        vaultFrontCube.transform.position = vaultFrontPos;
        vaultFrontCube.transform.tag = "DepositPosition";
        vaultFrontCube.transform.localScale = cubeScale;
        vaultFrontCube.transform.SetParent(queuePosParent.transform);
        vaultFrontCube.AddComponent<Rigidbody>();
        vaultFrontCube.GetComponent<Rigidbody>().isKinematic = true;
        vaultFrontCube.GetComponent<MeshRenderer>().enabled = false;
        vaultFrontCube.GetComponent<BoxCollider>().enabled = true;
        vaultFrontCube.GetComponent<BoxCollider>().isTrigger = true;
        vaultFrontCube.GetComponent<BoxCollider>().size = Vector3.one * 40;
        queuePositions.Add(vaultFrontCube);

        for (int i = 1; i < 13; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = cubeScale;
            cube.GetComponent<MeshRenderer>().enabled = false;
            cube.GetComponent<BoxCollider>().enabled = false;
            
            Vector3 cubePos = new Vector3(vaultPos.x + 4 + i * 1.3f, vaultPos.y, vaultPos.z - 4);
            cube.transform.position = cubePos;

            cube.transform.SetParent(queuePosParent.transform);
            queuePositions.Add(cube);
        }
    }



    void Update()
    {
        ManageQueue();
    }


    void ManageQueue()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            queue[i].GetComponentInChildren<JoinQueueState>().positionInQueue = queuePositions[i].transform.position;

            //if (i == 0 && playerSettings.stickmans[0].GetComponent<StickmanManager>().ReachedDestination())
            //{
            //    if (Vector3.Distance(playerSettings.stickmans[i].transform.position, queuePositions[i].transform.position) < .1f)
            //    {
                    
            //    }
            //    StickmanEvents.OnReachedDepositPosition?.Invoke(queue[i]);
            //}
        }
    }

    public void AddToQueue(GameObject stickman)
    {
        queue.Add(stickman);
    }

    public void RemoveFromQueue(GameObject stickman)
    {
        queue.Remove(stickman);
    }
}