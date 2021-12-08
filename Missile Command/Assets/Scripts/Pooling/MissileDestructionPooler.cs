namespace MissileCommand.Pooling
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
