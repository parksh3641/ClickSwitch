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
            // 인터넷 연결이 안되었을때
            isConnect = false;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            // 데이터로 인터넷 연결이 되었을때
            isConnect = true;
        }
        else
        {
            // 와이파이로 연결이 되었을때
            isConnect = true;
        }
        return isConnect;
    }
}