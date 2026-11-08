using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid
{

    private float playerInteractionDistance = 0;
    private bool avoidPlayer = false;

    public int teamID;

    float moveSpeed = 10f;
    float turnSpeed = .1f;

    public Vector3 direction;
    public Vector3 position;

    public float collisionAvoidanceDistance = 5f;
    public float maxEffectorDistance = 5f;

    int tilePositionX = 0;
    int tilePositionZ = 0;

    public Boid(int teamID)
    {
        this.teamID = teamID;
        direction.x = Random.Range(0, 1f);
        direction.z = Random.Range(0, 1f);

        playerInteractionDistance = Random.Range(3, 10f);
        avoidPlayer = Random.value < 0.5f;
    }

    public void UpdateFollowTeamBehavior(List<Boid> boids, int followedTeamID, int boidID, bool[,] tileMap)
    {

        Vector3 resultingDirection = Vector3.zero;
        Vector3 resultingDirectionFollowedTeam = Vector3.zero;
        Vector3 directTargetDirection = Vector3.zero;
        bool gotDirectTarget = false;
        int effectorCount = 0;
        int effectorCountFollowedTeam = 0;

        float moveSpeedMul = 1.25f;
        float closeTargetDistance = 5000;

        for (int i = 0; i < boids.Count; i++)
        {
            if (i == boidID)
                continue;

            float distance = Vector3.Distance(position, boids[i].position);

            if (boids[i].teamID == followedTeamID)
            {
                if (distance < closeTargetDistance && distance < 10f)
                {
                    gotDirectTarget = true;
                    closeTargetDistance = distance;
                    directTargetDirection = (boids[i].position - position).normalized / Vector3.Distance(position, boids[i].position) * 4f;
                }
                if (gotDirectTarget)
                    continue;

                resultingDirection += (boids[i].position - position).normalized / Vector3.Distance(position, boids[i].position);
                effectorCountFollowedTeam++;
                continue;
            }

            if (distance > maxEffectorDistance)
                continue;

            if (distance < collisionAvoidanceDistance)
            {
                if (distance < 3f)
                {
                    resultingDirection += (position - boids[i].position).normalized * 3f;
                    //moveSpeedMul = 0.25f;
                }
                else
                    resultingDirection += (position - boids[i].position).normalized;

            }
            else    //booth?
            {
                resultingDirection.x += boids[i].direction.x * .015f;
                resultingDirection.z += boids[i].direction.z * .015f;
            }

            effectorCount++;
        }

        if (moveSpeedMul == 1.25f)
        {
            //if (effectorCount > 10)
             //   moveSpeedMul = 0.75f;
            //if (effectorCount > 15)
              //  moveSpeedMul = 0.5f;
        }

        if (gotDirectTarget)
            resultingDirection = (resultingDirection / effectorCount + directTargetDirection).normalized;
        else
            resultingDirection = (resultingDirection / effectorCount + resultingDirectionFollowedTeam / effectorCountFollowedTeam).normalized;

        float distanceToCenter = Vector3.Distance(Vector3.zero, position);
        if (distanceToCenter >= 20)
        {
            float t = (20 - distanceToCenter) / 50;
            resultingDirection = (resultingDirection + -position.normalized * (t * t)).normalized;
        }

        direction = new Vector3(Mathf.Lerp(direction.x, resultingDirection.x, turnSpeed * Time.deltaTime * 1.25f), 0, Mathf.Lerp(direction.z, resultingDirection.z, turnSpeed * Time.deltaTime * 1.25f));


        Vector3 newPosition = position + direction * moveSpeed * Time.deltaTime * moveSpeedMul;
        int newTilePosX = Mathf.CeilToInt(newPosition.x) + 100;
        int newTilePosZ = Mathf.CeilToInt(newPosition.z) + 100;
        if (newTilePosX != tilePositionX || newTilePosZ != tilePositionZ)
        {
            if (tileMap[newTilePosX, newTilePosZ] == true)
                return;

            tileMap[newTilePosX, newTilePosZ] = true;
            tileMap[tilePositionX, tilePositionZ] = false;

            tilePositionX = newTilePosX;
            tilePositionZ = newTilePosZ;
        }
        position = newPosition;
    }

    public void UpdateAvoidTeamBehavior(List<Boid> boids, int avoidedTeamID, int boidID, bool[,] tileMap)
    {

        Vector3 resultingDirection = Vector3.zero;
        Vector3 resultingDirectionFollowedTeam = Vector3.zero;

        int effectorCount = 0;
        int effectorCountAvoidedTeam = 0;
        float moveSpeedMul = 1f;

        for (int i = 0; i < boids.Count; i++)
        {
            if (i == boidID)
                continue;

            float distance = Vector3.Distance(position, boids[i].position);

            if (boids[i].teamID == avoidedTeamID)
            {
                resultingDirection -= (boids[i].position - position).normalized;
                effectorCountAvoidedTeam++;
                if (distance < .5f)
                    moveSpeedMul = .4f;

                continue;
            }

            if (distance > maxEffectorDistance)
                continue;

            if (distance < collisionAvoidanceDistance)
            {
                if (distance < 3f)
                {
                    resultingDirection += (position - boids[i].position).normalized * 3f;
                    moveSpeedMul = 0.5f;
                }
                else
                    resultingDirection += (position - boids[i].position).normalized;
            }
            else    //booth?
            {
                resultingDirection.x += boids[i].direction.x * .1f;
                resultingDirection.z += boids[i].direction.z * .1f;
            }

            effectorCount++;
        }


        resultingDirection = (resultingDirection / effectorCount + resultingDirectionFollowedTeam / effectorCountAvoidedTeam).normalized;

        float distanceToCenter = Vector3.Distance(Vector3.zero, position);//Player

        if (distanceToCenter >= 20)
        {
            float t = (20 - distanceToCenter) / 50;
            resultingDirection = (resultingDirection + -position.normalized * (t * t)).normalized;
        }

        direction = new Vector3(Mathf.Lerp(direction.x, resultingDirection.x, turnSpeed * Time.deltaTime), 0, Mathf.Lerp(direction.z, resultingDirection.z, turnSpeed * Time.deltaTime));

        position += direction * moveSpeed * Time.deltaTime * moveSpeedMul;
    }


    // Update is called once per frame
    public void UpdateFollowTeamBehavior(List<Boid> boids, int followedTeamID, int boidID)
    {

        Vector3 resultingDirection = Vector3.zero;
        Vector3 resultingDirectionFollowedTeam = Vector3.zero;
        Vector3 directTargetDirection = Vector3.zero;
        bool gotDirectTarget = false;
        int effectorCount = 0;
        int effectorCountFollowedTeam = 0;

        float moveSpeedMul = 1.25f;
        float closeTargetDistance = 5000;

        for (int i = 0; i < boids.Count; i++)
        {
            if (i == boidID)
                continue;

            float distance = Vector3.Distance(position, boids[i].position);

            if (boids[i].teamID == followedTeamID)
            {
                if (distance < closeTargetDistance && distance < 10f)
                {
                    gotDirectTarget = true;
                    closeTargetDistance = distance;
                    directTargetDirection = (boids[i].position - position).normalized / Vector3.Distance(position, boids[i].position) * 4f;
                }
                if (gotDirectTarget)
                    continue;

                resultingDirection += (boids[i].position - position).normalized / Vector3.Distance(position, boids[i].position);
                effectorCountFollowedTeam++;
                continue;
            }

            if (distance > maxEffectorDistance)
                continue;

            if (distance < collisionAvoidanceDistance)
            {
                if (distance < 3f)
                {
                    resultingDirection += (position - boids[i].position).normalized * 3f;
                    moveSpeedMul = 0.25f;
                }
                else
                    resultingDirection += (position - boids[i].position).normalized;

            }
            else    //booth?
            {
                resultingDirection.x += boids[i].direction.x * .015f;
                resultingDirection.z += boids[i].direction.z * .015f;
            }

            effectorCount++;
        }

        if (moveSpeedMul == 1.25f)
        {
            if (effectorCount > 10)
                moveSpeedMul = 0.75f;
            if (effectorCount > 15)
                moveSpeedMul = 0.5f;
        }

        if (gotDirectTarget)
            resultingDirection = (resultingDirection / effectorCount + directTargetDirection).normalized;
        else
            resultingDirection = (resultingDirection / effectorCount + resultingDirectionFollowedTeam / effectorCountFollowedTeam).normalized;

        float distanceToCenter = Vector3.Distance(Vector3.zero, position);
        if (distanceToCenter >= 20)
        {
            float t = (20 - distanceToCenter) / 50;
            resultingDirection = (resultingDirection + -position.normalized * (t * t)).normalized;
        }

        direction = new Vector3(Mathf.Lerp(direction.x, resultingDirection.x, turnSpeed * Time.deltaTime * 1.25f), 0, Mathf.Lerp(direction.z, resultingDirection.z, turnSpeed * Time.deltaTime * 1.25f));



        position += direction * moveSpeed * Time.deltaTime * moveSpeedMul;
    }

    public void UpdateAvoidTeamBehavior(List<Boid> boids, int avoidedTeamID, int boidID)
    {

        Vector3 resultingDirection = Vector3.zero;
        Vector3 resultingDirectionFollowedTeam = Vector3.zero;

        int effectorCount = 0;
        int effectorCountAvoidedTeam = 0;
        float moveSpeedMul = 1f;

        for (int i = 0; i < boids.Count; i++)
        {
            if (i == boidID)
                continue;

            float distance = Vector3.Distance(position, boids[i].position);

            if (boids[i].teamID == avoidedTeamID)
            {
                resultingDirection -= (boids[i].position - position).normalized;
                effectorCountAvoidedTeam++;
                if (distance < .5f)
                    moveSpeedMul = .4f;

                continue;
            }

            if (distance > maxEffectorDistance)
                continue;

            if (distance < collisionAvoidanceDistance)
            {
                if (distance < 3f)
                {
                    resultingDirection += (position - boids[i].position).normalized * 3f;
                    moveSpeedMul = 0.5f;
                }
                else
                    resultingDirection += (position - boids[i].position).normalized;///////////
            }
            else    //booth?
            {
                resultingDirection.x += boids[i].direction.x * .1f;
                resultingDirection.z += boids[i].direction.z * .1f;
            }

            effectorCount++;
        }



        resultingDirection = (resultingDirection / effectorCount + resultingDirectionFollowedTeam / effectorCountAvoidedTeam).normalized;



        float distanceToCenter = Vector3.Distance(Vector3.zero, position);////////

        if (distanceToCenter >= 20)
        {
            float t = (20 - distanceToCenter) / 50;
            resultingDirection = (resultingDirection + -position.normalized * (t * t)).normalized;
        }
        direction = new Vector3(Mathf.Lerp(direction.x, resultingDirection.x, turnSpeed * Time.deltaTime), 0, Mathf.Lerp(direction.z, resultingDirection.z, turnSpeed * Time.deltaTime));

        position += direction * moveSpeed * Time.deltaTime * moveSpeedMul;
    }
















    public void UpdateFollowTeamBehavior(List<Boid> boids, int followedTeamID, int boidID, Transform playerTransform)
    {

        Vector3 resultingDirection = Vector3.zero;
        Vector3 resultingDirectionFollowedTeam = Vector3.zero;
        Vector3 directTargetDirection = Vector3.zero;
        bool gotDirectTarget = false;
        int effectorCount = 0;
        int effectorCountFollowedTeam = 0;

        float moveSpeedMul = 1.25f;
        float closeTargetDistance = 5000;

        for (int i = 0; i < boids.Count; i++)
        {
            if (i == boidID)
                continue;

            float distance = Vector3.Distance(position, boids[i].position);

            if (boids[i].teamID == followedTeamID)
            {
                if (distance < closeTargetDistance && distance < 10f)
                {
                    gotDirectTarget = true;
                    closeTargetDistance = distance;
                    directTargetDirection = (boids[i].position - position).normalized / Vector3.Distance(position, boids[i].position) * 4f;
                }
                if (gotDirectTarget)
                    continue;

                resultingDirection += (boids[i].position - position).normalized / Vector3.Distance(position, boids[i].position);
                effectorCountFollowedTeam++;
                continue;
            }

            if (distance > maxEffectorDistance)
                continue;

            if (distance < collisionAvoidanceDistance)
            {
                if (distance < 3f)
                {
                    resultingDirection += (position - boids[i].position).normalized * 3f;
                    moveSpeedMul = 0.25f;
                }
                else
                    resultingDirection += (position - boids[i].position).normalized;

            }
            else    //booth?
            {
                resultingDirection.x += boids[i].direction.x * .015f;
                resultingDirection.z += boids[i].direction.z * .015f;
            }

            effectorCount++;
        }

        if (moveSpeedMul == 1.25f)
        {
            if (effectorCount > 10)
                moveSpeedMul = 0.75f;
            if (effectorCount > 15)
                moveSpeedMul = 0.5f;
        }

        resultingDirection += InteractWithPlayer(position, playerTransform);


        if (gotDirectTarget)
            resultingDirection = (resultingDirection / effectorCount + directTargetDirection).normalized;
        else
            resultingDirection = (resultingDirection / effectorCount + resultingDirectionFollowedTeam / effectorCountFollowedTeam).normalized;

        float distanceToCenter = Vector3.Distance(Vector3.zero, position);
        if (distanceToCenter >= 20)
        {
            float t = (20 - distanceToCenter) / 50;
            resultingDirection = (resultingDirection + -position.normalized * (t * t)).normalized;
        }

        direction = new Vector3(Mathf.Lerp(direction.x, resultingDirection.x, turnSpeed * Time.deltaTime * 1.25f), 0, Mathf.Lerp(direction.z, resultingDirection.z, turnSpeed * Time.deltaTime * 1.25f));



        position += direction * moveSpeed * Time.deltaTime * moveSpeedMul;
    }

    public void UpdateAvoidTeamBehavior(List<Boid> boids, int avoidedTeamID, int boidID, Transform playerTransform)
    {

        Vector3 resultingDirection = Vector3.zero;
        Vector3 resultingDirectionFollowedTeam = Vector3.zero;

        int effectorCount = 0;
        int effectorCountAvoidedTeam = 0;
        float moveSpeedMul = 1f;

        for (int i = 0; i < boids.Count; i++)
        {
            if (i == boidID)
                continue;

            float distance = Vector3.Distance(position, boids[i].position);

            if (boids[i].teamID == avoidedTeamID)
            {
                resultingDirection -= (boids[i].position - position).normalized;
                effectorCountAvoidedTeam++;
                if (distance < .5f)
                    moveSpeedMul = .4f;

                continue;
            }

            if (distance > maxEffectorDistance)
                continue;

            if (distance < collisionAvoidanceDistance)
            {
                if (distance < 3f)
                {
                    resultingDirection += (position - boids[i].position).normalized * 3f;
                    moveSpeedMul = 0.5f;
                }
                else
                    resultingDirection += (position - boids[i].position).normalized;///////////
            }
            else    //booth?
            {
                resultingDirection.x += boids[i].direction.x * .1f;
                resultingDirection.z += boids[i].direction.z * .1f;
            }

            effectorCount++;
        }

        resultingDirection += InteractWithPlayer(position, playerTransform);

        resultingDirection = (resultingDirection / effectorCount + resultingDirectionFollowedTeam / effectorCountAvoidedTeam).normalized;



        float distanceToCenter = Vector3.Distance(Vector3.zero, position);////////

        if (distanceToCenter >= 20)
        {
            float t = (20 - distanceToCenter) / 50;
            resultingDirection = (resultingDirection + -position.normalized * (t * t)).normalized;
        }
        direction = new Vector3(Mathf.Lerp(direction.x, resultingDirection.x, turnSpeed * Time.deltaTime), 0, Mathf.Lerp(direction.z, resultingDirection.z, turnSpeed * Time.deltaTime));

        position += direction * moveSpeed * Time.deltaTime * moveSpeedMul;
    }

    public Vector3 InteractWithPlayer(Vector3 position, Transform playerTransform)
    {


        float distance = Vector3.Distance(position, playerTransform.position);
        Vector3 dir = (position - playerTransform.position).normalized;

        if (distannce < playerInteractionDistance)
        {
            if (avoidPlayer)
                return dir *= (distance / playerInteractionDistance) * .2f;
            else
                return dir *= (distance / playerInteractionDistance) * -.2f;

        }

        return Vector3.zero;

    }

}
