using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class MissileDestructionPooler : Pooler<Destruction>
    {
        public override void ReturnObject(Destruction objectToReturn)
        {
            objectToReturn.transform.localScale = objectToReturn.StartScale;
            base.ReturnObject(objectToReturn);
        }
    }
}
