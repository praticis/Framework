
namespace Praticis.Framework.Layers.Data.Abstractions.Filters
{
    public class BasePaginationFilter : BaseFilter
    {
        #region Properties

        /// <summary>
        /// The page number of pagination filter. 
        /// Starts with value 1 by default.
        /// </summary>
        private int _pageNumber;
        public int PageNumber
        {
            get
            {
                if (this._pageNumber <= 0)
                    return this._pageNumber = 1;

                return this._pageNumber;
            }
            set => this._pageNumber = value;
        }

        /// <summary>
        /// The count items that will be returned of filter.
        /// Starts with value 1 by default.
        /// </summary>
        private int _pageSize;
        public int PageSize
        {
            get
            {
                if (this._pageSize <= 0)
                    this._pageSize = 1;

                return this._pageSize;
            }
            set => this._pageSize = value;
        }

        /// <summary>
        /// The count items need to skip to get items of <see cref="PageNumber"/>.
        /// </summary>
        public int SkipLength => (this.PageNumber - 1) * this.PageSize;

        #endregion

        public BasePaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 20;
        }
    }
}