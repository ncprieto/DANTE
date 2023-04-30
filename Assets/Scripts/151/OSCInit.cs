using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//************** use UnityOSC namespace...
using UnityOSC;
//*************

public class OSCInit : MonoBehaviour
{
    //************* Need to setup this server dictionary...
    Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
    //*************

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true; //allows unity to update when not in focus

        //************* Instantiate the OSC Handler...
        OSCHandler.Instance.Init();
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/trigger", 1);
        //OSCHandler.Instance.SendMessageToClient("pd", "/unity/playseq", 1);
        //*************
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ReceiveMessages();
    }

    void ReceiveMessages()
    {
        //************* Routine for receiving the OSC...
        OSCHandler.Instance.UpdateLogs();
        Dictionary<string, ServerLog> servers = new Dictionary<string, ServerLog>();
        servers = OSCHandler.Instance.Servers;

        foreach (KeyValuePair<string, ServerLog> item in servers)
        {
            // If we have received at least one packet,
            // show the last received from the log in the Debug console
            if (item.Value.log.Count > 0)
            {
                int lastPacketIndex = item.Value.packets.Count - 1;

                //get address and data packet
                //countText.text = item.Value.packets[lastPacketIndex].Address.ToString();
                //countText.text += item.Value.packets[lastPacketIndex].Data[0].ToString();

            }
        }
        //*************
    }

    // code to send message
    //OSCHandler.Instance.SendMessageToClient("pd", "/unity/colwall", 1);
}
