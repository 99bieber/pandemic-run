using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Player player;
    public Slider energySlider;
    public float energy;
    float maxEnergy = 100;
    private Animator animator;
    void Start()
    {
        player = GetComponent<Player>();
        energy = maxEnergy;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        energySlider.value = energy;
        energy -= 10f * Time.deltaTime;

        if(energy <= 0){
            player.yDeathCondition = 2f;
            player.Death();
        }
        else if(energy >= 100){
            energy = 100;
        }
    }
}
