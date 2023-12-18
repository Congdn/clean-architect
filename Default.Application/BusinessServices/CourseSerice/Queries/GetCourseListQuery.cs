using Default.Application.BusinessServices.CourseSerice.dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.CourseSerice.Queries
{
    public class GetCourseListQuery : IRequest<List<CourseDetailViewModel>>
    {
        public int? itemPerPage { get; set; }

        public int? pageSize { get; set; }  

        public Guid? afterItem { get; set; }

        public String query { get; set; }
        public string orderBy { get; set; } 
        public string categories { get; set; }

        public GetCourseListQuery(int? itemPerPage, int? pageSize, Guid? afterItem, string query, string orderBy, string categories)
        {
            this.itemPerPage = itemPerPage;
            this.pageSize = pageSize;
            this.afterItem = afterItem;
            this.query = query;
            this.orderBy = orderBy;
            this.categories = categories;
        }
    }
}
