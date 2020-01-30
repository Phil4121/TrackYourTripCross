namespace TrackYourTrip.Core.ViewModelResults
{
    public class OperationResult<TEntity>
    {

        public OperationResult(TEntity entity, bool isSaved = false, bool isDeleted = false, bool isCanceld = false)
        {
            Entity = entity;
            IsSaved = isSaved;
            IsDeleted = isDeleted;
            IsCanceld = isCanceld;
        }
        public TEntity Entity { get; set; }
        public bool IsSaved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCanceld { get; set; }
    }
}
