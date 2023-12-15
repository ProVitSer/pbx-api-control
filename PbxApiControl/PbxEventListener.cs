using System;
using System.Collections.Generic;
using TCX.Configuration;

public static class PbxEventListener
{
    private static object lockH = new object();

    public static void Start()
    {
        PhoneSystem.Root.Updated += new NotificationEventHandler(ActiveConnectionsHandler);
        PhoneSystem.Root.Inserted += new NotificationEventHandler(ActiveConnectionsHandler);
        PhoneSystem.Root.Deleted += new NotificationEventHandler(ActiveConnectionsHandler);
    }


    static void ActiveConnectionsHandler(object sender, NotificationEventArgs ev)
    {
        lock (PbxEventListener.lockH)
        {

            Dictionary<uint, List<ActiveConnection>> connectionsByCallId = PhoneSystem.Root.GetActiveConnectionsByCallID();
            foreach (KeyValuePair<uint, List<ActiveConnection>> keyValuePair in connectionsByCallId)
            {
                Console.WriteLine(keyValuePair.Key);

                if (keyValuePair.Value == null)
                {
                    return;
                }
                else
                {
                    foreach (ActiveConnection call in keyValuePair.Value)
                    {

                        Console.WriteLine(call.Status);
                        Console.WriteLine(call.CallID);
                        Console.WriteLine(call.CallConnectionID);
                        Console.WriteLine(call);

                    }

                }

            }
        }
    }


}

