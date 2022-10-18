using UnityEngine;

public class NetworkConnect : MonoBehaviour
{
    public static NetworkConnect instance;

    // public static bool isConnect = false;
    public bool isConnect = false;

    void Awake()
    {
        instance = this;
    }

    public bool CheckConnectInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isConnect = false;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            isConnect = true;
        }
        else
        {
            isConnect = true;
        }
        return isConnect;
    }
}