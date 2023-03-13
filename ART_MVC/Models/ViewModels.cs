using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ART_MVC.Models
{
    public class MasterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Select Project")]
        public string ProjectId { get; set; }
        public string CandidateId { get; set; }
        //  public ProjectsBRModel ProjectsBRModel { get; set; }
        /*  public int AccountsBRModelId { get; set; }
          public AccountsBRModel AccountsBRModel { get; set; }
  */
        [Required]
        public string CandidateName { get; set; }
        public int EmployeeId { get; set; }
        // [Required]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage ="Please Select Internal/External")]
        public string Int_Ext { get; set; }
        [Required(ErrorMessage = "Please Select Location")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Please Select Source")]
        public string Source { get; set; }
        [Required(ErrorMessage = "Please Select Grade")]
        public string Grade { get; set; }
        public string SkillSetRequired { get; set; }

        public string JobDescription { get; set; }
        public int Age { get; set; }
        [Required]
        public DateTime? ScreeningDate { get; set; } 
        public string ScreeningResult { get; set; }

       // [GreaterThan(nameof(ScreeningDate), ErrorMessage = "L1_Eval_Date must be greater than Screening Date")]

        public DateTime? L1_Eval_Date { get; set; } 
        public string L1_Eval_Result { get; set; }

      //  [GreaterThan(nameof(L1_Eval_Date), ErrorMessage = "Client_Eval_Date must be greater than L1_Eval_Date")]

        public DateTime? Client_Eval_Date { get; set; } 
        public string Client_Eval_Result { get; set; }
      //  [GreaterThan(nameof(Client_Eval_Date), ErrorMessage = "Manager_Eval_Date must be greater than Client_Eval_Date")]

        public DateTime? Manager_Eval_Date { get; set; } 
        public string Manager_Eval_Result { get; set; }

        public string Status { get; set; }
        public string Eval_Comments { get; set; }

        public List<ProjectViewModel> ProjectViewModels { get; set; }

    }

    public class ProjectViewModel 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Select Account")]
        public int AccountId { get; set; }

        public string ProjectId { get; set; }
        public int EmployeeId { get; set; }
       // [Required]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Please Enter Project Name")]
        public string ProjectName { get; set; }
        public int Total_Positions { get; set; }

        /* [ForeignKey("AccountsBRModel")]
         public int AccountId { get; set; }
         public AccountsBRModel AccountsBRModel { get; set; }
 */

        [JsonProperty("AccountViewModel")]
        public AccountViewModel AccountViewModel { get; set; }
        public List<AccountViewModel> AccountViewModels { get; set; }



    }

    public class DomainViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Domain Name")]
        public string DomainName { get; set; }

        public string ProjectId { get; set; }
        [Required]
        public string ProjectName { get; set; }

       
      /*  public int AccountId { get; set; }
        public AccountViewModel AccountViewModel { get; set; }
*/
       // [ForeignKey("SignUpModel")]
        public int EmployeeId { get; set; }
        //  public SignUpViewModel SignUpViewModel { get; set; }

        [Required(ErrorMessage = "No_Of_Positions is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "No_Of_Positions must be greater than 0.")]
        public int No_Of_Positions { get; set; }

        public string Added_Modified_By { get; set; }

       // [ForeignKey("ProjectsBRModel")]
        public int ProjectFkId { get; set; }
        public ProjectViewModel ProjectsViewModel { get; set; }
        public DateTime ApprovedDate { get; set; }
        [Required(ErrorMessage = "Please Select Grade")]
        public string Grade { get; set; }
        public string SkillSetRequired { get; set; }

        [Required(ErrorMessage = "Please Select Status")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Please Enter Job Description")]
        public string JobDescription { get; set; }
      //  [Required(ErrorMessage = "No_Of_Positions is required.")]
       // [Range(0, int.MaxValue, ErrorMessage = "Approval date must be today or past.")]
        public int Age { get; set; }

        public List<ProjectViewModel> ProjectViewModels { get; set; }
        public List<AccountViewModel> AccountViewModels { get; set; }
        public SignUpViewModel SignUpViewModel { get; set; }

    }
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required]
        public string AccountName { get; set; }
        public string AccountId { get; set; }
    }


    public class Proj_Acc_Dto
    {
      public  List<ProjectViewModel> projectViewModels { get; set; }
       public List<AccountViewModel> accountViewModels
        {
            get; set;
        }
        public string Status { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }

        public string ProjectId { get; set; }
        public int ProjectFkId { get; set; }

    }


    public class SignUpViewModel
    {

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Employee Name")]
        [MaxLength(50)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Emp name should be minimum of 3 characters and maximum of 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the Email")]
        [EmailAddress(ErrorMessage = "please enter a valid email address")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Please enter the Password")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password should be atleast 4 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password is too short.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }


      
      
    }

    public class LoginViewModel
    {

        [Required(ErrorMessage = "Please enter your name")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter your password")]
        public string Password { get; set; }
    }


    public class GreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public GreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (comparisonValue == null || !(comparisonValue is IComparable))
            {
                return ValidationResult.Success;
            }

            if (value == null || !(value is IComparable))
            {
                return ValidationResult.Success;
            }

            if (((IComparable)value).CompareTo(comparisonValue) <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be greater than {_comparisonProperty}.");
            }

            return ValidationResult.Success;
        }
    }

}
