using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int currentPatrolPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public float waitAtPoint = 2;
    private float waitCounter;

    public float chaseRange; //Perseguir a player

    public float attackRange = 1f;
    public float timeBetweenAttacks = 2f;
    private float attackCounter; //Contador de tiempo entre ataques

    public enum AIState //Lista con los estados
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking
    };
    public AIState currentState;

    
    void Start()
    {
        waitCounter = waitAtPoint;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position); //Distancia del navmesh al jugador

        switch (currentState)
        {
            case AIState.Idle:

                animator.SetBool("IsMoving", false);

                if(waitCounter > 0) //Si esta en Idle
                {
                    waitCounter -= Time.deltaTime;
                }
                else //Sino patrulla
                {
                    currentState = AIState.Patrolling;
                    agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                }

                if(distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.Chasing;
                    animator.SetBool("IsMoving", true);
                }

                break;

            case AIState.Patrolling:

                    if(agent.remainingDistance <= 0.2f) //Con respecto a la distancia que queda
                    {
                        currentPatrolPoint++; //Para llegar al siguiente punto
                        
                        if(currentPatrolPoint >= patrolPoints.Length) //Si tenemos más puntos para patrullar
                        {
                            currentPatrolPoint = 0;
                        }

                        //agent.SetDestination(patrolPoints[currentPatrolPoint].position); //Actualiza 
                        currentState = AIState.Idle; //Cuando termina de patrullar
                        waitCounter = waitAtPoint;
                    }

                        if(distanceToPlayer <= chaseRange)
                        {
                            currentState = AIState.Chasing;
                        }

                    animator.SetBool("IsMoving", true);

                break;

            case AIState.Chasing:
                agent.SetDestination(PlayerController.instance.transform.position); //Destino del jugador, Persigue

                if(distanceToPlayer <= attackRange)
                {
                    currentState = AIState.Attacking;
                    animator.SetTrigger("Attack");
                    animator.SetBool("IsMoving", false);

                    agent.velocity = Vector3.zero;
                    agent.isStopped = true; //No se desliza cuando ataca
                }

                if(distanceToPlayer > chaseRange) //Escapa
                {
                    currentState = AIState.Idle;
                    waitCounter = waitAtPoint;

                    agent.velocity = Vector3.zero;
                    agent.SetDestination(transform.position);
                }

                break;

            case AIState.Attacking:

                transform.LookAt(PlayerController.instance.transform, Vector3.up); //Para que mire al Player
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0); //Suave

                attackCounter -= Time.deltaTime;
                if(attackCounter <= 0)
                {
                    if(distanceToPlayer < attackRange)
                    {
                        animator.SetTrigger("Attack");
                        attackCounter = timeBetweenAttacks;
                    }
                    else
                    {
                        currentState = AIState.Idle; //Sino está dentro del rango de ataque
                        waitCounter= waitAtPoint;

                        agent.isStopped = false;

                    }
                }

                break;
                
        }

    }
}
