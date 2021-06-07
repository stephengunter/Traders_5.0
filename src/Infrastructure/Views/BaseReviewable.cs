using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Views
{
    public abstract class BaseReviewableViewModel : BaseRecordViewModel
    {
        public bool Reviewed { get; set; }
    }
}
