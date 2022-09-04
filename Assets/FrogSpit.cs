using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogSpit : Projectiles
{
    
    public override void DeathBehaviour()
    {
        Destroy(gameObject);
     //   throw new System.NotImplementedException();
    }

    public override void InitializeBehaviour()
    {
     //   throw new System.NotImplementedException();
    }

    public override void MovementBehaviour()
    {

       // throw new System.NotImplementedException();
    }

}
