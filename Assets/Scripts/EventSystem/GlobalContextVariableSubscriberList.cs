using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is basically just a simple way to subscribe scripts via inspector to an event.
 * Whenever you have a script implementing IGlobalContextSubscriber, just add the corresponding gameobject to the list of this script via inspector.
 * 
 * See GlobalContextVariable for more information.
 */
public class GlobalContextVariableSubscriberList : MonoBehaviour
{

    [SerializeField]
    private GameObject[] subscribers;


    private void Awake()
    {
        foreach (var subscriber in subscribers)
            foreach(var subscriberInterface in subscriber.GetComponents<IGlobalContextSubscriber>())
                GlobalContextVariable.subscribe(subscriberInterface);

        // initialize scene by updating each object with main_view context (applys automatically all settings defined in settings script)
        GlobalContextVariable.updateValue(GlobalContextVariable.GlobalContextVariableValue.main_view, forceUpdate: true);
    }
}
