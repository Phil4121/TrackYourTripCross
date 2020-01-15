using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.ViewModelResults
{
    public class OperationResult<TEntity>
    {

        public OperationResult(TEntity entity, bool isSaved = false, bool isDeleted = false, bool isCanceld = false)
        {
            this.Entity = entity;
            this.IsSaved = isSaved;
            this.IsDeleted = isDeleted;
            this.IsCanceld = isCanceld;
        }
        public TEntity Entity { get; set; }
        public bool IsSaved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCanceld { get; set; }
    }
}
