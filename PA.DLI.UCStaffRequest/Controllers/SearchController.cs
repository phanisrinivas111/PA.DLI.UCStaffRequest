using PA.DLI.UCStaffRequest.DataAccess.DataAccess;
using PA.DLI.UCStaffRequest.DataAccess.DataObjects;
using PA.DLI.UCStaffRequest.DataAccess.Models;
using PA.DLI.UCStaffRequest.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SearchRequest = PA.DLI.UCStaffRequest.ViewModels.SearchRequest;

namespace PA.DLI.UCStaffRequest.Controllers
{
    public class SearchController : Controller
    {

        private readonly InquiryDataAccess _dataAccess;

        public SearchController()
        {
            _dataAccess = new InquiryDataAccess();
        }
        public ActionResult Index()
        {
            return View();
        }
            //GET: Search
            public ActionResult Search(SearchRequest criteria, int page = 1, int pageSize = 25)
        {
            var allResults = _dataAccess.Search(MapToSearchRequest(criteria)).ToList();
            var modelUser = MapModelResult(allResults).OrderBy(u => u.TicketId).ToList();
            var pagedResults = modelUser.Skip((page - 1) * pageSize).Take(pageSize).ToList();

           var  viewModel = new SearchViewModel
            {
               searchRequest=criteria,
                Results = pagedResults,
                TotalResults = modelUser.Count,
                CurrentPage = page,
                 TotalPages  = (int)Math.Ceiling((double)modelUser.Count / pageSize)
             };

            return PartialView("_SearchListPartial", viewModel);
        }

        private IEnumerable<PA.DLI.UCStaffRequest.Models.SearchResult> MapModelResult(IEnumerable<PA.DLI.UCStaffRequest.DataAccess.Models.SearchResult> dataUserObject)
        {
            return dataUserObject.Select(u => new Models.SearchResult
            {
                FromEmail = u.FromEmail,
                SubmissionDate = u.SubmissionDate,
                Category = u.Category,
                TicketId = u.TicketId
            }).ToList();
        }
        private PA.DLI.UCStaffRequest.DataAccess.Models.SearchRequest MapToSearchRequest(SearchRequest viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel), "The RequestViewModel cannot be null.");
            }

            var searchRequest = new PA.DLI.UCStaffRequest.DataAccess.Models.SearchRequest
            {
                Category = viewModel.Category,
                TicketNumber = viewModel.TicketNumber,
                CwopaId = viewModel.CwopaId
            };

            if (!string.IsNullOrEmpty(viewModel.SubmittedDate))
            {
                searchRequest.SubmittedDate = DateTime.ParseExact(viewModel.SubmittedDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            return searchRequest;
        }
    }
}