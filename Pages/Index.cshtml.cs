using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ResidentMail.Models;
using ResidentMail.Services;

namespace ResidentMail.Pages
{
    public class IndexModel : PageModel
    {
        public string PageTitle = "ResidentMail";

        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession _session;

        // Bind variable with properties of the Contact model class.
        [BindProperty]
        public Contact Contact { get; set; }

        public Equation Equation { get; set; }

        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public void OnGet()
        {
            if (_session.GetInt32("FirstNumber") == null || _session.GetInt32("SecondNumber") == null)
            {
                FormSpamEquationService addition = new FormSpamEquationService();

                Equation = addition.GetEquationNumbers();

                _session.SetString("FirstNumber", Equation.FirstNumber.ToString());
                _session.SetString("SecondNumber", Equation.SecondNumber.ToString());
                _session.SetString("SessionResult", Equation.Sum.ToString());
            }

            FirstNumber = Equation.FirstNumber;
            SecondNumber = Equation.SecondNumber;
        }

        public void OnPost()
        {

        }
    }
}