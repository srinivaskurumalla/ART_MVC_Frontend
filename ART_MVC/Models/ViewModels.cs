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

        public DateTime ScreeningDate { get; set; } = DateTime.Now;
        public string ScreeningResult { get; set; }
        public DateTime L1_Eval_Date { get; set; } = DateTime.Now;
        public string L1_Eval_Result { get; set; }

        public DateTime Client_Eval_Date { get; set; } = DateTime.Now;
        public string Client_Eval_Result { get; set; }

        public DateTime Manager_Eval_Date { get; set; } = DateTime.Now;
        public string Manager_Eval_Result { get; set; }

        public string Status { get; set; }
        public string Eval_Comments { get; set; }

        public List<ProjectViewModel> ProjectViewModels { get; set; }

    }

    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string ProjectId { get; set; }
       // [Required]
        public string ProjectName { get; set; }

       /* [ForeignKey("AccountsBRModel")]
        public int AccountId { get; set; }
        public AccountsBRModel AccountsBRModel { get; set; }
*/
        public DateTime ApprovedDate { get; set; } = DateTime.Now;

        public string Grade { get; set; }
        public string SkillSetRequired { get; set; }

        public string Status { get; set; }
        public string JobDescription { get; set; }




    }
}
