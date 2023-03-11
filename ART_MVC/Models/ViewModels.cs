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
        public string ProjectId { get; set; }
        public string CandidateId { get; set; }
        //  public ProjectsBRModel ProjectsBRModel { get; set; }
        /*  public int AccountsBRModelId { get; set; }
          public AccountsBRModel AccountsBRModel { get; set; }
  */
        [Required]
        public string CandidateName { get; set; }
        public string Int_Ext { get; set; }
        public string Location { get; set; }
        public string Source { get; set; }
        public string Grade { get; set; }
        public string SkillSetRequired { get; set; }

        public string JobDescription { get; set; }
        public int Age { get; set; }

        public DateTime? ScreeningDate { get; set; } 
        public string ScreeningResult { get; set; }

        public DateTime? L1_Eval_Date { get; set; } 
        public string L1_Eval_Result { get; set; }

        public DateTime? Client_Eval_Date { get; set; } 
        public string Client_Eval_Result { get; set; }

        public DateTime? Manager_Eval_Date { get; set; } 
        public string Manager_Eval_Result { get; set; }

        public string Status { get; set; }
        public string Eval_Comments { get; set; }

        public List<ProjectViewModel> ProjectViewModels { get; set; }

    }

    public class ProjectViewModel 
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public string ProjectId { get; set; }
        public int EmployeeId { get; set; }
       // [Required]
        public string ProjectName { get; set; }

       /* [ForeignKey("AccountsBRModel")]
        public int AccountId { get; set; }
        public AccountsBRModel AccountsBRModel { get; set; }
*/
        public DateTime ApprovedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "please enter number of positions required")]
        public int No_Of_Positions { get; set; }
        public string Grade { get; set; }
        public string SkillSetRequired { get; set; }

        public string Status { get; set; }
        public string JobDescription { get; set; }
        [JsonProperty("AccountViewModel")]
        public AccountViewModel AccountViewModel { get; set; }
        public List<AccountViewModel> AccountViewModels { get; set; }



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

}
