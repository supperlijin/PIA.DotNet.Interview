using System;using System.Collections.Generic;using System.Linq;using System.Threading.Tasks;using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Mvc.RazorPages;using Microsoft.Extensions.Logging;namespace PIA.DotNet.Interview.WebUI.Pages{    public class chartModel : PageModel    {        public int Completed = 0;        public int incomplete = 0;        private readonly ILogger<chartModel> _logger;        public chartModel(ILogger<chartModel> logger)        {            _logger = logger;        }        public void OnGet()        {/*                    public int Completed = 0;        public int incomplete = 0;*/        }    }}