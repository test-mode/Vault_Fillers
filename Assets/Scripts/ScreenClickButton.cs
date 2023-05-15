using UnityEngine;


public class ScreenClickButton : MonoBehaviour
{
    // Settings
    
    // Connections
    
    // State Variables
    

    void Start()
    {
        //InitConnections();
        //InitState();
    }

    void InitConnections()
    {

    }
    void InitState()
    {

    }

    void Update()
    {
        
    }

    public void OnClick()
    {
        EventManager.ScreenClickedEvent();
    }
}

