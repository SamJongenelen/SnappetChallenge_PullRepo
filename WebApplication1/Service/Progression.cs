using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    [RoutePrefix("api")]
    public class ProgressController : ApiController
    {
        [HttpGet]
        public object GetAllProgress()
        {
            #region  working stubdata:
            //var bla = "{                 \"sEcho\": 3,     \"iTotalRecords\": 57,     \"iTotalDisplayRecords\": 57,     \"aaData\": [         {             \"DT_RowId\": \"row_7\",             \"DT_RowClass\": \"gradeA\",             \"0\": \"Gecko\",             \"1\": \"Firefox 1.0\",             \"2\": \"Win 98+ / OSX.2+\",             \"3\": \"1.7\",             \"4\": \"A\"         },         {             \"DT_RowId\": \"row_8\",             \"DT_RowClass\": \"gradeA\",             \"0\": \"Gecko\",             \"1\": \"Firefox 1.5\",             \"2\": \"Win 98+ / OSX.2+\",             \"3\": \"1.8\",             \"4\": \"A\"         }            ] }";
            //JObject json = JObject.Parse(bla);
            //return json;
            #endregion

            var dt = new DateTime(2015, 3, 24, 11, 30, 0); //requirement; cant show data from the future, Marty
            DateTime dateTimeEnd = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            DateTime dateTimeStart = DateTime.SpecifyKind(dt.AddDays(-7), DateTimeKind.Utc);

            var model = ProgressBetween(dateTimeStart, dateTimeEnd);

            var dtr = new DataTableResult<StudentProgression>();
            dtr.aaData = model;
            dtr.sEcho = 3;
            dtr.iTotalRecords = model.Count;
            dtr.iTotalDisplayRecords = 2;

            return dtr;
        }

        public List<StudentProgression> ProgressBetween(DateTime dateStart, DateTime dateEnd)
        {
            //todo: refactor los van de view laag

            var progressList = new List<StudentProgression>();

            //progressList.Add(new StudentProgression { CorrectAnswerRate = 1, DifficultyOfExercises = 2, NumberOfExercises = 3, Progresss = 1 });
            //progressList.Add(new StudentProgression { CorrectAnswerRate = 2, DifficultyOfExercises = 3, NumberOfExercises = 1, Progresss = 1 });

            //todo: get progress
            var controller = new ProgressionController();
            var progress= controller.GetProgress(dateStart, dateEnd);
          
            return progress;
        }
    }
}
