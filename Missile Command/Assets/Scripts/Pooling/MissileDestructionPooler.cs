using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MissileCommand
{
    public class MissileDestructionPooler : Pooler<MissileDestruction>
    {
        public override void ReturnObject(MissileDestruction objectToReturn)
        {
            objectToReturn.transform.localScale = objectToReturn.StartScale;
            base.ReturnObject(objectToReturn);
        }
    }
}
