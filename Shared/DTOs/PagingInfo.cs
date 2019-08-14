using System;
using System.Collections.Generic;
using System.Text;

namespace AwqafBlazor.Shared.DTOs
{
    public abstract class PagingInfo
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
