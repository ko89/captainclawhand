using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondidition : MonoBehaviour
{
    public AudioSource WinFanfare;
    public EndScreen EndScreen;

    private void OnCollisionEnter(Collision collision)
    {
        WinFanfare.Play();
        EndScreen.ShowSuccess();
        //Debug.Log($"<color=\"yellow\">[OnCollisionEnter]</color> name: {name} impulse: {collision.impulse}");
    }
}
